using ReactiveField;

namespace CameraView.Ship
{
    public class ShipCameraModel : IShipCameraModel
    {
        public bool IsFollow { get; set; }
        public float InitialPositionFollowStrength { get; }
        public float InitialRotationFollowStrength { get; }
        public ReactiveField<float> CurrentPositionFollowStrength { get; set; } = new();
        public ReactiveField<float> CurrentRotationFollowStrength { get; set; } = new();

        public ShipCameraModel(float positionFollowStrength, float rotationFollowStrength)
        {
            InitialPositionFollowStrength = positionFollowStrength;
            InitialRotationFollowStrength = rotationFollowStrength;
            CurrentPositionFollowStrength.Value = InitialRotationFollowStrength;
            CurrentRotationFollowStrength.Value = InitialRotationFollowStrength;
        }
    }
}