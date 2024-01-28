using Presenter;

namespace Dock.Interface.Info.Card.Variants.Ship.Requirement
{
    public class DockInfoShipRequirementCardPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly IDockInfoShipRequirementCardView _view;

        public DockInfoShipRequirementCardPresenter(DockLocationGameModel gameModel, IDockInfoShipRequirementCardView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        { 
            
        }

        public void Dispose()
        {
        }
    }
}