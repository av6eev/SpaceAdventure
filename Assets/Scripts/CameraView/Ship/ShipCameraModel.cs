namespace CameraView.Ship
{
    public class ShipCameraModel : IShipCameraModel
    {
        public bool IsFollow { get; set; }
        public float InitialPositionFollowStrength { get; }
        public float InitialRotationFollowStrength { get; }

        public ShipCameraModel(float positionFollowStrength, float rotationFollowStrength)
        {
            InitialPositionFollowStrength = positionFollowStrength;
            InitialRotationFollowStrength = rotationFollowStrength;
        }
    }
}