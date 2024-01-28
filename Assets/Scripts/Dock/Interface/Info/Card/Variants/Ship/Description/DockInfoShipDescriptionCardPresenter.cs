using Dock.Interface.Slider.Card;
using Presenter;

namespace Dock.Interface.Info.Card.Variants.Ship.Description
{
    public class DockInfoShipDescriptionCardPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly IDockInfoShipDescriptionCardView _view;

        public DockInfoShipDescriptionCardPresenter(DockLocationGameModel gameModel, IDockInfoShipDescriptionCardView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        {
            _gameModel.SliderModel.OnSelected += FillView;   
        }

        public void Dispose()
        {
            _gameModel.SliderModel.OnSelected -= FillView;   
        }

        private void FillView(DockSliderShipCardModel card)
        {
            _view.FillData(card.Specification);
        }
    }
}