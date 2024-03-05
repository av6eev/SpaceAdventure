using Biome.Collection;
using Chunk.Collection;
using Entities.Asteroids.Collection;

namespace Space
{
    public interface ISpaceModel
    {
        IChunkCollection ChunkCollection { get; }
        IAsteroidCollection AsteroidCollection { get; set; }
        IBiomeCollection BiomeCollection { get; }
    }
}