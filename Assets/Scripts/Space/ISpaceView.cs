using Chunk.Collection;

namespace Space
{
    public interface ISpaceView
    {
        IChunkCollectionView ChunkCollectionView { get; }
    }
}