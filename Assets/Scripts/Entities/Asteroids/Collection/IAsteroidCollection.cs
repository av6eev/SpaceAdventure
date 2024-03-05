using Entities.Asteroids.Asteroid;

namespace Entities.Asteroids.Collection
{
    public interface IAsteroidCollection
    {
        float SpawnRate { get; }
        float SpeedShift { get; }
        int Count { get; }
        void UpdateModifiers(float spawnRateShift, float speedShift);
        void CreateAsteroid(AsteroidModel model);
        void DestroyAsteroid(AsteroidModel model);
    }
}