namespace CameraView.Ship
{
    public interface IShipCameraModel
    {
        bool IsFollow { get; set; }
        float InitialPositionFollowStrength { get; }
        float InitialRotationFollowStrength { get; }
    }
}