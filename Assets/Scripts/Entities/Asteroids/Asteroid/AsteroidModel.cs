using System;
using ReactiveField;
using Specifications.Asteroid;
using UnityEngine;

namespace Entities.Asteroids.Asteroid
{
    public class AsteroidModel : IAsteroidModel
    {
        public event Action OnDestroy;
        
        public AsteroidSpecification Specification { get; }
        public ReactiveField<Vector3> Position { get; } = new(new Vector3(0,0,0));
        public ReactiveField<float> CurrentHealth { get; }
        public float Speed { get; }
        public bool IsVisibleFromCamera { get; set; }

        public AsteroidModel(AsteroidSpecification specification, float speedShift)
        {
            Specification = specification;
            CurrentHealth = new ReactiveField<float>(specification.Health);
            Speed = specification.Speed * speedShift;
        }

        public void ApplyDamage(float damage)
        {
            CurrentHealth.Value -= damage;
            
            if (CurrentHealth.Value <= 0)
            {
                OnDestroy?.Invoke();
            }
        }
    }
}