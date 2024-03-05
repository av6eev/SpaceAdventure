using Presenter;
using Session;

namespace Chunk.Collection.Generate
{
    public class ChunkCollectionGeneratePresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly ChunkCollection _model;
        private readonly IChunkCollectionView _view;

        private readonly PresentersDictionary<string> _chunkPresenters = new();

        public ChunkCollectionGeneratePresenter(SessionLocationGameModel gameModel, ChunkCollection model, IChunkCollectionView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            _model.OnChunkAdded += CreateChunk;
            _model.OnChunkRemoved += RemoveChunk;
        }

        public void Dispose()
        {
            _model.OnChunkAdded -= CreateChunk;
            _model.OnChunkRemoved -= RemoveChunk;
        }

        private void CreateChunk(ChunkModel chunkModel)
        {
            var chunkPresenter = new ChunkPresenter(_gameModel, chunkModel, _view.InstantiateChunkView(chunkModel.Position, $"{chunkModel.Id}-{chunkModel.BiomeType}"));                    
            chunkPresenter.Init();
            _chunkPresenters.Add(chunkModel.Id, chunkPresenter);
        }

        private void RemoveChunk(string id)
        {
            _chunkPresenters.Remove(id);
        }
    }
}