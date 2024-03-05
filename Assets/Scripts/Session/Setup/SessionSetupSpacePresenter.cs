using Awaiter;
using Presenter;
using Space;
using UnityEngine;

namespace Session.Setup
{
    public class SessionSetupSpacePresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly PresentersList _presenters;
        private readonly ISessionSceneView _view;

        public readonly CustomAwaiter LoadAwaiter = new();
        
        public SessionSetupSpacePresenter(SessionLocationGameModel gameModel, PresentersList presenters, ISessionSceneView view)
        {
            _gameModel = gameModel;
            _presenters = presenters;
            _view = view;
        }
        
        public async void Init()
        {
            _gameModel.SpaceModel = new SpaceModel();
            
            var spacePresenter = new SpacePresenter(_gameModel, (SpaceModel)_gameModel.SpaceModel, _view.SpaceView);
            spacePresenter.Init();
            await spacePresenter.LoadAwaiter;
            _presenters.Add(spacePresenter);
            
            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            LoadAwaiter.Dispose();
        }
    }
}