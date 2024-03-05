using Chunk.Collection;
using UnityEngine;

namespace Space
{
    public class SpaceView : MonoBehaviour, ISpaceView
    {
        public ChunkCollectionView ChunkCollectionViewGo;
        public IChunkCollectionView ChunkCollectionView => ChunkCollectionViewGo;
    }
}