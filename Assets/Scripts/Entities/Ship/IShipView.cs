using UnityEngine;

namespace Entities.Ship
{
    public interface IShipView
    {
        Vector3 Speed { get; }
        Vector3 Position { get; }
        Vector3 CameraTargetInverseTransformDirection { get; }
        void AddRelativeTorque(Vector3 torque);
        void AddRelativeForce(Vector3 thrust);
        void ChangeBoostEffectSpeed(Vector2 speed);
        void ChangeBoostEffectSize(float size);
    }
}