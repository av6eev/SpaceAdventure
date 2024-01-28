using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dock.Interface.Play
{
    public class DockPlayView : MonoBehaviour, IDockPlayView
    {
        public event Action OnPlayClicked;
        
        public Button PlayButton;
        public string SwitchToSceneId;

        private void Start()
        {
            PlayButton.onClick.AddListener(HandlePlayClick);
        }

        private void OnDestroy()
        {
            PlayButton.onClick.RemoveListener(HandlePlayClick);
        }

        private void HandlePlayClick()
        {
            OnPlayClicked?.Invoke();
        }

        public string GetNextSceneId()
        {
            return SwitchToSceneId;
        }

        public void ShowPlayButton()
        {
            PlayButton.gameObject.SetActive(true);
        }
        
        public void HidePlayButton()
        {
            PlayButton.gameObject.SetActive(false);
        }
    }
}