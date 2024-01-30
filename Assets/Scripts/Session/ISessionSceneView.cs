using CameraView.Ship;
using Entities.Ship;
using Input;
using UnityEngine;

namespace Session
{
    public interface ISessionSceneView
    {
        IShipCameraView ShipCameraView { get; }
        IShipView ShipView { get; }
        IShipView InstantiateShip(GameObject go);
        T InstantiateInput<T>() where T : Component, IInputView;
        void SetupCamera();
    }
}