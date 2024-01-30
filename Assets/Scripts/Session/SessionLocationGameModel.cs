using AreaBorders;
using CameraView.Ship;

namespace Session
{
    public class SessionLocationGameModel : GameModel
    {
        public IShipCameraView ShipCameraView { get; set; }
        public IShipCameraModel ShipCameraModel { get; set; }
        public ISessionAreaBordersModel AreaBordersModel { get; set; }
        
        public SessionLocationGameModel(IGameModel gameModel) : base(gameModel)
        {
            
        }
    }
}