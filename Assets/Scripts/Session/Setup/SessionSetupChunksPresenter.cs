using Awaiter;
using Chunk.Collection;
using Presenter;

namespace Session.Setup
{
    public class SessionSetupChunksPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly PresentersList _presenters;
        private readonly ISessionSceneView _view;

        public readonly CustomAwaiter LoadAwaiter = new();
        
        public SessionSetupChunksPresenter(SessionLocationGameModel gameModel, PresentersList presenters, ISessionSceneView view)
        {
            _gameModel = gameModel;
            _presenters = presenters;
            _view = view;
        }
        
        public async void Init()
        {
            _gameModel.ChunkCollection = new ChunkCollection(_view.ShipView.Position);
            
            var chunksPresenter = new ChunkCollectionPresenter(_gameModel, (ChunkCollection)_gameModel.ChunkCollection, _view.ChunkCollectionView);
            chunksPresenter.Init();
            await chunksPresenter.LoadAwaiter;
            _presenters.Add(chunksPresenter);
            
            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            LoadAwaiter.Dispose();
        }
    }
}