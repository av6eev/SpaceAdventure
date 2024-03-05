using Biome.Collection;
using Chunk.Collection;
using Entities.Asteroids.Collection;

namespace Space
{
    public class SpaceModel : ISpaceModel
    {
        public IChunkCollection ChunkCollection { get; set; }
        public IAsteroidCollection AsteroidCollection { get; set; }
        public IBiomeCollection BiomeCollection { get; set; }
    }
}