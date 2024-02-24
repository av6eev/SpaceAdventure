using Awaiter;
using Entities.Asteroids.Collection;
using Entities.Asteroids.Pull;
using Presenter;
using UnityEngine;

namespace Session.Setup
{
    public class SessionSetupAsteroidsPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly PresentersList _presenters;
        private readonly ISessionSceneView _view;

        public readonly CustomAwaiter LoadAwaiter = new();
        
        public SessionSetupAsteroidsPresenter(SessionLocationGameModel gameModel, PresentersList presenters, ISessionSceneView view)
        {
            _gameModel = gameModel;
            _presenters = presenters;
            _view = view;
        }
        
        public async void Init()
        {
            var pullCollection = new AsteroidsPullsCollection();

            foreach (var specification in _gameModel.Specifications.AsteroidSpecifications.GetSpecifications())
            {
                var asteroidGo = _gameModel.LoadObjectsModel.Load<GameObject>(specification.Value.PrefabKey3D);
                await asteroidGo.LoadAwaiter;
                
                switch (specification.Key)
                {
                    case "small_asteroid":
                        pullCollection.SmallAsteroidPull = new AsteroidsPull(asteroidGo.Result);
                        pullCollection.SmallAsteroidPull.Init(30);
                        break;
                    case "medium_asteroid":
                        pullCollection.MediumAsteroidPull = new AsteroidsPull(asteroidGo.Result);
                        pullCollection.MediumAsteroidPull.Init(15);
                        break;
                    case "big_asteroid":
                        pullCollection.BigAsteroidPull = new AsteroidsPull(asteroidGo.Result);
                        pullCollection.BigAsteroidPull.Init(15);
                        break;
                    case "fire_asteroid":
                        pullCollection.FireAsteroidPull = new AsteroidsPull(asteroidGo.Result);
                        pullCollection.FireAsteroidPull.Init(10);
                        break;
                }
            }

            _gameModel.AsteroidsCollection = new AsteroidsCollection(_gameModel.Specifications.AsteroidSpecifications.GetSpecifications());
            var asteroidsCollectionPresenter = new AsteroidsCollectionPresenter(_gameModel, (AsteroidsCollection)_gameModel.AsteroidsCollection, pullCollection);
            asteroidsCollectionPresenter.Init();
            _presenters.Add(asteroidsCollectionPresenter);
            
            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            LoadAwaiter.Dispose();
        }
    }
}