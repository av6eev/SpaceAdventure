using CameraView.Ship;
using UnityEngine;
using Updater;

namespace Entities.Asteroids.Asteroid.Physics
{
    public class AsteroidPhysicsUpdater : IUpdater
    {
        private readonly AsteroidModel _asteroidModel;
        private readonly IAsteroidView _asteroidView;
        private readonly IShipCameraView _shipCameraView;

        private readonly Vector3 _thrustSpeed;
        private readonly Vector3 _torqueSpeed;

        public AsteroidPhysicsUpdater(AsteroidModel asteroidModel, IAsteroidView asteroidView, IShipCameraView shipCameraView)
        {
            _asteroidModel = asteroidModel;
            _asteroidView = asteroidView;
            _shipCameraView = shipCameraView;

            _thrustSpeed = new Vector3(
                Random.Range(-2f, 2f),
                Random.Range(-2f, 2f),
                Random.Range(-2f, 2f)) * 15f;
            _torqueSpeed = new Vector3(
                Random.Range(-20f, 20f),
                Random.Range(-20f, 20f),
                Random.Range(-20f, 20f)) * 10f;
        }
    
        public void Update(float deltaTime)
        {
            MoveUpdate(deltaTime);
            RotateUpdate(deltaTime);
            VisibilityUpdate();
        }

        private void VisibilityUpdate()
        {
            _asteroidModel.IsVisibleFromCamera = _shipCameraView.CheckObjectVisibleFromCamera(_asteroidView.Position);
        }

        private void MoveUpdate(float deltaTime)
        {
            _asteroidView.Move(_thrustSpeed * deltaTime);
            _asteroidModel.Position.Value = _asteroidView.Position;
        }

        private void RotateUpdate(float deltaTime)
        {
            _asteroidView.Rotate(_torqueSpeed * deltaTime);
        }
    }
}