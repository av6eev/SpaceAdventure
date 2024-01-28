using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dock.Interface.Slider.Card
{
    public class DockSliderShipCardView : MonoBehaviour, IDockSliderShipCardView
    {
        public event Action OnClick;
        
        public Button Button;
        public TextMeshProUGUI TitleText;
        public GameObject UnselectedBackground;
        public GameObject SelectedBackground;
        public GameObject LockImageRoot;

        private void Start()
        {
            Button.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick()
        {
            OnClick?.Invoke();
        }

        public void SetTitle(string title)
        {
            TitleText.text = title;
        }

        public void EnableSelectedHighlight()
        {
            UnselectedBackground.SetActive(false);
            SelectedBackground.SetActive(true);
        }
        
        public void DisableSelectedHighlight()
        {
            UnselectedBackground.SetActive(true);
            SelectedBackground.SetActive(false);
        }

        public void DisableLockIcon()
        {
            LockImageRoot.SetActive(false);
        }

        public void EnableLockIcon()
        {
            LockImageRoot.SetActive(true);
        }
    }

    public interface IDockSliderShipCardView
    {
        event Action OnClick;
        void SetTitle(string title);
        void EnableSelectedHighlight();
        void DisableSelectedHighlight();
        void DisableLockIcon();
        void EnableLockIcon();
    }
}