using System;
using System.Collections.Generic;
using Dock.Interface.Slider.Card;

namespace Dock.Interface.Slider
{
    public class DockSliderModel : IDockSliderModel
    {
        public event Action<DockSliderShipCardModel> OnSelected;

        public readonly List<DockSliderShipCardModel> Cards = new();
        public int CurrentIndex { get; private set; }

        public void Select(int index, bool isPreview = false)
        {
            var previousCard = Cards[CurrentIndex];

            if (previousCard.PreviewState.Value == SliderCardPreviewState.Previewed)
            {
                previousCard.State.Value = SliderCardState.Locked;
                previousCard.PreviewState.Value = SliderCardPreviewState.Unpreviewed;
            }
            else
            {
                previousCard.State.Value = SliderCardState.Unselected;
            }

            CurrentIndex = index;
            
            if (isPreview)
            {
                Cards[CurrentIndex].PreviewState.Value = SliderCardPreviewState.Previewed;
            }

            Cards[CurrentIndex].State.Value = SliderCardState.Selected;
           
            OnSelected?.Invoke(Cards[index]);
        }

        public void Add(DockSliderShipCardModel model)
        {
            Cards.Add(model);
        }
    }
}