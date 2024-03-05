namespace Biome
{
    public abstract class Biome : IBiome
    {
        public string Id { get; }
        public BiomeType Type { get; protected set; }

        protected Biome(string id)
        {
            Id = id;
        }
    }

    public interface IBiome
    {
        string Id { get; }
        BiomeType Type { get; }
    }
}