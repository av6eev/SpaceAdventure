using System;
using Entities.Asteroids.Asteroid;
using Specifications.Asteroid;

namespace Entities.Asteroids.Collection
{
    public interface IAsteroidsCollection
    {
        event Action<AsteroidModel, bool, bool> OnAsteroidDestroyed;
        
        float SpawnRate { get; }
        float SpeedShift { get; }

        void CreateAsteroid(AsteroidSpecification specification);
        void DestroyAsteroid(AsteroidModel model, bool byBorder, bool byShip);
        void UpdateModifiers(float spawnRateShift, float speedShift);
    }
}