using System;

namespace Save
{
    public class SaveModel : ISaveModel
    {
        public event Action<string> OnShipIdChanged; 
        
        public void SaveCurrentShipId(string id)
        {
            OnShipIdChanged?.Invoke(id);
        }

        public void Save()
        {
        }

        public void Deserialize()
        {
        }
    }
}