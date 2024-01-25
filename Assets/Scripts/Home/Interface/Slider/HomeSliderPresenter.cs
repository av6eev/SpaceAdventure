using Home.Interface.Slider.Card;
using Presenter;

namespace Home.Interface.Slider
{
    public class HomeSliderPresenter : IPresenter
    {
        private readonly HomeLocationGameModel _gameModel;
        private readonly HomeSliderModel _model;
        private readonly IHomeSliderView _view;
        
        private readonly PresentersList _cardPresenters = new();

        public HomeSliderPresenter(HomeLocationGameModel gameModel, HomeSliderModel model, IHomeSliderView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            var index = 0;
            
            foreach (var card in _view.Cards)
            {
                var model = new HomeSliderCardModel(index);
                var view = _view.InstantiateCard(card);
                var presenter = new HomeSliderCardPresenter(_gameModel, model, view);
                
                _cardPresenters.Add(presenter);

                index++;
            }
            
            _cardPresenters.Init();
        }

        public void Dispose()
        {
            _cardPresenters.Dispose();
            _cardPresenters.Clear();
        }
    }
}