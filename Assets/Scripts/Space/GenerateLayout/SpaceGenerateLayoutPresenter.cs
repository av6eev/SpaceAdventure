using Awaiter;
using Biome;
using Presenter;
using Session;
using Space.Preview;
using UnityEngine;

namespace Space.GenerateLayout
{
    public class SpaceGenerateLayoutPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly SpaceModel _model;
        private readonly ISpaceView _view;

        public readonly CustomAwaiter LoadAwaiter = new();
        
        public SpaceGenerateLayoutPresenter(SessionLocationGameModel gameModel, SpaceModel model, ISpaceView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            GenerateInitialChunks();

            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            LoadAwaiter.Dispose();
        }

        private void GenerateInitialChunks()
        {
            var mapGraph = SpaceMapLoader.Load();

            foreach (var node in mapGraph.VoidNodes)
            {
                _model.ChunkCollection.Add(node.BiomeId, BiomeType.Void, new Vector2(node.CenterPoint.x, node.CenterPoint.z));
            }

            foreach (var meteorCircleNode in mapGraph.MeteorCircleNodes)
            {
                _model.BiomeCollection.Add(meteorCircleNode.Key, BiomeType.MeteorCircle);
                
                foreach (var node in meteorCircleNode.Value)
                {
                    _model.ChunkCollection.Add(meteorCircleNode.Key, BiomeType.MeteorCircle, new Vector2(node.CenterPoint.x, node.CenterPoint.z));                    
                }
            }
            
            foreach (var innerMeteorCircleNode in mapGraph.InnerMeteorCircleNodes)
            {
                _model.BiomeCollection.Add(innerMeteorCircleNode.Key, BiomeType.InnerMeteorCircle);
                
                foreach (var node in innerMeteorCircleNode.Value)
                {
                    _model.ChunkCollection.Add(innerMeteorCircleNode.Key, BiomeType.InnerMeteorCircle, new Vector2(node.CenterPoint.x, node.CenterPoint.z));                    
                }
            }
        }
    }
}