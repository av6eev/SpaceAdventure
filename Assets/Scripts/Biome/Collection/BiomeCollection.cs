using System.Collections.Generic;
using Biome.InnerMeteorCircle;
using Biome.MeteorCircle;
using Biome.Void;

namespace Biome.Collection
{
    public class BiomeCollection : IBiomeCollection
    {
        public readonly Dictionary<string, Biome> Biomes = new();

        public void Add(string id, BiomeType type)
        {
            if (Biomes.ContainsKey(id))
            {
                return;
            }

            Biome newBiome = null;
            
            switch (type)
            {
                case BiomeType.Void:
                    newBiome = new VoidBiome(id);
                    break;
                case BiomeType.MeteorCircle:
                    newBiome = new MeteorCircleBiome(id);
                    break;
                case BiomeType.InnerMeteorCircle:
                    newBiome = new InnerMeteorCircleBiome(id);
                    break;
            }
            
            Biomes.Add(id, newBiome);
        }

        public BiomeType GetBiomeType(string id)
        {
            if (Biomes.TryGetValue(id, out var biome))
            {
                return biome.Type;
            }

            return BiomeType.Uncertain;
        }
    }
}