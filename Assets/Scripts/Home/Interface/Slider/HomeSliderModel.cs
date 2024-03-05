using System.Collections.Generic;
using Home.Interface.Slider.Card;

namespace Home.Interface.Slider
{
    public class HomeSliderModel : IHomeSliderModel
    {
        public int Current { get; private set; } = -1;

        public readonly List<HomeSliderCardModel> CardModels = new();

        public void Select(int index)
        {
            if (Current.Equals(-1)) return;
            
            CardModels[Current].IsActive = false;
            
            Current = index;
            
            CardModels[index].IsActive = true;
        }
    }
}