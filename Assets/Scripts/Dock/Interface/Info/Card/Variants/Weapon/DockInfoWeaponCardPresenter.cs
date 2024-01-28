using Presenter;

namespace Dock.Interface.Info.Card.Variants.Weapon
{
    public class DockInfoWeaponCardPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly IDockInfoWeaponCardView _view;

        public DockInfoWeaponCardPresenter(DockLocationGameModel gameModel, IDockInfoWeaponCardView view)
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