using ReactiveField;

namespace CameraView.Ship
{
    public interface IShipCameraModel
    {
        bool IsFollow { get; set; }
        float InitialPositionFollowStrength { get; }
        float InitialRotationFollowStrength { get; }
        ReactiveField<float> CurrentPositionFollowStrength { get; set; }
        ReactiveField<float> CurrentRotationFollowStrength { get; set; }
    }
}