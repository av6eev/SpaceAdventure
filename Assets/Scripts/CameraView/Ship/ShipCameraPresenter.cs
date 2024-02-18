using Presenter;
using Session;

namespace CameraView.Ship
{
    public class ShipCameraPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly ShipCameraModel _model;
        private readonly IShipCameraView _view;

        public ShipCameraPresenter(SessionLocationGameModel gameModel, ShipCameraModel model, IShipCameraView view)
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