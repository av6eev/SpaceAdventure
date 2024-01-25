using Home.Interface.Slider;
using Presenter;

namespace Home
{
    public class HomePresenter : IPresenter
    {
        private readonly HomeLocationGameModel _gameModel;
        private readonly HomeSceneView _view;

        private readonly PresentersList _presenters = new();
        
        public HomePresenter(HomeLocationGameModel gameModel, HomeSceneView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        {
            _gameModel.SliderModel = new HomeSliderModel();
            
            _presenters.Add(new HomeSliderPresenter(_gameModel, (HomeSliderModel)_gameModel.SliderModel, _view.SliderView));
            
            _presenters.Init();
        }

        public void Dispose()
        {
            _presenters.Dispose();
            _presenters.Clear();
        }
    }
}