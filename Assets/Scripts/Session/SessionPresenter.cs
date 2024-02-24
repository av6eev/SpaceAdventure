using AreaBorders;
using Chunk.Collection;
using Chunk.Collection.Generate;
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
            _gameModel.AreaBordersModel = new SessionAreaBordersModel();
            
            var inputPresenter = new SessionSetupInputPresenter(_gameModel, _presenters, _view);
            inputPresenter.Init();
            await inputPresenter.LoadAwaiter;
            inputPresenter.Dispose();
            
            var shipPresenter = new SessionSetupShipPresenter(_gameModel, _presenters, _view);
            shipPresenter.Init();
            await shipPresenter.LoadAwaiter;
            shipPresenter.Dispose();
            
            var utilitiesPresenter = new SessionSetupUtilitiesPresenter(_gameModel, _presenters, _view);
            utilitiesPresenter.Init();
            _presenters.Add(utilitiesPresenter);
            
            var chunksPresenter = new SessionSetupChunksPresenter(_gameModel, _presenters, _view);
            chunksPresenter.Init();
            await chunksPresenter.LoadAwaiter;
            chunksPresenter.Dispose();
            
            var asteroidsPresenter = new SessionSetupAsteroidsPresenter(_gameModel, _presenters, _view);
            asteroidsPresenter.Init();
            await asteroidsPresenter.LoadAwaiter;
            asteroidsPresenter.Dispose();
        }

        public void Dispose()
        {
            _presenters.Dispose();
            _presenters.Clear();
        }
    }
}