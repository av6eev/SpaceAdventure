namespace Chunk
{
    public interface IChunkView
    {
        void Enable();
        void Disable();
        void Prepare();
        void Dispose();
    }
}