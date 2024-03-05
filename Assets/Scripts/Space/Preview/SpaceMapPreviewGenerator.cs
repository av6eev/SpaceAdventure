using System.Collections;
using System.Collections.Generic;
using Biome;
using UnityEngine;

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

        public SpaceMapGraph SpaceMapGraph { get; private set; }
        
        private void OnDrawGizmos()
        {
            if (SpaceMapGraph == null)
            {
                return;
            }

            if (SpaceMapGraph.NodesByCenterPosition.Count == 0)
            {
                return;
            }
            
            DrawNodes();

            if (IsDrawDelauneyTriangles)
            {
                DrawDelauneyTriangles();
            }

            if (IsDrawNodeBoundaries)
            {
                DrawBoundaries();
            }
        }

        private void DrawBoundaries()
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

        private void DrawNodes()
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

        private void DrawDelauneyTriangles()
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

        private void Start()
        {
            StartCoroutine(GenerateMapAsync());
        }

        private IEnumerator GenerateMapAsync()
        {
            yield return new WaitForSeconds(1f);
            GenerateMap();
        }

        public void GenerateMap()
        {
            GenerateInternal();
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

        private void GenerateInternal()
        {
            SpaceMapGraph = null;
            SpaceMapGraph = SpaceMapGenerator.Generate(MapSize, RelaxationIterations, SnapDistance, Seed);
        }
    }
}