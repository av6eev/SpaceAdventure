using Awaiter;
using Presenter;
using Session;
using UnityEngine;

namespace Chunk.Collection.Generate
{
    public class ChunkCollectionGeneratePresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly ChunkCollection _model;
        private readonly IChunkCollectionView _view;

        private readonly PresentersDictionary<string> _chunkPresenters = new();

        public readonly CustomAwaiter LoadAwaiter = new();
        
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
            
            GenerateInitialChunks();

            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            _model.OnChunkAdded -= CreateChunk;
            _model.OnChunkRemoved -= RemoveChunk;
            
            LoadAwaiter.Dispose();
        }

        private void GenerateInitialChunks()
        {
            var renderDistance = GameConst.ForwardDrawDistance;
            
            for (var x = _model.StartPoint.x - renderDistance; x <= _model.StartPoint.x + renderDistance; x++)
            {
                for (var z = _model.StartPoint.z - renderDistance; z <= _model.StartPoint.z + renderDistance; z++)
                {
                    if ((int)(x % ChunkCollection.ChunkSize.x) == 0 && (int)(z % ChunkCollection.ChunkSize.y) == 0)
                    {
                        _model.AddChunk(new Vector2(x, z));
                    }
                }   
            }
        }

        private void CreateChunk(ChunkModel chunkModel)
        {
            var chunkPresenter = new ChunkPresenter(_gameModel, chunkModel, _view.InstantiateChunkView(chunkModel.Position, $"{chunkModel.Id}"));                    
            chunkPresenter.Init();
            _chunkPresenters.Add(chunkModel.Id, chunkPresenter);
        }

        private void RemoveChunk(string id)
        {
            _chunkPresenters.Remove(id);
        }
    }
}