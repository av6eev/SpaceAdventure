using ReactiveField;
using Specifications.Ship;
using UnityEngine;

namespace Entities.Ship
{
    public interface IShipModel : IEntity, IMovable 
    {
        ShipSpecification Specification { get; }
        ReactiveField<Vector3> CurrentSpeed { get; }
        ReactiveField<float> CurrentBoostAmount { get; }
        public bool IsImmune { get; }
    }
}