using Entities.Ship;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CameraView.Ship
{
    public class ShipCameraView : MonoBehaviour, IShipCameraView
    {
        public Camera Camera;
        public ShipView Target { get; set; }

        public float SpinOffsetCoefficient = .3f;
        public float YawLateralOffset = .4f;
        public float PositionFollowPower = .7f;
        public float RotationFollowPower = .35f;
        private Vector3 _velocity;
        public Vector3 Position => transform.position;
        public float PositionFollowStrength
        {
            get => PositionFollowPower;
            set => PositionFollowPower = Mathf.Lerp(PositionFollowPower, value, .01f);
        }

        public float RotationFollowStrength
        {
            get => RotationFollowPower;
            set => RotationFollowPower = Mathf.Lerp(RotationFollowPower, value, .01f);
        }

        public void Rotate()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Target.transform.rotation, RotationFollowStrength);
        }

        public void Follow(float spinLateralOffset, float yawLateralOffset)
        {
            var newTargetPosition = Target.CameraLockGo.TransformPoint(new UnityEngine.Vector3(SpinOffsetCoefficient * spinLateralOffset + YawLateralOffset * yawLateralOffset, 0f, 0f));
            
            transform.position = (1 - PositionFollowStrength) * Position + PositionFollowStrength * newTargetPosition;
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
        
        public Vector3 GetRandomPointInCameraView(float forwardBorder, float xBorderOffset = 0f, float yBorderOffset = 0f, float zOffset = 0f)
        {
            var xPoint = Random.Range(xBorderOffset, Camera.pixelWidth - xBorderOffset);
            var yPoint = Random.Range(yBorderOffset, Camera.pixelHeight - yBorderOffset);
            var zPoint = Random.Range(zOffset, Mathf.Abs(forwardBorder));
            
            return Camera.ScreenToWorldPoint(new Vector3(xPoint, yPoint, zPoint));
        }
        
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}