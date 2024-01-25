using Home.Interface.Slider;
using LocationBuilder;
using UnityEngine;

namespace Home
{
    public class HomeSceneView : LocationSceneView
    {
        public HomeSliderView SliderView;
        public GameObject ContentRoot;
        
        public void Show()
        {
            ContentRoot.SetActive(false);
        }
        
        public void Hide()
        {
            ContentRoot.SetActive(true);
        }
    }
}