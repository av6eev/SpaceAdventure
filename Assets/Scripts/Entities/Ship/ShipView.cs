using UnityEngine;

namespace Entities.Ship
{
    public class ShipView : MonoBehaviour, IShipView
    {
        public Transform CameraLockGo;
        public Rigidbody Rigidbody;
        public Transform BulletSpawnPoint;
        public Material BoostMaterial;

        public Vector3 Speed => Rigidbody.velocity;
        public Vector3 Position => transform.position;
        public Vector3 CameraTargetInverseTransformDirection => CameraLockGo.transform.InverseTransformDirection(Rigidbody.angularVelocity);
        
        private static readonly int SpeedProperty = Shader.PropertyToID("_Speed");
        private static readonly int SizeProperty = Shader.PropertyToID("_Size");

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void AddRelativeTorque(Vector3 torque)
        {
            Rigidbody.AddRelativeTorque(torque);
        }

        public void AddRelativeForce(Vector3 thrust)
        {
            Rigidbody.AddRelativeForce(thrust);
        }

        public void ChangeBoostEffectSpeed(Vector2 speed)
        {
            BoostMaterial.SetVector(SpeedProperty, Vector4.Lerp(BoostMaterial.GetVector(SpeedProperty), new Vector4(speed.x, speed.y, 0f, 0f), 0.3f));
        }

        public void ChangeBoostEffectSize(float size)
        {
            BoostMaterial.SetFloat(SizeProperty, Mathf.Lerp(BoostMaterial.GetFloat(SizeProperty), size, 0.4f));
        }
    }
}