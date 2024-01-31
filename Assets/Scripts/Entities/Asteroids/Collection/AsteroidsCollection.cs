using System;
using System.Collections.Generic;
using Entities.Asteroids.Asteroid;
using Specifications.Asteroid;

namespace Entities.Asteroids.Collection
{
    public class AsteroidsCollection : IAsteroidsCollection
    {
        public event Action<AsteroidModel, bool, bool> OnAsteroidDestroyed;
        public event Action<AsteroidModel> OnAsteroidCreated;

        public readonly Dictionary<string, AsteroidSpecification> Specifications;
        public readonly List<AsteroidModel> ActiveAsteroids = new();

        public float SpawnRate { get; private set; } = .5f;
        public float SpeedShift { get; private set; }
        public int MaxCount => 70;

        public AsteroidsCollection(Dictionary<string, AsteroidSpecification> specifications)
        {
            Specifications = specifications;
        }

        public void UpdateModifiers(float spawnRateShift, float speedShift)
        {
            SpeedShift = speedShift;
            SpawnRate *= spawnRateShift;
        }

        public void DestroyAsteroid(AsteroidModel model, bool byBorder, bool byShip)
        {
            ActiveAsteroids.Remove(model);
            OnAsteroidDestroyed?.Invoke(model, byBorder, byShip);
        }

        public void CreateAsteroid(AsteroidSpecification specification)
        {
            var newModel = new AsteroidModel(specification, SpeedShift);
            ActiveAsteroids.Add(newModel);

            OnAsteroidCreated?.Invoke(newModel);
        }
    }
}