using Dock.Interface.Info.Card.Variants.Ship.Characteristics;
using Dock.Interface.Info.Card.Variants.Ship.Description;
using Dock.Interface.Info.Card.Variants.Ship.Requirement;
using Dock.Interface.Info.Card.Variants.Weapon;
using Presenter;

namespace Dock.Interface.Info
{
    public class DockInfoPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly IDockInfoView _view;

        private readonly PresentersList _presenters = new();
        
        public DockInfoPresenter(DockLocationGameModel gameModel, IDockInfoView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        {
            _presenters.Add(new DockInfoWeaponCardPresenter(_gameModel, _view.WeaponCardView));
            _presenters.Add(new DockInfoShipCharacteristicsCardPresenter(_gameModel, _view.ShipCharacteristicsCardView));
            _presenters.Add(new DockInfoShipDescriptionCardPresenter(_gameModel, _view.ShipDescriptionCardView));
            _presenters.Add(new DockInfoShipRequirementCardPresenter(_gameModel, _view.ShipRequirementCardView));
            
            _presenters.Init();
        }

        public void Dispose()
        {
            _presenters.Dispose();
            _presenters.Clear();
        }
    }
}