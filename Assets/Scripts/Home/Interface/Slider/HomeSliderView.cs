using System.Collections.Generic;
using Home.Interface.Slider.Card;
using UnityEngine;

namespace Home.Interface.Slider
{
    public class HomeSliderView : MonoBehaviour, IHomeSliderView
    {
        public Transform ContentRoot;
        public List<HomeSliderCardView> CardViews;
        public List<HomeSliderCardView> Cards => CardViews;

        public IHomeSliderCardView InstantiateCard(IHomeSliderCardView card) => Instantiate((HomeSliderCardView)card, ContentRoot);
    }
}