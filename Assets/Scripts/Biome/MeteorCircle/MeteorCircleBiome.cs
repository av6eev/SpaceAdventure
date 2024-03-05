namespace Biome.MeteorCircle
{
    public class MeteorCircleBiome : Biome
    {
        public static readonly float OuterRadius = 2500;
        public static readonly float InnerRadius = 1600;
        
        public MeteorCircleBiome(string id) : base(id)
        {
            Type = BiomeType.MeteorCircle;
        }
    }
}