using Entities.Asteroids.Asteroid;
using Entities.Asteroids.Pull;
using Presenter;
using Session;

namespace Entities.Asteroids.Collection
{
    public class AsteroidsCollectionPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly AsteroidCollection _model;
        private readonly AsteroidsPullsCollection _pullsCollection;

        private readonly PresentersDictionary<AsteroidModel> _asteroidsPresenters = new();
        private AsteroidsCollectionUpdater _updater;

        public AsteroidsCollectionPresenter(SessionLocationGameModel gameModel, AsteroidCollection model, AsteroidsPullsCollection pullsCollection)
        {
            _gameModel = gameModel;
            _model = model;
            _pullsCollection = pullsCollection;
        }

        public void Init()
        {
            _model.OnAsteroidDestroyed += DestroyAsteroid;
            _model.OnAsteroidCreated += CreateAsteroid;
            
            // _updater = new AsteroidsCollectionUpdater(_model);
            
            // _gameModel.UpdatersEngine.Add(_updater);
        }

        public void Dispose()
        {
            _model.OnAsteroidDestroyed -= DestroyAsteroid;
            _model.OnAsteroidCreated -= CreateAsteroid;
            
            // _gameModel.UpdatersEngine.Remove(_updater);

            _asteroidsPresenters.Dispose();
            _asteroidsPresenters.Clear();
        }

        private void CreateAsteroid(AsteroidModel model)
        {
            _gameModel.SpaceModel.ChunkCollection[model.ChunkId].AddElement(model);

            var presenter = new AsteroidPresenter(_gameModel, model, _pullsCollection[model.Specification.Id]);
            presenter.Init();
            _asteroidsPresenters.Add(model, presenter);
        }

        private void DestroyAsteroid(AsteroidModel model)
        {
            _asteroidsPresenters.Remove(model);
        }
    }
}