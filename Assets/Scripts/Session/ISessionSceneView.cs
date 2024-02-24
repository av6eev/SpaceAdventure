using CameraView.Ship;
using Chunk.Collection;
using Entities.Ship;
using Input;
using UnityEngine;

namespace Session
{
    public interface ISessionSceneView
    {
        IShipCameraView ShipCameraView { get; }
        IShipView ShipView { get; }
        IChunkCollectionView ChunkCollectionView { get; }
        IShipView InstantiateShip(GameObject go);
        T InstantiateInput<T>() where T : Component, IInputView;
        void SetupCamera();
    }
}