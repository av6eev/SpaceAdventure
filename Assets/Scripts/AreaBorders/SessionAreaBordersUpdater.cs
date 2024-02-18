using CameraView.Ship;
using Entities.Ship;
using Updater;

namespace AreaBorders
{
    public class SessionAreaBordersUpdater : IUpdater
    {
        private readonly IShipView _shipView;
        private readonly ISessionAreaBordersModel _areaBordersModel;

        public SessionAreaBordersUpdater(IShipView shipView, ISessionAreaBordersModel areaBordersModel)
        {
            _shipView = shipView;
            _areaBordersModel = areaBordersModel;
        }
        
        public void Update(float deltaTime)
        {
            var shipPositionZ = _shipView.Position.z;

            _areaBordersModel.ForwardBorder.Value = shipPositionZ < 0
                ? shipPositionZ - GameConst.ForwardDrawDistance
                : shipPositionZ + GameConst.ForwardDrawDistance;
        }
    }
}