using System;
using UnityEngine;
using UnityEngine.UI;

namespace Home.Interface.Slider.Card
{
    public class HomeSliderCardView : MonoBehaviour, IHomeSliderCardView
    {
        public event Action OnClick;
        
        public string Id;
        public string SwitchToSceneId;
        public Button Button;

        private void Start() => Button.onClick.AddListener(HandleClick);
        private void OnDestroy() => Button.onClick.RemoveListener(HandleClick);
        private void HandleClick() => OnClick?.Invoke();
        public string GetId() => Id;
        public string GetNextSceneId() => SwitchToSceneId;
    }
}