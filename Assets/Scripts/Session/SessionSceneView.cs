using System;
using CameraView.Ship;
using Entities.Ship;
using Input;
using LocationBuilder;
using Space;
using UnityEngine;

namespace Session
{
    public class SessionSceneView : LocationSceneView, ISessionSceneView
    {
        public ShipCameraView ShipCameraViewGo;
        public SpaceView SpaceViewGo;
        public Transform ShipSpawnPoint;

        public ISpaceView SpaceView => SpaceViewGo;
        public IShipCameraView ShipCameraView => ShipCameraViewGo;
        public IShipView ShipView { get; private set; }

        public IShipView InstantiateShip(GameObject go)
        {
            var newGo = Instantiate(go, ShipSpawnPoint.position, Quaternion.identity, null);
            var rb = newGo.AddComponent<Rigidbody>();

            if (!newGo.TryGetComponent(out ShipView shipView))
            {
                throw new Exception("Couldn't find ShipView component on ship the prefab");
            }

            rb.useGravity = false;
            rb.drag = .5f;
            rb.angularDrag = .5f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            return ShipView = shipView;
        }

        public T InstantiateInput<T>() where T : Component, IInputView
        {
            var inputView = new GameObject("InputView").AddComponent<T>();
            
            inputView.transform.SetParent(transform);
            
            return inputView;
        }

        public void SetupCamera()
        {
            ShipCameraViewGo.Target = ShipView;
        }
    }
}