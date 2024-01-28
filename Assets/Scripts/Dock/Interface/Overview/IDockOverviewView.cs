using UnityEngine;

namespace Dock.Interface.Overview
{
    public interface IDockOverviewView
    {
        DockShipGateAnimationController AnimationController { get; }
        GameObject InstantiateShip(GameObject go);
        void DestroyShip(GameObject go);
    }
}