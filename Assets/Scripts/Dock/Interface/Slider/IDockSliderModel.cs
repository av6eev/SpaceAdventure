using System;
using Dock.Interface.Slider.Card;

namespace Dock.Interface.Slider
{
    public interface IDockSliderModel
    {
        event Action<DockSliderShipCardModel> OnSelected;
        int CurrentIndex { get; }
        void Select(int index, bool isPreview = false);
        void Add(DockSliderShipCardModel model);
    }
}