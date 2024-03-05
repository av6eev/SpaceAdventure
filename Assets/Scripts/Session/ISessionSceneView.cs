using CameraView.Ship;
using Chunk.Collection;
using Entities.Ship;
using Input;
using Space;
using UnityEngine;

namespace Session
{
    public interface ISessionSceneView
    {
        ISpaceView SpaceView { get; }
        IShipCameraView ShipCameraView { get; }
        IShipView ShipView { get; }
        IShipView InstantiateShip(GameObject go);
        T InstantiateInput<T>() where T : Component, IInputView;
        void SetupCamera();
    }
}