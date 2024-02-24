using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Chunk.Collection
{
    public class ChunkCollection : IChunkCollection
    {
        public event Action<ChunkModel> OnChunkAdded;
        public event Action<string> OnChunkRemoved;
        
        public static readonly Vector3 ChunkSize = new(250f, 250f, 250f);
        public static readonly Vector3 WorldSize = new(5000f, 5000f, 5000f);

        public long Seed { get; }
        public Vector3 StartPoint { get; }
        public Dictionary<string, ChunkModel> Chunks { get; } = new();
        public float DestroyRate => .1f;

        public List<string> ActiveChunks
        {
            get
            {
                var newList = new List<string>();

                foreach (var chunk in Chunks)
                {
                    if (chunk.Value.RenderingState.Value == ChunkRenderingState.Enabled)
                    {
                        newList.Add(chunk.Key);
                    }
                }

                return newList;
            }
        }


        public ChunkModel this[string key] => Chunks[key];

        public ChunkCollection(Vector3 startPoint, long seed = 0)
        {
            StartPoint = startPoint;
            Seed = seed == 0 ? (long)Random.Range(0, long.MaxValue) : seed;
        }

        public void AddChunk(Vector2 position)
        {
            var id = $"{position.x}-{position.y}";
            
            if (Chunks.ContainsKey(id)) return;

            var chunkModel = new ChunkModel(id, position);

            Chunks.Add(id, chunkModel);
            OnChunkAdded?.Invoke(chunkModel);
        }

        public void RemoveChunk(string id)
        {
            if (!Chunks.ContainsKey(id))
            {
                return;
            }

            Chunks.Remove(id);
            OnChunkRemoved?.Invoke(id);
        }
    }
}