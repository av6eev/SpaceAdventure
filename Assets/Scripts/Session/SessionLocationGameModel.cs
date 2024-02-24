using AreaBorders;
using CameraView.Ship;
using Chunk.Collection;
using Entities.Asteroids.Collection;

namespace Session
{
    public class SessionLocationGameModel : GameModel
    {
        public IShipCameraView ShipCameraView { get; set; }
        public IShipCameraModel ShipCameraModel { get; set; }
        public ISessionAreaBordersModel AreaBordersModel { get; set; }
        public IChunkCollection ChunkCollection { get; set; }
        public IAsteroidsCollection AsteroidsCollection { get; set; }
        
        public SessionLocationGameModel(IGameModel gameModel) : base(gameModel)
        {
            
        }
    }
}