using System;
using System.Collections.Generic;
using Biome;
using Biome.MeteorCircle;
using Chunk.Collection;
using UnityEngine;
using Utilities.Voronoi;
using Random = UnityEngine.Random;

namespace Space.Preview
{
    public static class SpaceMapGenerator
    {
        private static SpaceMapGraph _spaceMapGraph;

        public static SpaceMapGraph Generate(int mapSize, int relaxationIterations, float snapDistance, int seed)
        {
            ClearPreviousData();
            GenerateInternal(mapSize, relaxationIterations, snapDistance, seed);
            
            Debug.Log("MAP GENERATED SUCCESSFULLY");

            return _spaceMapGraph;
        }

        private static void GenerateInternal(int mapSize, int relaxationIterations, float snapDistance, int seed)
        {
            var points = VoronoiHelper.GetVector2Points(seed, mapSize / Convert.ToInt32(ChunkCollection.ChunkSize.x) * (mapSize / Convert.ToInt32(ChunkCollection.ChunkSize.x)), mapSize);
            var voronoi = new Voronoi(points, new Rect(0, 0, mapSize, mapSize), relaxationIterations);
            _spaceMapGraph = new SpaceMapGraph(voronoi, snapDistance);
            
            SetAllUndetermined(_spaceMapGraph);
            FindMeteorCircleNodes(_spaceMapGraph);
        }

        private static void SetAllUndetermined(SpaceMapGraph graph)
        {
            foreach (var node in graph.NodesByCenterPosition.Values)
            {
                node.BiomeType = BiomeType.Void;
                graph.VoidNodes.Add(node);
            }
        }

        private static void FindMeteorCircleNodes(SpaceMapGraph graph)
        {
            graph.MeteorCircleNodes.Clear();

            foreach (var node in graph.NodesByCenterPosition.Values)
            {
                if (node.BiomeType == BiomeType.MeteorCircle)
                {
                    continue;
                }

                var random = Random.Range(0f, 1f);
                var chanceToSpawn = BiomeConst.MeteorCircleChance;

                if (graph.MeteorCircleNodes.Count > 0)
                {
                    chanceToSpawn = BiomeConst.MeteorCircleChance / (graph.MeteorCircleNodes.Count * BiomeConst.MeteorCircleCost * 3);
                }

                if (random < chanceToSpawn)
                {
                    var endPointNode = graph.GetClosestNode(node.CenterPoint.x + MeteorCircleBiome.OuterRadius * 2,
                        node.CenterPoint.z + MeteorCircleBiome.OuterRadius * 2, out var offset);

                    if (offset > 200f)
                    {
                        Debug.Log("Closest node is too far");
                        continue;
                    }

                    if (graph.CheckIfNodesInAreaVoid(node.CenterPoint, endPointNode.CenterPoint, out var areaNodes))
                    {
                        var biomeId = GenerateRandomNodeId(BiomeType.MeteorCircle);

                        node.BiomeId = biomeId;
                        graph.AddNodeToList(biomeId, node);
                        graph.VoidNodes.Remove(node);

                        var centerNode = graph.GetClosestNode(node.CenterPoint.x + MeteorCircleBiome.OuterRadius,
                            node.CenterPoint.z + MeteorCircleBiome.OuterRadius, out var centerOffset);
                        CreateMeteorCircleStructure(graph, biomeId, areaNodes, centerNode);
                    }
                }
            }
        }

        private static void CreateMeteorCircleStructure(SpaceMapGraph graph, string biomeId,
            List<SpaceMapNode> areaNodes, SpaceMapNode centerNode)
        {
            var innerBiomeId = GenerateRandomNodeId(BiomeType.InnerMeteorCircle);

            foreach (var node in areaNodes)
            {
                var xSquared = Mathf.Pow(node.CenterPoint.x - centerNode.CenterPoint.x, 2);
                var zSquared = Mathf.Pow(node.CenterPoint.z - centerNode.CenterPoint.z, 2);
                var outerRadiusSquared = Mathf.Pow(MeteorCircleBiome.OuterRadius, 2);
                var innerRadiusSquared = Mathf.Pow(MeteorCircleBiome.InnerRadius, 2);

                if (xSquared + zSquared < outerRadiusSquared)
                {
                    if (xSquared + zSquared > innerRadiusSquared)
                    {
                        node.BiomeType = BiomeType.MeteorCircle;

                        graph.AddNodeToList(biomeId, node);
                        graph.VoidNodes.Remove(node);
                    }
                    else
                    {
                        node.BiomeType = BiomeType.InnerMeteorCircle;
                        node.BiomeId = innerBiomeId;

                        graph.AddNodeToList(innerBiomeId, node);
                        graph.VoidNodes.Remove(node);
                    }
                }
                else
                {
                    node.BiomeType = BiomeType.Void;

                    graph.VoidNodes.Remove(node);
                    graph.VoidNodes.Add(node);
                }
            }
        }

        private static string GenerateRandomNodeId(BiomeType type)
        {
            var id = string.Empty;

            switch (type)
            {
                case BiomeType.Void:
                    id = $"Void-{Random.Range(0, 10000)}-{Random.Range(0, 1000)}";
                    break;
                case BiomeType.MeteorCircle:
                    id = $"MeteorCircle-{Random.Range(0, 100000)}-{Random.Range(0, 10000)}";
                    break;
                case BiomeType.InnerMeteorCircle:
                    id = $"Heart of Meteor Circle-{Random.Range(0, 10000)}-{Random.Range(0, 1000)}";
                    break;
            }

            return id;
        }

        private static void ClearPreviousData()
        {
            _spaceMapGraph = null;
        }
    }
}