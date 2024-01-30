using AreaBorders;
using CameraView.Ship;
using Presenter;

namespace Session.Setup
{
    public class SessionSetupUtilitiesPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly PresentersList _presentersList;
        private readonly ISessionSceneView _view;
        
        private SessionAreaBordersUpdater _bordersUpdater;
        private ShipCameraFollowUpdater _shipCameraUpdater;

        public SessionSetupUtilitiesPresenter(SessionLocationGameModel gameModel, PresentersList presentersList, ISessionSceneView view)
        {
            _gameModel = gameModel;
            _presentersList = presentersList;
            _view = view;
        }
        
        public void Init()
        {
            _gameModel.ShipCameraView = _view.ShipCameraView;
            _gameModel.ShipCameraModel = new ShipCameraModel(_view.ShipCameraView.PositionFollowStrength, _view.ShipCameraView.RotationFollowStrength);
            
            var shipCameraPresenter = new ShipCameraPresenter(_gameModel, (ShipCameraModel)_gameModel.ShipCameraModel, _view.ShipCameraView);
            shipCameraPresenter.Init();
            _presentersList.Add(shipCameraPresenter);
            
            _shipCameraUpdater = new ShipCameraFollowUpdater(_gameModel.ShipCameraModel, _view.ShipCameraView, _view.ShipView);
            _bordersUpdater = new SessionAreaBordersUpdater(_gameModel, _view.ShipView, (SessionAreaBordersModel)_gameModel.AreaBordersModel);
            
            _gameModel.LateUpdatersEngine.Add(_shipCameraUpdater);
            _gameModel.UpdatersEngine.Add(_bordersUpdater);
            
            _view.SetupCamera();
        }

        public void Dispose()
        {
            _gameModel.LateUpdatersEngine.Remove(_shipCameraUpdater);
            _gameModel.UpdatersEngine.Remove(_bordersUpdater);
        }
    }
}