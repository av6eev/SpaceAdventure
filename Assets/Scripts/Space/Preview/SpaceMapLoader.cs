using System;
using System.Collections.Generic;
using System.Linq;
using Biome;
using Chunk.Collection;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;
using Utilities.Voronoi;

namespace Space.Preview
{
    public static class SpaceMapLoader
    {
        private static readonly Dictionary<Vector3, SpaceMapNode> _loadedNodesByCenter = new();
        private static SpaceMapGraph _spaceMapGraph;

        public static SpaceMapGraph Load()
        {
            ClearPreviousData();
            LoadInternal();
            SetAllUndetermined();
            LoadMeteorCircleNodes();

            Debug.Log("SAVED MAP LOADED SUCCESSFULLY");

            return _spaceMapGraph;
        }

        private static void LoadInternal()
        {
            var jsonString = JsonConvert.DeserializeObject<IDictionary<string, object>>(PlayerPrefs.GetString(SavingElementsKeys.GeneratedSpaceMapKey));
            var points = new List<Vector2>();
            var seed = 0;
            var mapSize = 0;
            var relaxationIterations = 0;
            var snapDistance = 0;
            
            foreach (var node in jsonString)
            {
                switch (node.Key)
                {
                    case SavingElementsKeys.GeneratedSpaceMapSeedKey:
                        seed = Convert.ToInt32(node.Value);
                        break;
                    case SavingElementsKeys.GeneratedSpaceMapSizeKey:
                        mapSize = Convert.ToInt32(node.Value);
                        break;
                    case SavingElementsKeys.GeneratedSpaceMapRelaxationIterationsKey:
                        relaxationIterations = Convert.ToInt32(node.Value);
                        break;
                    case SavingElementsKeys.GeneratedSpaceMapSnapDistanceKey:
                        snapDistance = Convert.ToInt32(node.Value);
                        break;
                    default:
                        var tokens = node.Key.Split('_');
                        var center = new Vector3(Convert.ToInt32(tokens[0]), 0, Convert.ToInt32(tokens[1]));
                        var position = new Vector2(center.x, center.z);
                        
                        points.Add(position);
                        _loadedNodesByCenter.Add(center, FillNodeData(center, node));
                        break;
                }
            }
            
            var voronoi = new Voronoi(points, new Rect(0, 0, mapSize, mapSize), relaxationIterations);
            _spaceMapGraph = new SpaceMapGraph(voronoi, snapDistance);
        }

        private static void SetAllUndetermined()
        {
            foreach (var node in _spaceMapGraph.NodesByCenterPosition.Values)
            {
                node.BiomeType = BiomeType.Void;
                _spaceMapGraph.VoidNodes.Add(node);
            }
        }

        private static void LoadMeteorCircleNodes()
        {
            foreach (var mapNode in _spaceMapGraph.NodesByCenterPosition)
            {
                var generatedNodePosition = mapNode.Key;
                var savedNode = _loadedNodesByCenter.Values.First(node => CheckNodePositionsAreEquals(generatedNodePosition, node.CenterPoint, ChunkCollection.ChunkSize.x, ChunkCollection.ChunkSize.z));

                mapNode.Value.BiomeId = savedNode.BiomeId;
                mapNode.Value.BiomeType = savedNode.BiomeType;
                
                switch (savedNode.BiomeType)
                {
                    case BiomeType.Void:
                        _spaceMapGraph.VoidNodes.Remove(mapNode.Value);
                        _spaceMapGraph.VoidNodes.Add(mapNode.Value);
                        break;
                    case BiomeType.MeteorCircle:
                        _spaceMapGraph.AddNodeToList(savedNode.BiomeId, mapNode.Value);
                        _spaceMapGraph.VoidNodes.Remove(mapNode.Value);
                        break;
                    case BiomeType.InnerMeteorCircle:
                        _spaceMapGraph.AddNodeToList(savedNode.BiomeId, mapNode.Value);
                        _spaceMapGraph.VoidNodes.Remove(mapNode.Value);
                        break;
                }
            }
        }

        private static void ClearPreviousData()
        {
            _spaceMapGraph = null;
            _loadedNodesByCenter.Clear();
        }

        private static SpaceMapNode FillNodeData(Vector3 center, KeyValuePair<string, object> node)
        {
            var resultNode = JsonConvert.DeserializeObject<IDictionary<string, object>>(node.Value.ToString());
            var biomeId = string.Empty;
            var biomeType = BiomeType.Void;
            
            foreach (var element in resultNode)
            {
                switch (element.Key)
                {
                    case SavingElementsKeys.GeneratedSpaceMapNodeBiomeTypeKey:
                        var isParsed = Enum.TryParse(element.Value.ToString(), out BiomeType type);
                        biomeType =  isParsed ? type : BiomeType.Void;
                        break;
                    case SavingElementsKeys.GeneratedSpaceMapNodeBiomeIdKey:
                        biomeId = element.Value.ToString();
                        break;
                }
            }

            return new SpaceMapNode { CenterPoint = center, BiomeType = biomeType, BiomeId = biomeId };
        }
        
        private static bool CheckNodePositionsAreEquals(Vector3 v1, Vector3 v2, float precisionX, float precisionZ)
        {
            if (Mathf.Abs(v1.x - v2.x) > precisionX)
            {
                return false;
            }

            if (Mathf.Abs(v1.z - v2.z) > precisionZ)
            {
                return false;
            }

            return true;
        }
    }
}