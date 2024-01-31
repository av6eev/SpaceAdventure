using System.Collections.Generic;
using Entities.Asteroids.Asteroid;
using Entities.Asteroids.Pull;
using Presenter;
using Session;

namespace Entities.Asteroids.Collection
{
    public class AsteroidsCollectionPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly AsteroidsCollection _model;
        private readonly AsteroidsPullsCollection _pullsCollection;

        private readonly Dictionary<AsteroidModel, AsteroidPresenter> _asteroidsPresenters = new();
        
        private AsteroidsCollectionUpdater _updater;

        public AsteroidsCollectionPresenter(SessionLocationGameModel gameModel, AsteroidsCollection model, AsteroidsPullsCollection pullsCollection)
        {
            _gameModel = gameModel;
            _model = model;
            _pullsCollection = pullsCollection;
        }

        public void Init()
        {
            _model.OnAsteroidDestroyed += DestroyAsteroid;
            _model.OnAsteroidCreated += CreateAsteroid;

            _updater = new AsteroidsCollectionUpdater(_gameModel, _model);
            
            _gameModel.FixedUpdatersEngine.Add(_updater);
        }

        public void Dispose()
        {
            _model.OnAsteroidDestroyed -= DestroyAsteroid;
            _model.OnAsteroidCreated -= CreateAsteroid;
            
            _gameModel.FixedUpdatersEngine.Remove(_updater);

            foreach (var presenter in _asteroidsPresenters.Values)
            {
                presenter.Dispose();
            }

            _asteroidsPresenters.Clear();
        }

        private void DestroyAsteroid(AsteroidModel model, bool byBorder, bool byShip)
        {
            _asteroidsPresenters[model].Dispose();
        }

        private void CreateAsteroid(AsteroidModel model)
        {
            var presenter = new AsteroidPresenter(_gameModel, model, _pullsCollection.GetById(model.Specification.Id));
            presenter.Init();
            
            _asteroidsPresenters.Add(model, presenter);
        }
    }
}