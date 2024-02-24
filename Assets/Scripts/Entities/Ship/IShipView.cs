using UnityEngine;

namespace Entities.Ship
{
    public interface IShipView
    {
        Vector3 Speed { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 CameraTargetInverseTransformDirection { get; }
        void AddRelativeTorque(Vector3 torque);
        void AddRelativeForce(Vector3 thrust);
        void ChangeBoostEffectSpeed(Vector2 speed);
        void ChangeBoostEffectSize(float size);
        Vector3 TransformDirection(Vector3 forward);
        Vector3 TransformPoint(Vector3 point);
    }
}