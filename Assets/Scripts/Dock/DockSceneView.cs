using Dock.Interface.Header;
using Dock.Interface.Info;
using Dock.Interface.Overview;
using Dock.Interface.Play;
using Dock.Interface.Slider;
using LocationBuilder;

namespace Dock
{
    public class DockSceneView : LocationSceneView
    {
        public DockInfoView InfoView;
        public DockOverviewView OverviewView;
        public DockSliderView SliderView;
        public DockHeaderView HeaderView;
        public DockPlayView PlayView;
    }
}