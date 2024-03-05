using System;
using System.Collections.Generic;
using Entities.Asteroids.Asteroid;
using Specifications.Asteroid;

namespace Entities.Asteroids.Collection
{
    public class AsteroidCollection : IAsteroidCollection
    {
        public event Action<AsteroidModel> OnAsteroidDestroyed;
        public event Action<AsteroidModel> OnAsteroidCreated;

        public readonly Dictionary<string, AsteroidSpecification> Specifications;
        public readonly Dictionary<string, AsteroidModel> Asteroids = new();

        public AsteroidModel this[string key] => Asteroids[key];
        public int Count => Asteroids.Count;
        public float SpawnRate { get; private set; } = .1f;
        public float SpeedShift { get; private set; }

        public const int MaxCount = 50;

        public AsteroidCollection(Dictionary<string, AsteroidSpecification> specifications)
        {
            Specifications = specifications;
        }

        public void UpdateModifiers(float spawnRateShift, float speedShift)
        {
            SpeedShift = speedShift;
            SpawnRate *= spawnRateShift;
        }

        public void CreateAsteroid(AsteroidSpecification specification)
        {
            var newModel = new AsteroidModel(specification, SpeedShift);

            Asteroids.Add(newModel.Id, newModel);
            OnAsteroidCreated?.Invoke(newModel);
        }
        
        public void CreateAsteroid(AsteroidModel model)
        {
            Asteroids.Add(model.Id, model);
            OnAsteroidCreated?.Invoke(model);
        }

        public void DestroyAsteroid(AsteroidModel model)
        {
            Asteroids.Remove(model.Id);
            OnAsteroidDestroyed?.Invoke(model);
        }
    }
}