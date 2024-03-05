using Dock.Interface.Slider.Card;
using Loader.Object;
using Presenter;
using UnityEngine;
using Utilities;

namespace Dock.Interface.Slider
{
    public class DockSliderPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly DockSliderModel _model;
        private readonly IDockSliderView _view;
        
        private ILoadObjectModel<GameObject> _shipCardObject;
        private readonly PresentersList _cardPresenters = new();

        public DockSliderPresenter(DockLocationGameModel gameModel, DockSliderModel model, IDockSliderView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public async void Init()
        {
            _shipCardObject = _gameModel.LoadObjectsModel.Load<GameObject>("ship_card");
            await _shipCardObject.LoadAwaiter;

            var index = 0;
            
            foreach (var specification in _gameModel.Specifications.ShipSpecifications.GetSpecifications().Values)
            {
                var cardModel = new DockSliderShipCardModel(specification, index);
                _model.Add(cardModel);
                var view = _view.InstantiateShipCard(_shipCardObject.Result);
                var presenter = new DockSliderShipCardPresenter(_gameModel, cardModel, view);
                
                _cardPresenters.Add(presenter);

                index++;
            }
            
            _cardPresenters.Init();
            
            _view.OnNextClick += SelectNextCard;
            _view.OnPreviousClick += SelectPreviousCard;

            _model.OnSelected += SaveCurrentShipId;
        }

        public void Dispose()
        {
            _shipCardObject.Dispose();
            
            _cardPresenters.Dispose();
            _cardPresenters.Clear();
            
            _view.OnNextClick -= SelectNextCard;
            _view.OnPreviousClick -= SelectPreviousCard;
            
            _gameModel.LoadObjectsModel.Unload(_shipCardObject);
            
            _model.OnSelected -= SaveCurrentShipId;
        }

        private void SaveCurrentShipId(DockSliderShipCardModel card)
        {
            if (card.PreviewState.Value == SliderCardPreviewState.Previewed)
            {
                Debug.Log(card.Specification.Id + $" is in preview state: {card.PreviewState.Value} and cannot be saved!");
                return;
            }

            if (card.Specification.Id != PlayerPrefs.GetString(SavingElementsKeys.CurrentShipIdKey))
            {
                _gameModel.SaveModel.SaveCurrentShipId(card.Specification.Id);
            }
            else
            {
                Debug.Log(card.Specification.Id + " is already saved and current");
            }
        }

        private void SelectNextCard()
        {
            var nextIndex = _model.CurrentIndex + 1;
            
            if (nextIndex >= _model.Cards.Count) return;

            if (nextIndex % 4 == 0)
            {
                _view.ScrollNext();
            }

            _model.Select(nextIndex, _model.Cards[nextIndex].State.Value == SliderCardState.Locked);
        }

        private void SelectPreviousCard()
        {
            var previousIndex = _model.CurrentIndex - 1;
            
            if (previousIndex < 0) return;
            
            if (previousIndex % 4 == 0)
            {
                _view.ScrollPrevious();
            }
            
            _model.Select(previousIndex, _model.Cards[previousIndex].State.Value == SliderCardState.Locked);
        }
    }
}