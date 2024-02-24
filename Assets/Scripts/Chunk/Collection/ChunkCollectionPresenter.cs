using Awaiter;
using Chunk.Collection.Generate;
using Presenter;
using Session;

namespace Chunk.Collection
{
    public class ChunkCollectionPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly ChunkCollection _model;
        private readonly IChunkCollectionView _view;

        private readonly PresentersList _presenters = new();
        private ChunkCollectionUpdater _collectionUpdater;
        public readonly CustomAwaiter LoadAwaiter = new();
        
        public ChunkCollectionPresenter(SessionLocationGameModel gameModel, ChunkCollection model, IChunkCollectionView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public async void Init()
        {
            _collectionUpdater = new ChunkCollectionUpdater(_model, _gameModel.ShipCameraView);
            _gameModel.UpdatersEngine.Add(_collectionUpdater);

            var generatePresenter = new ChunkCollectionGeneratePresenter(_gameModel, _model, _view);
            generatePresenter.Init();
            await generatePresenter.LoadAwaiter;
            _presenters.Add(generatePresenter);

            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            _gameModel.UpdatersEngine.Remove(_collectionUpdater);
            
            _presenters.Dispose();
            _presenters.Clear();
            
            LoadAwaiter.Dispose();
        }
    }
}