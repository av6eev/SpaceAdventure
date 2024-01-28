using System;

namespace Dock.Interface.Play
{
    public interface IDockPlayView
    {
        event Action OnPlayClicked;
        string GetNextSceneId();
        void ShowPlayButton();
        void HidePlayButton();
    }
}