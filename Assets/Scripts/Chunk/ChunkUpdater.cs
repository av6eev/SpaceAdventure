using Updater;

namespace Chunk
{
    public class ChunkUpdater : IUpdater
    {
        private readonly ChunkModel _chunkModel;

        public ChunkUpdater(ChunkModel chunkModel)
        {
            _chunkModel = chunkModel;
        }
        
        public void Update(float deltaTime)
        {
        }
    }
}