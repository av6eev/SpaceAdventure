using System.Collections.Generic;
using Home.Interface.Slider.Card;

namespace Home.Interface.Slider
{
    public interface IHomeSliderView
    {
        List<HomeSliderCardView> Cards { get; }
        IHomeSliderCardView InstantiateCard(IHomeSliderCardView card);
    }
}