using System;
using ReactiveField;
using Specifications.Ship;
using UnityEngine;

namespace Entities.Ship
{
    public class ShipModel : IShipModel
    {
        public event Action OnDamageApplied;

        public ShipSpecification Specification { get; }
        public ReactiveField<Vector3> Position { get; } = new(new Vector3(0,0,0));
        public ReactiveField<Vector3> CurrentSpeed { get; } = new(new Vector3(0,0,0));
        public ReactiveField<float> CurrentBoostAmount { get; } = new();
        public ReactiveField<float> CurrentHealth { get; }
        public bool IsImmune { get; set; }

        public ShipModel(ShipSpecification specification)
        {
            Specification = specification;
            CurrentHealth = new ReactiveField<float>(specification.Health);
            CurrentBoostAmount.Value = Specification.MaxBoostAmount;
        }

        public void ApplyDamage(float damage)
        {
            if (IsImmune) return;
            
            CurrentHealth.Value -= damage;
            OnDamageApplied?.Invoke();
        }
    }
}