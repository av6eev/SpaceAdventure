using UnityEngine;

namespace Dock.Interface.Overview
{
    public class DockOverviewView : MonoBehaviour, IDockOverviewView
    {
        public Transform OverviewShipSpawnPoint;
        public Transform SpawnRoot;
        public DockShipGateAnimationController AnimationControllerGo;

        public DockShipGateAnimationController AnimationController => AnimationControllerGo;

        public GameObject InstantiateShip(GameObject go)
        {
            var result = Instantiate(go, SpawnRoot);
            
            result.transform.localScale = Vector3.one;
            
            return result;
        }

        public void DestroyShip(GameObject go)
        {
            Destroy(go);
        }
    }
}