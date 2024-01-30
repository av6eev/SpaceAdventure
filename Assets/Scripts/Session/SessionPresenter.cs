using Presenter;
using Session.Setup;

namespace Session
{
    public class SessionPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly SessionModel _model;
        private readonly ISessionSceneView _view;

        private readonly PresentersList _presenters = new();
        
        public SessionPresenter(SessionLocationGameModel gameModel, SessionModel model, ISessionSceneView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }

        public async void Init()
        {
            var inputPresenter = new SessionSetupInputPresenter(_gameModel, _presenters, _view);
            inputPresenter.Init();
            await inputPresenter.LoadAwaiter;
            inputPresenter.Dispose();
        }

        public void Dispose()
        {
            _presenters.Dispose();
            _presenters.Clear();
        }
    }
}