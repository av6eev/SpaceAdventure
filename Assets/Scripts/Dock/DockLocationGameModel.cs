using Dock.Interface.Slider;

namespace Dock
{
    public class DockLocationGameModel : GameModel
    {
        public IDockSliderModel SliderModel { get; set; }

        public DockLocationGameModel(IGameModel gameModel) : base(gameModel)
        {
            
        }
    }
}