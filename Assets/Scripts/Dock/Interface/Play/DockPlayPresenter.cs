using Dock.Interface.Slider.Card;
using Presenter;

namespace Dock.Interface.Play
{
    public class DockPlayPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly IDockPlayView _view;

        public DockPlayPresenter(DockLocationGameModel gameModel, IDockPlayView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        {
            _gameModel.SliderModel.OnSelected += ChangePlayButtonVisibility;
            _view.OnPlayClicked += SwitchScene;
        }

        public void Dispose()
        {
            _gameModel.SliderModel.OnSelected -= ChangePlayButtonVisibility;
            _view.OnPlayClicked -= SwitchScene;
        }

        private void SwitchScene()
        {
            if (_view.GetNextSceneId() == string.Empty) return;

            _gameModel.SceneManagementModel.SwitchScene(_gameModel.Specifications.SceneSpecifications[_view.GetNextSceneId()].SceneId);
        }

        private void ChangePlayButtonVisibility(DockSliderShipCardModel card)
        {
            if (card.State.Value == SliderCardState.Locked || card.PreviewState.Value == SliderCardPreviewState.Previewed)
            {
                _view.HidePlayButton();;
            }
            else
            {
                _view.ShowPlayButton();
            }
        }
    }
}