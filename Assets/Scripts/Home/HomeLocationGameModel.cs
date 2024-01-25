using Home.Interface.Slider;

namespace Home
{
    public class HomeLocationGameModel : GameModel
    {
        public IHomeSliderModel SliderModel { get; set; }

        public HomeLocationGameModel(IGameModel gameModel) : base(gameModel)
        {
            
        }
    }
}