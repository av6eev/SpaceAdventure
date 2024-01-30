using Presenter;

namespace Entities.Ship.Damage
{
    public class ShipDamagePresenter : IPresenter
    {
        private readonly IGameModel _gameModel;
        private readonly ShipModel _model;
        private readonly IShipView _view;
    
        public ShipDamagePresenter(IGameModel gameModel, ShipModel model, IShipView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
    
        public void Init()
        {
        }

        public void Dispose()
        {
        }
    }
}