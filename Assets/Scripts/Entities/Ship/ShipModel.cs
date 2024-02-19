using System;
using ReactiveField;
using Specifications.Ship;
using UnityEngine;

namespace Entities.Ship
{
    public class ShipModel : Entity, IShipModel
    {
        public ShipSpecification Specification { get; }
        public ReactiveField<Vector3> CurrentSpeed { get; } = new(new Vector3(0,0,0));
        public ReactiveField<float> CurrentBoostAmount { get; } = new();
        public bool IsImmune { get; set; }

        public ShipModel(ShipSpecification specification)
        {
            Specification = specification;
            CurrentHealth = new ReactiveField<float>(specification.Health);
            CurrentBoostAmount.Value = Specification.MaxBoostAmount;
        }
    }
}