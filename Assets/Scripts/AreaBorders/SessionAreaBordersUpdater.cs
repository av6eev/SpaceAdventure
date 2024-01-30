using Entities.Ship;
using Updater;

namespace AreaBorders
{
    public class SessionAreaBordersUpdater : IUpdater
    {
        private readonly IGameModel _gameModel;
        private readonly IShipView _shipView;
        private readonly ISessionAreaBordersModel _areaBordersModel;

        public SessionAreaBordersUpdater(IGameModel gameModel, IShipView shipView, ISessionAreaBordersModel areaBordersModel)
        {
            _gameModel = gameModel;
            _shipView = shipView;
            _areaBordersModel = areaBordersModel;
        }
        
        public void Update(float deltaTime)
        {
            var shipPositionZ = _shipView.Position.z;

            _areaBordersModel.ForwardBorder.Value = shipPositionZ < 0
                ? shipPositionZ - SessionAreaBordersModel.ForwardAdditionalNumber
                : shipPositionZ + SessionAreaBordersModel.ForwardAdditionalNumber;
        }
    }
}