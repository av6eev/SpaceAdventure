using Presenter;

namespace Dock.Interface.Header
{
    public class DockHeaderPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly IDockHeaderView _view;

        public DockHeaderPresenter(DockLocationGameModel gameModel, IDockHeaderView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        {
            _view.OnBackClick += OnBackButtonClicked;
        }

        public void Dispose()
        {
            _view.OnBackClick -= OnBackButtonClicked;
        }

        private void OnBackButtonClicked()
        {
            _gameModel.SceneManagementModel.SwitchScene(_gameModel.Specifications.SceneSpecifications[_view.GetHomeSceneId()].SceneId);
        }
    }
}