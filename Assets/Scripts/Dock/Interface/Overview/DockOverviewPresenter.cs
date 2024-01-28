using Dock.Interface.Slider.Card;
using Loader.Object;
using Presenter;
using UnityEngine;

namespace Dock.Interface.Overview
{
    public class DockOverviewPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly IDockOverviewView _view;
        
        private ILoadObjectModel<GameObject> _shipView;
        private GameObject _shipViewGo;

        public DockOverviewPresenter(DockLocationGameModel gameModel, IDockOverviewView view)
        {
            _gameModel = gameModel;
            _view = view;
        }
        
        public void Init()
        {
            _gameModel.SliderModel.OnSelected += OnSelected;
        }

        public void Dispose()
        {
            _gameModel.SliderModel.OnSelected -= OnSelected;
        }

        private async void OnSelected(DockSliderShipCardModel cardModel)
        {
            var animationController = _view.AnimationController;
            
            if (_shipView != null)
            {
                animationController.PlayDownAnimation();
                await animationController.DownAnimationAwaiter;

                if (_shipViewGo != null)
                {
                    _view.DestroyShip(_shipViewGo);
                    _shipViewGo = null;
                }
                
                _shipView.LoadAwaiter.Dispose();
                _gameModel.LoadObjectsModel.Unload(_shipView);
            }
            
            _shipView = _gameModel.LoadObjectsModel.Load<GameObject>(cardModel.Specification.PrefabKey3D);
            await _shipView.LoadAwaiter;

            _shipViewGo = _view.InstantiateShip(_shipView.Result);
            
            animationController.PlayUpAnimation();
            await animationController.UpAnimationAwaiter;
        }
    }
}