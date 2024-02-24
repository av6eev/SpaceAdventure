using UnityEngine;

namespace Chunk.Collection
{
    public interface IChunkCollectionView
    {
        IChunkView InstantiateChunkView(Vector2 position, string goName);
    }
}