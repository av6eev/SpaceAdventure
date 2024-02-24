using Entities.Asteroids.Asteroid;

namespace Entities.Asteroids.Collection
{
    public interface IAsteroidsCollection
    {
        float SpawnRate { get; }
        float SpeedShift { get; }
        int Count { get; }
        AsteroidModel this[string key] { get; }
        void UpdateModifiers(float spawnRateShift, float speedShift);
        void CreateAsteroid(AsteroidModel model);
        void DestroyAsteroid(AsteroidModel model);
    }
}