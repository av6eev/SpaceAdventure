using System;
using Input;
using UnityEngine;
using Updater;

namespace Entities.Ship.Physics
{
    public class ShipPhysicsUpdater : IUpdater
    {
        private readonly IInputModel _inputModel;
        private readonly IShipView _shipView;
        private readonly IShipModel _shipModel;

        private float _currentRotation;
        private float _glide;
        private float _verticalGlide;
        private float _horizontalGlide;
        private float _boostAmount;
        
        public ShipPhysicsUpdater(IInputModel inputModel, IShipView shipView, IShipModel shipModel)
        {
            _inputModel = inputModel;
            _shipView = shipView;
            _shipModel = shipModel;

            _boostAmount = _shipModel.Specification.MaxBoostAmount;
        }
        
        public void Update(float deltaTime)
        {
            BoostUpdate();
            RollUpdate(deltaTime);
            PitchUpdate(deltaTime);
            YawUpdate(deltaTime);
            ThrustUpdate(deltaTime);
            UpDownUpdate(deltaTime);
            StrafeUpdate(deltaTime);

            _shipModel.CurrentSpeed.Value = _shipView.GetSpeed();
            _shipModel.Position.Value = _shipView.GetPosition();
            _shipModel.CurrentBoostAmount.Value = _boostAmount;
        }

        private void BoostUpdate()
        {
            var shipSpecification = _shipModel.Specification;
            
            if (_inputModel.IsBoosted && _boostAmount > 0f)
            {
                _boostAmount -= shipSpecification.BoostDeprecationRate;
                
                if (_boostAmount <= 0f)
                {
                    _inputModel.IsBoosted = false;
                }
            }
            else
            {
                if (_boostAmount < shipSpecification.MaxBoostAmount)
                {
                    _boostAmount += shipSpecification.BoostRechargeRate;
                }
            }
        }

        private void ThrustUpdate(float deltaTime)
        {
            var specificationThrust = _shipModel.Specification.Thrust;
            var inputThrust = _inputModel.Thrust;

            if (inputThrust > .1f || inputThrust < -.1f)
            {
                float currentThrust;

                if (_inputModel.IsBoosted)
                {
                    currentThrust = specificationThrust * _shipModel.Specification.BoostMultiplier;
                }
                else
                {
                    currentThrust = specificationThrust;
                }
                
                _shipView.AddRelativeForce(Vector3.forward * (inputThrust * currentThrust * deltaTime));
                _glide = specificationThrust;
            }
            else
            {
                _shipView.AddRelativeForce(Vector3.forward * (_glide * deltaTime));
                _glide *= _shipModel.Specification.ThrustGlideReduction;
            }
        }

        private void StrafeUpdate(float deltaTime)
        {
            var specificationStrafeThrust = _shipModel.Specification.StrafeThrust;
            var inputStrafe = _inputModel.Strafe;

            if (inputStrafe > .1f || inputStrafe < -.1f)
            {
                _shipView.AddRelativeForce(Vector3.right * (inputStrafe * specificationStrafeThrust * deltaTime));
                _horizontalGlide = inputStrafe * specificationStrafeThrust;
            }
            else
            {
                _shipView.AddRelativeForce(Vector3.right * (_horizontalGlide * deltaTime));
                _horizontalGlide *= _shipModel.Specification.LeftRightGlideReduction;
            }
        }

        private void UpDownUpdate(float deltaTime)
        {
            var specificationUpThrust = _shipModel.Specification.UpThrust;
            var inputUpDown = _inputModel.UpDown;

            if (inputUpDown > .1f || inputUpDown < -.1f)
            {
                _shipView.AddRelativeForce(Vector3.up * (inputUpDown * specificationUpThrust * deltaTime));
                _verticalGlide = inputUpDown * specificationUpThrust;
            }
            else
            {
                _shipView.AddRelativeForce(Vector3.up * (_verticalGlide * deltaTime));
                _verticalGlide *= _shipModel.Specification.UpDownGlideReduction;
            }
        }

        private void YawUpdate(float deltaTime)
        {
            _shipView.AddRelativeTorque(Vector3.up * (Math.Clamp(_inputModel.PitchYaw.x, -1f, 1f) * _shipModel.Specification.YawTorque * deltaTime));
        }

        private void PitchUpdate(float deltaTime)
        {
            _shipView.AddRelativeTorque(Vector3.right * (Math.Clamp(-_inputModel.PitchYaw.y, -1f, 1f) * _shipModel.Specification.PitchTorque * deltaTime));
        }

        private void RollUpdate(float deltaTime)
        {
            _shipView.AddRelativeTorque(Vector3.back * (_inputModel.Roll * _shipModel.Specification.RollTorque * deltaTime));
        }
    }
}