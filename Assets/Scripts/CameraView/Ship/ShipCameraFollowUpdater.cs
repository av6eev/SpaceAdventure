using System;
using Entities.Ship;
using UnityEngine;
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
            MoveUpdate(deltaTime);
            RotateUpdate();
        }

        private void RotateUpdate()
        {
            var newRotation = Quaternion.Lerp(_shipCameraView.Rotation, _shipView.Rotation, _shipCameraView.RotationFollowStrength);
            _shipCameraView.Rotate(newRotation);
        }

        private void MoveUpdate(float deltaTime)
        {
            float currentPositionFollowStrength;
            
            var shipSpeed = _shipView.Speed;
            var firstSpeed = 40f;
            var secondSpeed = 65f;
            var thirdSpeed = 80f;
            var absX = Math.Abs(shipSpeed.x);
            var absY = Math.Abs(shipSpeed.y);
            var absZ = Math.Abs(shipSpeed.z);
            
            if (absX > thirdSpeed || absY > thirdSpeed || absZ > thirdSpeed)
            {
                currentPositionFollowStrength = 1f;
            }
            else if (absX > secondSpeed || absY > secondSpeed || absZ > secondSpeed)
            {
                currentPositionFollowStrength = .9f;
            }
            else if (absX > firstSpeed || absY > firstSpeed || absZ > firstSpeed)
            {
                currentPositionFollowStrength = .8f;
            }
            else
            {
                currentPositionFollowStrength = _shipCameraModel.InitialPositionFollowStrength;
            }

            _shipCameraView.PositionFollowStrength = Mathf.Lerp(_shipCameraView.PositionFollowStrength, currentPositionFollowStrength, deltaTime);

            var newTargetPosition = _shipView.TransformPoint(new Vector3(_shipCameraView.SpinOffset * -_shipView.CameraTargetInverseTransformDirection.z + _shipCameraView.YawOffset * _shipView.CameraTargetInverseTransformDirection.y, 0f, 0f));
            var lerpedNewPosition = Vector3.Lerp(_shipView.Position, newTargetPosition, _shipCameraView.PositionFollowStrength);
            
            _shipCameraView.Follow(lerpedNewPosition);
        }
    }
}