using System.Collections.Generic;
using Entities.Asteroids.Asteroid;
using Specifications.Asteroid;
using UnityEngine;
using Updater;

namespace Entities.Asteroids.Collection
{
    public class AsteroidsCollectionUpdater : IUpdater
    {
        private readonly IGameModel _gameModel;
        private readonly AsteroidsCollection _model;

        private readonly List<AsteroidModel> _inactiveModels = new();
        private int _toCreateModelsCount;
        
        public AsteroidsCollectionUpdater(IGameModel gameModel, AsteroidsCollection model)
        {
            _gameModel = gameModel;
            _model = model;
        }
        
        public void Update(float deltaTime)
        {
            RemoveInactiveAsteroids();
            CreateMissingAsteroids();
        }

        private void CreateMissingAsteroids()
        {
            var activeAsteroidsCount = _model.ActiveAsteroids.Count;

            if (activeAsteroidsCount >= _model.MaxCount) return;
            
            _toCreateModelsCount = _model.MaxCount - activeAsteroidsCount;

            if (_toCreateModelsCount == 0) return;

            for (var i = 0; i < _toCreateModelsCount; i++)
            {
                _model.CreateAsteroid(GetNewAsteroidSpecification());
            }

            _toCreateModelsCount = 0;
        }

        private void RemoveInactiveAsteroids()
        {
            foreach (var model in _inactiveModels)
            {
                _model.DestroyAsteroid(model, true, false);
            }
            
            _inactiveModels.Clear();

            foreach (var model in _model.ActiveAsteroids)
            {
                if (!model.IsVisibleFromCamera)
                {
                    _inactiveModels.Add(model);
                }
            }
        }
        
        private AsteroidSpecification GetNewAsteroidSpecification()
        {
            var randomChance = Random.Range(0f, 1f);
            var smallAsteroidSpecification = _model.Specifications["small_asteroid"];
            var mediumAsteroidSpecification = _model.Specifications["medium_asteroid"];
            var bigAsteroidSpecification = _model.Specifications["big_asteroid"];
            var fireAsteroidSpecification = _model.Specifications["fire_asteroid"];
            
            AsteroidSpecification specification = null;

            if (randomChance < smallAsteroidSpecification.ChanceToSpawn)
            {
                specification = fireAsteroidSpecification;
            }

            if (randomChance > fireAsteroidSpecification.ChanceToSpawn &&
                randomChance < bigAsteroidSpecification.ChanceToSpawn)
            {
                specification = bigAsteroidSpecification;
            }

            if (randomChance > bigAsteroidSpecification.ChanceToSpawn &&
                randomChance < mediumAsteroidSpecification.ChanceToSpawn)
            {
                specification = mediumAsteroidSpecification;
            }

            if (randomChance > mediumAsteroidSpecification.ChanceToSpawn &&
                randomChance < smallAsteroidSpecification.ChanceToSpawn ||
                randomChance > smallAsteroidSpecification.ChanceToSpawn)
            {
                specification = smallAsteroidSpecification;
            }

            return specification;
        }
    }
}