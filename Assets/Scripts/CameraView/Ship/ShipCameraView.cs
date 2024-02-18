using Entities.Ship;
using UnityEngine;

namespace CameraView.Ship
{
    public class ShipCameraView : MonoBehaviour, IShipCameraView
    {
        public Camera Camera;
        
        private ShipView _target;
        public IShipView Target
        {
            get => _target;
            set => _target = (ShipView)value;
        }

        public float SpinOffsetCoefficient = .3f;
        public float YawLateralOffset = .4f;
        public float PositionFollowPower = 1f;
        public float RotationFollowPower = .35f;

        public Quaternion Rotation { get; }
        public float SpinOffset => SpinOffsetCoefficient;
        public float YawOffset => YawLateralOffset;
        public float PositionFollowStrength
        {
            get => PositionFollowPower;
            set => PositionFollowPower = value;
        }
        public float RotationFollowStrength
        {
            get => RotationFollowPower;
            set => RotationFollowPower = value;
        }
        
        public Vector3 Position => transform.position;
        private Vector3 _velocity;

        public void Rotate(Quaternion newRotation)
        {
            transform.rotation = newRotation;
        }

        public void Follow(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public bool CheckObjectVisibleFromCamera(Vector3 objectPosition)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(Camera);

            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(objectPosition) < 0)
                {
                    return false;
                }
            }

            return true;
        }
        
        public float GetRandomPointInCameraHeight()
        {
            var yPoint = Random.Range(0, Camera.pixelHeight);
            return Camera.ScreenToWorldPoint(new Vector3(0, yPoint, 0)).y;
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}