using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dock.Interface.Header
{
    public class DockHeaderView : MonoBehaviour, IDockHeaderView
    {
        public event Action OnBackClick;
        
        public Button BackButton;
        public string HomeSceneId;

        private void Start()
        {
            BackButton.onClick.AddListener(HandleBackClick);
        }

        private void OnDestroy()
        {
            BackButton.onClick.RemoveListener(HandleBackClick);
        }

        private void HandleBackClick()
        {
            OnBackClick?.Invoke();
        }

        public string GetHomeSceneId()
        {
            return HomeSceneId;
        }
    }

    public interface IDockHeaderView
    {
        event Action OnBackClick;
        string GetHomeSceneId();
    }
}