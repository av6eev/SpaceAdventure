using Pulls;
using UnityEngine;

namespace Entities.Asteroids.Asteroid
{
    public interface IAsteroidView : IPullObject
    {
        Vector3 Position { get; set; }
        void Rotate(Vector3 torque);
        void Move(Vector3 direction);
    }
}