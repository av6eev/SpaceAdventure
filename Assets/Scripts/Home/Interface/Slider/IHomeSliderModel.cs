namespace Home.Interface.Slider
{
    public interface IHomeSliderModel
    {
        int Current { get; }
        void Select(int index);
    }
}