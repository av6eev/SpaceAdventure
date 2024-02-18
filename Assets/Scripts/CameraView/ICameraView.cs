using Entities.Ship;
using UnityEngine;

namespace CameraView
{
    public interface ICameraView
    {
        IShipView Target { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        float SpinOffset { get; }
        float YawOffset { get; }
        float PositionFollowStrength { get; set; }
        float RotationFollowStrength { get; set; }
        bool CheckObjectVisibleFromCamera(Vector3 objectPosition);
        void Rotate(Quaternion newRotation);
        void Follow(Vector3 newPosition);
        void Show();
        void Hide();
    }
}