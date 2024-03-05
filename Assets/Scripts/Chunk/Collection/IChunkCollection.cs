using System.Collections.Generic;
using Biome;
using UnityEngine;

namespace Chunk.Collection
{
    public interface IChunkCollection
    {
        void Add(ChunkModel model);
        void Add(string biomeId, BiomeType biomeType, Vector2 position);
        IEnumerable<string> GetActiveChunks();
        ChunkModel this[string key] { get; }
    }
}