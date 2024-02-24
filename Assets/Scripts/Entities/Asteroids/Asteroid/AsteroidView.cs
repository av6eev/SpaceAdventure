using UnityEngine;

namespace Entities.Asteroids.Asteroid
{
    [RequireComponent(typeof(Rigidbody))]
    public class AsteroidView : MonoBehaviour, IAsteroidView
    {
        public Rigidbody Rigidbody;
        public MeshRenderer[] MeshRenderers;
        public Collider Collider;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Awake()
        {
            var materialPropertyBlock = new MaterialPropertyBlock();
            
            foreach (var meshRenderer in MeshRenderers)
            {
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
        }

        public void Show()
        {
            foreach (var meshRenderer in MeshRenderers)
            {
                meshRenderer.enabled = true;
            }
            
            Collider.enabled = true;
        }

        public void Hide()
        {
            foreach (var meshRenderer in MeshRenderers)
            {
               meshRenderer.enabled = false;
            }
            
            Collider.enabled = false;
        }

        public void Rotate(Vector3 torque)
        {
            Rigidbody.AddTorque(torque);
        }

        public void Move(Vector3 direction)
        {
            Rigidbody.AddForce(direction, ForceMode.Acceleration);
        }
    }
}