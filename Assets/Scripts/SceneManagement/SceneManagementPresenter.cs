using Loader.Scene;
using Presenter;

namespace SceneManagement
{
    public class SceneManagementPresenter : IPresenter
    {
        private readonly IGameModel _gameModel;
        private readonly SceneManagementModel _model;

        private ILoadSceneModel _currentScene;
        
        public SceneManagementPresenter(IGameModel gameModel, SceneManagementModel model)
        {
            _gameModel = gameModel;
            _model = model;
        }
        
        public void Init()
        {
            _model.OnSceneSwitched += SwitchScene;

            if (!string.IsNullOrEmpty(_model.CurrentSceneId))
            {
                SwitchScene(_model.CurrentSceneId);
            }
        }

        public void Dispose()
        {
            _model.OnSceneSwitched -= SwitchScene;
        }

        private async void SwitchScene(string id)
        {
            if (_currentScene != null)
            {
                if (_model.CurrentSceneId == id) return;

                if (!_currentScene.LoadAwaiter.IsCompleted)
                {
                    _currentScene.LoadAwaiter.Dispose();
                }
            
                _gameModel.LoadScenesModel.Unload(_currentScene);
                await _currentScene.UnloadAwaiter;
            }

            _currentScene = _gameModel.LoadScenesModel.Load(_gameModel.Specifications.SceneSpecifications[id]);
            await _currentScene.LoadAwaiter;
            
            _model.CurrentSceneId = id;
        }
    }
}