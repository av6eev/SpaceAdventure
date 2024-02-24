using Chunk.Collection;
using Entities.Asteroids.Asteroid.Damage;
using Entities.Asteroids.Asteroid.Physics;
using Presenter;
using Pulls;
using Session;
using UnityEngine;

namespace Entities.Asteroids.Asteroid
{
    public class AsteroidPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly AsteroidModel _model;
        private readonly IPull<IAsteroidView> _pull;
        private IAsteroidView _view;

        private readonly PresentersList _presenters = new();
        private AsteroidPhysicsUpdater _physicsUpdater;
    
        public AsteroidPresenter(SessionLocationGameModel gameModel, AsteroidModel model, IPull<IAsteroidView> pull)
        {
            _gameModel = gameModel;
            _model = model;
            _pull = pull;
        }

        public void Init()
        {
            _model.IsDisabled = false;
            _view = _pull.Get();
            
            if (_model.Position.Value == Vector3.zero)
            {
                _view.Position = GetRandomPointInChunk();
                _model.Position.Value = _view.Position;
            }
            else
            {
                _view.Position = _model.Position.Value;
            }
            
            _presenters.Add(new AsteroidDamagePresenter(_gameModel, _model, _view));
            _presenters.Init();

            _physicsUpdater = new AsteroidPhysicsUpdater(_model, _view, _gameModel.ShipCameraView);
            _gameModel.FixedUpdatersEngine.Add(_physicsUpdater);
        }

        public void Dispose()
        {
            _model.IsDisabled = true;
            
            _gameModel.FixedUpdatersEngine.Remove(_physicsUpdater);
            _physicsUpdater = null;

            _presenters.Dispose();
            _presenters.Clear();

            _pull.Put(_view);
        }

        private Vector3 GetRandomPointInChunk()
        {
            var chunkPosition = _gameModel.ChunkCollection.Chunks[_model.ChunkId].Position;
            var sizedChunkPosition = chunkPosition + new Vector2(ChunkCollection.ChunkSize.x, ChunkCollection.ChunkSize.z);
            
            var xPoint = Random.Range(chunkPosition.x, sizedChunkPosition.x);
            var yPoint = _gameModel.ShipCameraView.GetRandomPointInCameraHeight();
            var zPoint = Random.Range(chunkPosition.y, sizedChunkPosition.y);
            
            return new Vector3(xPoint, yPoint, zPoint);
        }
    }
}