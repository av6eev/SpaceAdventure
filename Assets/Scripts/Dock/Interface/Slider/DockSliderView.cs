using System;
using Dock.Interface.Slider.Card;
using UnityEngine;
using UnityEngine.UI;

namespace Dock.Interface.Slider
{
    public class DockSliderView : MonoBehaviour, IDockSliderView
    {
        public ScrollRect ScrollRect;
        public Transform ContentRoot;
        public Button NextButton;
        public Button PreviousButton;

        private const float ScrollSpeed = 1f;

        public event Action OnNextClick;
        public event Action OnPreviousClick;

        public IDockSliderShipCardView InstantiateShipCard(GameObject cardGo)
        {
            return Instantiate(cardGo, ContentRoot).GetComponent<DockSliderShipCardView>();
        }
        
        private void Start()
        {
            NextButton.onClick.AddListener(HandleNextClick);
            PreviousButton.onClick.AddListener(HandlePreviousClick);
        }

        private void OnDestroy()
        {
            NextButton.onClick.RemoveListener(HandleNextClick);
            PreviousButton.onClick.RemoveListener(HandlePreviousClick);
        }

        private void HandleNextClick()
        {
            OnNextClick?.Invoke();
        }

        private void HandlePreviousClick()
        {
            OnPreviousClick?.Invoke();
        }

        public void ScrollNext()
        {
            if (ScrollRect.horizontalNormalizedPosition <= 1f)
            {
                ScrollRect.horizontalNormalizedPosition += ScrollSpeed;
            }
        }
        
        public void ScrollPrevious()
        {
            if (ScrollRect.horizontalNormalizedPosition >= 0f)
            {
                ScrollRect.horizontalNormalizedPosition -= ScrollSpeed;
            }
        }
    }
}