using Presenter;

namespace Dock.Interface.Slider.Card
{
    public class DockSliderShipCardPresenter : IPresenter
    {
        private readonly DockLocationGameModel _gameModel;
        private readonly DockSliderShipCardModel _model;
        private readonly IDockSliderShipCardView _view;

        public DockSliderShipCardPresenter(DockLocationGameModel gameModel, DockSliderShipCardModel model, IDockSliderShipCardView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            _model.State.OnChanged += ChangeSelectedHighlight;
            _model.PreviewState.OnChanged += ChangePreviewedHighlight;
            
            if (_model.Specification.Id == "agasiz_ship")
            {
                _model.State.Value = SliderCardState.Selected;
                _gameModel.SliderModel.Select(_model.Index);
            }
            // TODO: check if player has purchased a ship, then set Unselected state
            else
            {
                _model.State.Value = SliderCardState.Locked;
            }
            
            _view.SetTitle(_model.Specification.Name);
            _view.OnClick += OnShipSelect;
        }

        public void Dispose()
        {
            _model.State.OnChanged -= ChangeSelectedHighlight;
            _model.PreviewState.OnChanged -= ChangePreviewedHighlight;

            _view.OnClick -= OnShipSelect;
        }

        private void ChangePreviewedHighlight(SliderCardPreviewState state)
        {
            switch (state)
            {
                case SliderCardPreviewState.Previewed:
                    _view.EnableSelectedHighlight();
                    break;
                case SliderCardPreviewState.Unpreviewed:
                    _view.DisableSelectedHighlight();
                    break;
            }
        }

        private void ChangeSelectedHighlight(SliderCardState state)
        {
            switch (state)
            {
                case SliderCardState.Selected:
                    _view.EnableSelectedHighlight();
                    break;
                case SliderCardState.Unselected:
                    _view.DisableSelectedHighlight();
                    break;
                case SliderCardState.Locked:
                    _view.DisableSelectedHighlight();
                    _view.EnableLockIcon();
                    break;
            }
        }

        private void OnShipSelect()
        {
            if (_gameModel.SliderModel.CurrentIndex != _model.Index)
            {
                _gameModel.SliderModel.Select(_model.Index, _model.State.Value == SliderCardState.Locked);
            }
        }
    }
}