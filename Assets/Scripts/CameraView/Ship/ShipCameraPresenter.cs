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
            _model.CurrentPositionFollowStrength.OnChanged += ChangePositionFollowStrength;
            _model.CurrentRotationFollowStrength.OnChanged += ChangeRotationFollowStrength;
        }

        public void Dispose()
        {
            _model.CurrentPositionFollowStrength.OnChanged -= ChangePositionFollowStrength;
            _model.CurrentRotationFollowStrength.OnChanged -= ChangeRotationFollowStrength;
        }

        private void ChangeRotationFollowStrength(float value)
        {
            _view.RotationFollowStrength = value;
        }

        private void ChangePositionFollowStrength(float value)
        {
            _view.PositionFollowStrength = value;
        }
    }
}