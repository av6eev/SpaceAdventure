using UnityEngine;

namespace CameraView
{
    public interface ICameraView
    {
        Vector3 Position { get; }
        float PositionFollowStrength { get; set; }
        float RotationFollowStrength { get; set; }
        void Show();
        void Hide();
        void Rotate();
        void Follow(float spinLateralOffset, float yawLateralOffset);
        bool CheckObjectVisibleFromCamera(Vector3 objectPosition);
        Vector3 GetRandomPointInCameraView(float forwardBorder, float xBorderOffset = 0f, float yBorderOffset = 0f, float zOffset = 0f);
    }
}