using Entities.Ship;
using Input;
using UnityEngine;

namespace Session
{
    public interface ISessionSceneView
    {
        IShipView ShipView { get; }
        IShipView InstantiateShip(GameObject go);
        T InstantiateInput<T>() where T : Component, IInputView;
    }
}