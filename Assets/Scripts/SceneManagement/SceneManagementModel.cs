using System;

namespace SceneManagement
{
    public class SceneManagementModel : ISceneManagementModel
    {
        public event Action<string> OnSceneSwitched; 
        public string CurrentSceneId;

        public SceneManagementModel(string startSceneId)
        {
            CurrentSceneId = startSceneId;
        }

        public void SwitchScene(string id)
        {
            OnSceneSwitched?.Invoke(id);
        }
    }
}