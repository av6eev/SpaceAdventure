using Presenter;
using UnityEngine;
using Utilities;

namespace Save
{
    public class SavePresenter : IPresenter
    {
        private readonly IGameModel _gameModel;
        private readonly SaveModel _model;

        public SavePresenter(IGameModel gameModel, SaveModel model)
        {
            _gameModel = gameModel;
            _model = model;
        }
        
        public void Init()
        {
            _model.OnShipIdChanged += SaveShipId;
        }

        public void Dispose()
        {
            _model.OnShipIdChanged -= SaveShipId;
        }

        private void SaveShipId(string id)
        {
            PlayerPrefs.SetString(SavingElementsKeys.CurrentShipIdKey, id);
            Debug.Log($"{id} saved to: {SavingElementsKeys.CurrentShipIdKey}");
        }
    }
}