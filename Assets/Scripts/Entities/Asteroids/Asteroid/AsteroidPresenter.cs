using Entities.Asteroids.Asteroid.Damage;
using Entities.Asteroids.Asteroid.Physics;
using Presenter;
using Pulls;
using Session;

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
            _view = _pull.Get();
            _view.Position = _gameModel.ShipCameraView.GetRandomPointInCameraView(_gameModel.AreaBordersModel.ForwardBorder.Value, 20f, 20f, 450f);
            _model.Position.Value = _view.Position;
            
            _presenters.Add(new AsteroidDamagePresenter(_gameModel, _model, _view));
            _presenters.Init();

            _physicsUpdater = new AsteroidPhysicsUpdater(_model, _view, _gameModel.ShipCameraView);
            _gameModel.UpdatersEngine.Add(_physicsUpdater);
        }

        public void Dispose()
        {
            _gameModel.UpdatersEngine.Remove(_physicsUpdater);
        
            _presenters.Dispose();
            _presenters.Clear();

            _pull.Put(_view);
        }
    }
}