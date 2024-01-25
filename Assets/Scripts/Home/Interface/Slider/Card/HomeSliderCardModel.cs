namespace Home.Interface.Slider.Card
{
    public class HomeSliderCardModel
    {
        public int Index { get; }
        public bool IsActive;

        public HomeSliderCardModel(int index)
        {
            Index = index;
        }
    }
}