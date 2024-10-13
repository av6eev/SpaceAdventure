using System;
using System.Collections;
using System.Collections.Generic;
using Biome;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Space.Preview
{
    public class SpaceMapPreviewGenerator : MonoBehaviour
    {
        public int Seed;
        public bool IsAutoUpdate;

        [Header("Map Settings")] 
        public int MapSize = 200;
        public bool IsDrawNodeBoundaries;
        public bool IsDrawDelauneyTriangles;

        [Header("Biomes Settings")]
        public List<SpaceMapChunkTypeColor> BiomeColors;

        [Header("Voronoi Generation Settings")] 
        public int RelaxationIterations = 1;
        public float SnapDistance;

        [Header("Interface")] 
        public TMP_InputField RelaxationIterationsInputField;
        public TMP_InputField SnapDistanceInputField;
        public Toggle ShowBoundaries;
        public Toggle ShowDelauney;
        public Button GenerateBtn;
        public Button ClearBtn;

        private Material _lineMaterial;
        private readonly List<GameObject> _spawnedNodes = new();
        private readonly List<LineRenderer> _spawnedDelauneyLines = new();
        private readonly List<LineRenderer> _spawnedBoundariesLines = new();

        public SpaceMapGraph SpaceMapGraph { get; private set; }

        private void Awake()
        {
            _lineMaterial = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));

            RelaxationIterationsInputField.onValueChanged.AddListener(HandleRelaxationIterationsChange);
            SnapDistanceInputField.onValueChanged.AddListener(HandleSnapDistanceChange);
            ShowBoundaries.onValueChanged.AddListener(HandleShowBoundariesToggle);
            ShowDelauney.onValueChanged.AddListener(HandleShowDelauneyToggle);
            GenerateBtn.onClick.AddListener(GenerateMap);
            ClearBtn.onClick.AddListener(ClearPreviousData);
        }

        private void OnDisable()
        {
            RelaxationIterationsInputField.onValueChanged.RemoveListener(HandleRelaxationIterationsChange);
            SnapDistanceInputField.onValueChanged.RemoveListener(HandleSnapDistanceChange);
            ShowBoundaries.onValueChanged.RemoveListener(HandleShowBoundariesToggle);
            ShowDelauney.onValueChanged.RemoveListener(HandleShowDelauneyToggle);
            GenerateBtn.onClick.RemoveListener(GenerateMap);
            ClearBtn.onClick.RemoveListener(ClearPreviousData);
        }

        private void HandleShowBoundariesToggle(bool state) => IsDrawNodeBoundaries = state;
        private void HandleShowDelauneyToggle(bool state) => IsDrawDelauneyTriangles = state;
        private void HandleSnapDistanceChange(string value) => SnapDistance = Convert.ToSingle(value);
        private void HandleRelaxationIterationsChange(string value) => RelaxationIterations = Convert.ToInt32(value);

        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying) return;
            if (SpaceMapGraph == null) return;
            if (SpaceMapGraph.NodesByCenterPosition.Count == 0) return;
            
            DrawNodesEditor();

            if (IsDrawDelauneyTriangles)
            {
                DrawDelauneyTrianglesEditor();
            }

            if (IsDrawNodeBoundaries)
            {
                DrawBoundariesEditor();
            }
        }

        private IEnumerator DrawBoundaries()
        {
            foreach (var edge in SpaceMapGraph.Edges)
            {
                var startPosition = edge.GetStartPosition();
                var endPosition = edge.GetEndPosition();

                if (edge.Node.BiomeType == BiomeType.Void)
                {
                    Gizmos.color = Color.black;
                }
                
                _spawnedBoundariesLines.Add(DrawLine(startPosition, endPosition, Color.white, Color.white));
                
                yield return new WaitForSeconds(.008f);
            }
        }
        
        private void DrawBoundariesEditor()
        {
            foreach (var edge in SpaceMapGraph.Edges)
            {
                var start = edge.GetStartPosition();
                var end = edge.GetEndPosition();

                if (edge.Node.BiomeType == BiomeType.Void)
                {
                    Gizmos.color = Color.black;
                }
                
                Gizmos.DrawLine(start, end);
            }
        }

        private void DrawNodesEditor()
        {
            foreach (var node in SpaceMapGraph.NodesByCenterPosition)
            {
                switch (node.Value.BiomeType)
                {
                    case BiomeType.Void:
                        Gizmos.color = BiomeColors[0].Color;
                        break;
                    case BiomeType.MeteorCircle:
                        Gizmos.color = BiomeColors[1].Color;
                        break;
                    case BiomeType.InnerMeteorCircle:
                        Gizmos.color = BiomeColors[2].Color;
                        break;
                }
                
                Gizmos.DrawCube(node.Key, new Vector3(25f,25f,25f));
            }
        }
        
        private IEnumerator DrawNodes()
        {
            foreach (var node in SpaceMapGraph.NodesByCenterPosition)
            {
                var color = Color.grey;
                
                switch (node.Value.BiomeType)
                {
                    case BiomeType.Void:
                        color = BiomeColors[0].Color;
                        break;
                    case BiomeType.MeteorCircle:
                        color = BiomeColors[1].Color;
                        break;
                    case BiomeType.InnerMeteorCircle:
                        color = BiomeColors[2].Color;
                        break;
                }
                
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = node.Key;
                go.transform.localScale = new Vector3(50f, 50f, 50f);
                go.GetComponent<MeshRenderer>().material.color = color;
                
                _spawnedNodes.Add(go);
                
                yield return new WaitForSeconds(.03f);
            }

            yield return null;
        }

        private void DrawDelauneyTrianglesEditor()
        {
            if (SpaceMapGraph.Edges.Count == 0)
            {
                return;
            }

            foreach (var edge in SpaceMapGraph.Edges)
            {
                if (edge.Opposite == null)
                {
                    continue;
                }
                
                var start = edge.Node.CenterPoint;
                var end = edge.Opposite.Node.CenterPoint;

                Gizmos.color = Color.black;
                
                Gizmos.DrawLine(start, end);
            }
        }

        private IEnumerator DrawDelauneyTriangles()
        {
            if (SpaceMapGraph.Edges.Count == 0)
            {
                yield return null;
            }

            foreach (var edge in SpaceMapGraph.Edges)
            {
                if (edge.Opposite == null)
                {
                    continue;
                }
                
                var startPosition = edge.Node.CenterPoint;
                var endPosition = edge.Opposite.Node.CenterPoint;

                _spawnedDelauneyLines.Add(DrawLine(startPosition, endPosition, Color.cyan, Color.cyan));
                
                yield return new WaitForSeconds(.008f);
            }
        }

        public void GenerateMap()
        {
            GenerateInternal();
        }

        private void GenerateInternal()
        {
            ClearPreviousData();
            
            SpaceMapGraph = null;
            SpaceMapGraph = SpaceMapGenerator.Generate(MapSize, RelaxationIterations, SnapDistance, Seed);

            if (!EditorApplication.isPlaying) return;
            
            StartCoroutine(DrawNodes());
            
            if (IsDrawDelauneyTriangles)
            {
                StartCoroutine(DrawDelauneyTriangles());
            }

            if (IsDrawNodeBoundaries)
            {
                StartCoroutine(DrawBoundaries());
            }
        }

        private void ClearPreviousData()
        {
            StopAllCoroutines();
            
            if (_spawnedNodes.Count > 0)
            {
                foreach (var go in _spawnedNodes)
                {
                    Destroy(go);
                }
            }
            
            if (_spawnedDelauneyLines.Count > 0)
            {
                foreach (var go in _spawnedDelauneyLines)
                {
                    Destroy(go.gameObject);
                }
            }
            
            if (_spawnedBoundariesLines.Count > 0)
            {
                foreach (var go in _spawnedBoundariesLines)
                {
                    Destroy(go.gameObject);
                }
            }
            
            _spawnedNodes.Clear();
            _spawnedDelauneyLines.Clear();
            _spawnedBoundariesLines.Clear();
        }

        private LineRenderer DrawLine(Vector3 startPosition, Vector3 endPosition, Color startColor, Color endColor)
        {
            var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer.material = _lineMaterial;
            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;    
                
            lineRenderer.SetPosition(0, startPosition); 
            lineRenderer.SetPosition(1, endPosition);

            return lineRenderer;
        }

        public void SaveMap()
        {
            SpaceMapSaver.Save(SpaceMapGraph, MapSize, RelaxationIterations, SnapDistance, Seed);
        }

        public void LoadMapFromFile()
        {
            SpaceMapGraph = null;
            SpaceMapGraph = SpaceMapLoader.Load();
        }
    }
}