using System;
using System.Collections.Generic;
using Biome;
using UnityEngine;

namespace Chunk.Collection
{
    public class ChunkCollection : IChunkCollection
    {
        public event Action<ChunkModel> OnChunkAdded;
        public event Action<string> OnChunkRemoved;
        
        public static readonly Vector3 ChunkSize = new(750f, 250f, 750f);
        public static readonly Vector3 WorldSize = new(5000f, 5000f, 5000f);

        public Dictionary<string, ChunkModel> Chunks { get; } = new();
        public float DestroyRate => .1f;
        
        public ChunkModel this[string key] => Chunks[key];

        public void Add(string biomeId, BiomeType biomeType, Vector2 position)
        {
            var id = $"{position.x}-{position.y}";
            
            if (Chunks.ContainsKey(id)) return;

            var chunkModel = new ChunkModel(position, biomeId, biomeType);

            Chunks.Add(id, chunkModel);
            OnChunkAdded?.Invoke(chunkModel);
        }
        
        public void Add(ChunkModel model)
        {
            if (!Chunks.TryAdd(model.Id, model))
            {
                return;
            }

            OnChunkAdded?.Invoke(model);
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

        public IEnumerable<string> GetActiveChunks()
        {
            foreach (var chunk in Chunks)
            {
                if (chunk.Value.RenderingState.Value == ChunkRenderingState.Enabled)
                {
                    yield return chunk.Key;
                }
            }
        }
    }
}