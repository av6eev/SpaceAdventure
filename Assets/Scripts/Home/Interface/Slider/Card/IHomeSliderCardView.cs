using System;

namespace Home.Interface.Slider.Card
{
    public interface IHomeSliderCardView
    {
        event Action OnClick; 
        string GetId();
        string GetNextSceneId();
    }
}