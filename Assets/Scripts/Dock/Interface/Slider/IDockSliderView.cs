using System;
using Dock.Interface.Slider.Card;
using UnityEngine;

namespace Dock.Interface.Slider
{
    public interface IDockSliderView
    {
        event Action OnNextClick;
        event Action OnPreviousClick;
        IDockSliderShipCardView InstantiateShipCard(GameObject cardGo);
        void ScrollNext();
        void ScrollPrevious();
    }
}