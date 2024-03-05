using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;

namespace Space.Preview
{
    public static class SpaceMapSaver
    {
        public static void Save(SpaceMapGraph graph, int mapSize, int relaxationIterations, float snapDistance, int seed)
        {
            SaveInternal(graph, mapSize, relaxationIterations, snapDistance, seed);
            
            Debug.Log("MAP SAVED SUCCESSFULLY");
        }

        private static void SaveInternal(SpaceMapGraph graph, int mapSize, int relaxationIterations, float snapDistance, int seed)
        {
            PlayerPrefs.DeleteKey(SavingElementsKeys.GeneratedSpaceMapKey);
            
            IDictionary<string, object> json = new Dictionary<string, object>
            {
                { SavingElementsKeys.GeneratedSpaceMapSeedKey, seed.ToString() },
                { SavingElementsKeys.GeneratedSpaceMapSizeKey, mapSize.ToString() },
                { SavingElementsKeys.GeneratedSpaceMapRelaxationIterationsKey, relaxationIterations.ToString() },
                { SavingElementsKeys.GeneratedSpaceMapSnapDistanceKey, snapDistance.ToString() }
            };
            
            foreach (var node in graph.NodesByCenterPosition)
            {
                IDictionary<string, object> array = new Dictionary<string, object>
                {
                    {SavingElementsKeys.GeneratedSpaceMapNodeBiomeTypeKey, node.Value.BiomeType.ToString()},
                    {SavingElementsKeys.GeneratedSpaceMapNodeBiomeIdKey, node.Value.BiomeId},
                };

                var positionX = Mathf.Floor(node.Key.x);
                var positionZ = Mathf.Floor(node.Key.z);
                
                json[$"{positionX}_{positionZ}"] = array;
            }
            
            PlayerPrefs.SetString(SavingElementsKeys.GeneratedSpaceMapKey, JsonConvert.SerializeObject(json));
        }
    }
}