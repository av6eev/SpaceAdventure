using System.Collections.Generic;
using UnityEngine;

namespace Chunk.Collection
{
    public interface IChunkCollection
    {
        long Seed { get; }
        Vector3 StartPoint { get; }
        Dictionary<string, ChunkModel> Chunks { get; }
        List<string> ActiveChunks { get; }
        ChunkModel this[string key] { get; }
    }
}