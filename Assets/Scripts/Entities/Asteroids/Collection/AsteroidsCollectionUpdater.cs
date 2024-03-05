using Specifications.Asteroid;
using Updater;
using Random = UnityEngine.Random;

namespace Entities.Asteroids.Collection
{
    public class AsteroidsCollectionUpdater : IUpdater
    {
        private readonly AsteroidCollection _model;
        
        private int _toCreateCount;
        private float _timeToSpawn;

        public AsteroidsCollectionUpdater(AsteroidCollection model)
        {
            _model = model;
        }
        
        public void Update(float deltaTime)
        {
            CreateMissingAsteroidsUpdate(deltaTime);
        }

        private void CreateMissingAsteroidsUpdate(float deltaTime)
        {
            if (_timeToSpawn >= _model.SpawnRate)
            {
                if (_model.Asteroids.Count >= AsteroidCollection.MaxCount) return;
                
                // var activeChunks = _gameModel.ChunkCollection.GetActiveChunks();
                // model.ChunkId = activeChunks[Random.Range(0, activeChunks.Count)];
                
                _model.CreateAsteroid(GetNewAsteroidSpecification());
            
                _timeToSpawn = 0;
            }
            else
            {
                _timeToSpawn += deltaTime;
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