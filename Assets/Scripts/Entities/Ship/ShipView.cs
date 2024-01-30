using UnityEngine;

namespace Entities.Ship
{
    public class ShipView : MonoBehaviour, IShipView
    {
        public Transform CameraLockGo;
        public Rigidbody Rigidbody;
        public Transform BulletSpawnPoint;
        public Material BoostMaterial;
        
        private static readonly int Speed = Shader.PropertyToID("_Speed");
        private static readonly int Size = Shader.PropertyToID("_Size");

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public Vector3 GetCameraTargetInverseTransformDirection()
        {
            return CameraLockGo.transform.InverseTransformDirection(Rigidbody.angularVelocity);
        }

        public void AddRelativeTorque(Vector3 torque)
        {
            Rigidbody.AddRelativeTorque(torque);
        }

        public void AddRelativeForce(Vector3 thrust)
        {
            Rigidbody.AddRelativeForce(thrust);
        }

        public Vector3 GetSpeed()
        {
            return Rigidbody.velocity;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void ChangeBoostEffectSpeed(Vector2 speed)
        {
            BoostMaterial.SetVector(Speed, Vector4.Lerp(BoostMaterial.GetVector(Speed), new Vector4(speed.x, speed.y, 0f, 0f), 0.3f));
        }

        public void ChangeBoostEffectSize(float size)
        {
            BoostMaterial.SetFloat(Size, Mathf.Lerp(BoostMaterial.GetFloat(Size), size, 0.4f));
        }

        public Vector3 GetBulletSpawnPoint()
        {
            throw new System.NotImplementedException();
        }

        public void EnableImmunity()
        {
            throw new System.NotImplementedException();
        }

        public void DisableImmunity()
        {
            throw new System.NotImplementedException();
        }
    }
}