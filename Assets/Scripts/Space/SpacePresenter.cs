using Awaiter;
using Biome.Collection;
using Chunk.Collection;
using Presenter;
using Session;
using Space.GenerateLayout;

namespace Space
{
    public class SpacePresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly SpaceModel _model;
        private readonly ISpaceView _view;
        
        private readonly PresentersList _presenters = new();
        public readonly CustomAwaiter LoadAwaiter = new();

        public SpacePresenter(SessionLocationGameModel gameModel, SpaceModel model, ISpaceView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public async void Init()
        {
            _model.ChunkCollection = new ChunkCollection();
            _model.BiomeCollection = new BiomeCollection();
            
            var chunksPresenter = new ChunkCollectionPresenter(_gameModel, (ChunkCollection)_model.ChunkCollection, _view.ChunkCollectionView);
            chunksPresenter.Init();
            _presenters.Add(chunksPresenter);
            
            var biomePresenter = new BiomeCollectionPresenter(_gameModel, (BiomeCollection)_model.BiomeCollection);
            biomePresenter.Init();
            _presenters.Add(biomePresenter);
            
            var generateLayoutPresenter = new SpaceGenerateLayoutPresenter(_gameModel, _model, _view);
            generateLayoutPresenter.Init();
            await generateLayoutPresenter.LoadAwaiter;
            _presenters.Add(generateLayoutPresenter);
            
            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            LoadAwaiter.Dispose();
        }
    }
}