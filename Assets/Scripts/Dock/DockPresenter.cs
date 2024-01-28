using Dock.Interface.Header;
using Dock.Interface.Info;
using Dock.Interface.Overview;
using Dock.Interface.Play;
using Dock.Interface.Slider;
using Presenter;

namespace Dock
{
    public class DockPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly DockSceneView _view;
        
        private readonly PresentersList _presenters = new();
        
        public DockPresenter(DockLocationGameModel gameModel, DockSceneView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        {
            _gameModel.SliderModel = new DockSliderModel();
            
            _presenters.Add(new DockHeaderPresenter(_gameModel, _view.HeaderView));
            _presenters.Add(new DockSliderPresenter(_gameModel, (DockSliderModel)_gameModel.SliderModel, _view.SliderView));
            _presenters.Add(new DockInfoPresenter(_gameModel, _view.InfoView));
            _presenters.Add(new DockOverviewPresenter(_gameModel, _view.OverviewView));
            _presenters.Add(new DockPlayPresenter(_gameModel, _view.PlayView));

            _presenters.Init();
        }

        public void Dispose()
        {
            _presenters.Dispose();
            _presenters.Clear();
        }
    }
}