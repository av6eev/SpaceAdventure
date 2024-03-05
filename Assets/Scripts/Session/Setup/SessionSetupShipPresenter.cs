using Awaiter;
using Entities.Ship;
using Presenter;
using UnityEngine;
using Utilities;

namespace Session.Setup
{
    public class SessionSetupShipPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly PresentersList _presenters;
        private readonly ISessionSceneView _view;

        public readonly CustomAwaiter LoadAwaiter = new();

        public SessionSetupShipPresenter(SessionLocationGameModel gameModel, PresentersList presenters, ISessionSceneView view)
        {
            _gameModel = gameModel;
            _presenters = presenters;
            _view = view;
        }
        
        public async void Init()
        {
            var currentShipId = PlayerPrefs.GetString(SavingElementsKeys.CurrentShipIdKey);
            var shipSpecification = _gameModel.Specifications.ShipSpecifications[currentShipId];
            var shipPrefabKey = shipSpecification.PrefabKey3D;
            var go = _gameModel.LoadObjectsModel.Load<GameObject>(shipPrefabKey);
            await go.LoadAwaiter;

            _gameModel.ShipModel = new ShipModel(shipSpecification);
            var shipView = _view.InstantiateShip(go.Result);
            var shipPresenter = new ShipPresenter(_gameModel, (ShipModel)_gameModel.ShipModel, shipView);
            shipPresenter.Init();
            _presenters.Add(shipPresenter);

            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            LoadAwaiter.Dispose();
        }
    }
}