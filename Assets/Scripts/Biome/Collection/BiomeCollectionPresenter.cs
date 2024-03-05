using Presenter;
using Session;

namespace Biome.Collection
{
    public class BiomeCollectionPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly BiomeCollection _model;

        public BiomeCollectionPresenter(SessionLocationGameModel gameModel, BiomeCollection model)
        {
            _gameModel = gameModel;
            _model = model;
        }
        
        public void Init()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}