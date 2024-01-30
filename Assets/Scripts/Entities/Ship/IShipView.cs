using UnityEngine;

namespace Entities.Ship
{
    public interface IShipView
    {
        Vector3 GetSpeed();
        Vector3 GetPosition();
        void AddRelativeTorque(Vector3 torque);
        void AddRelativeForce(Vector3 thrust);
        void ChangeBoostEffectSpeed(Vector2 speed);
        void ChangeBoostEffectSize(float size);
    }
}