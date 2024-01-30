using Entities.Ship.BoostEffect;
using Entities.Ship.Damage;
using Entities.Ship.Physics;
using Presenter;
using Session;

namespace Entities.Ship
{
    public class ShipPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly ShipModel _model;
        private readonly IShipView _view;

        private ShipPhysicsUpdater _physicsUpdater;
        private readonly PresentersList _presenters = new();

        public ShipPresenter(SessionLocationGameModel gameModel, ShipModel model, IShipView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }

        public void Init()
        {
            _presenters.Add(new ShipDamagePresenter(_gameModel, _model, _view));
            _presenters.Add(new ShipBoostEffectPresenter(_gameModel, _model, _view));
            _presenters.Init();
            
            _physicsUpdater = new ShipPhysicsUpdater(_gameModel.InputModel, _view, _model);
            _gameModel.UpdatersEngine.Add(_physicsUpdater);
        }

        public void Dispose()
        {
            _presenters.Dispose();
            _presenters.Clear();
            
            _gameModel.UpdatersEngine.Remove(_physicsUpdater);
        }
    }
}