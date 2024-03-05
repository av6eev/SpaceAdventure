using AreaBorders;
using CameraView.Ship;
using Space;

namespace Session
{
    public class SessionLocationGameModel : GameModel
    {
        public IShipCameraView ShipCameraView { get; set; }
        public IShipCameraModel ShipCameraModel { get; set; }
        public ISessionAreaBordersModel AreaBordersModel { get; set; }
        public ISpaceModel SpaceModel { get; set; }
        
        public SessionLocationGameModel(IGameModel gameModel) : base(gameModel)
        {
            
        }
    }
}