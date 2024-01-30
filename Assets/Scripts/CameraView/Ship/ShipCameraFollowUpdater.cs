using System;
using Entities.Ship;
using Updater;

namespace CameraView.Ship
{
    public class ShipCameraFollowUpdater : IUpdater
    {
        private readonly IShipCameraModel _shipCameraModel;
        private readonly IShipCameraView _shipCameraView;
        private readonly IShipView _shipView;

        public ShipCameraFollowUpdater(IShipCameraModel shipCameraModel, IShipCameraView shipCameraView, IShipView shipView)
        {
            _shipCameraModel = shipCameraModel;
            _shipCameraView = shipCameraView;
            _shipView = shipView;
        }
        
        public void Update(float deltaTime)
        {
            MoveUpdate();
            RotateUpdate();
        }

        private void RotateUpdate()
        {
            _shipCameraView.Rotate();
        }

        private void MoveUpdate()
        {
            var spinLateralOffset = -_shipView.CameraTargetInverseTransformDirection.z;
            var yawLateralOffset = _shipView.CameraTargetInverseTransformDirection.y;
            var shipSpeed = _shipView.Speed;

            var firstSpeed = 40f;
            var secondSpeed = 65f;
            var thirdSpeed = 80f;
            var absX = Math.Abs(shipSpeed.x);
            var absY = Math.Abs(shipSpeed.y);
            var absZ = Math.Abs(shipSpeed.z);
            
            if (absX > thirdSpeed || absY > thirdSpeed || absZ > thirdSpeed)
            {
                _shipCameraModel.CurrentPositionFollowStrength.Value = .2f;
            }
            else if (absX > secondSpeed || absY > secondSpeed || absZ > secondSpeed)
            {
                _shipCameraModel.CurrentPositionFollowStrength.Value = .3f;
            }
            else if (absX > firstSpeed || absY > firstSpeed || absZ > firstSpeed)
            {
                _shipCameraModel.CurrentPositionFollowStrength.Value = .4f;
            }
            else
            {
                _shipCameraModel.CurrentPositionFollowStrength.Value = _shipCameraModel.InitialPositionFollowStrength;
            }

            _shipCameraView.Follow(spinLateralOffset, yawLateralOffset);
        }
    }
}