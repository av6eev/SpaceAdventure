using ReactiveField;
using UnityEngine;

namespace Entities
{
    public interface IMovable
    {
        ReactiveField<Vector3> Position { get; }
    }
}