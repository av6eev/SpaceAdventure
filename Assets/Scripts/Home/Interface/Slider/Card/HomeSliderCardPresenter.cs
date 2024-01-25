using Loader.Scene;
using Presenter;
using UnityEngine;

namespace Home.Interface.Slider.Card
{
    public class HomeSliderCardPresenter : IPresenter
    {
        private readonly HomeLocationGameModel _gameModel;
        private readonly HomeSliderCardModel _model;
        private readonly IHomeSliderCardView _view;
        
        private ILoadSceneModel _newScene;

        public HomeSliderCardPresenter(HomeLocationGameModel gameModel, HomeSliderCardModel model, IHomeSliderCardView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            _view.OnClick += SwitchScene;
        }

        public void Dispose()
        {
            _view.OnClick -= SwitchScene;
        }

        private void SwitchScene()
        {
            if (_gameModel.SliderModel.Current == _model.Index || _model.IsActive) return;
            if (_view.GetNextSceneId() == string.Empty) return;

            if (_view.GetId() == "play_card")
            {
                if (PlayerPrefs.GetString(SavingElementsKeys.CurrentShipId) == string.Empty)
                {
                    Debug.LogError("No ships have been selected yet!");
                    return;
                }
            }

            _gameModel.SliderModel.Select(_model.Index);
            _gameModel.SceneManagementModel.SwitchScene(_gameModel.Specifications.SceneSpecifications[_view.GetNextSceneId()].SceneId);
        }
    }
}