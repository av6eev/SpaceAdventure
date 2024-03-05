namespace Biome.Collection
{
    public interface IBiomeCollection
    {
        void Add(string id, BiomeType type);
        BiomeType GetBiomeType(string id);
    }
}