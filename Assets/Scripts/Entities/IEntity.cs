using ReactiveField;

namespace Entities
{
    public interface IEntity : IDamageable, IMovable
    {
        string Id { get; }
        ReactiveField<float> CurrentHealth { get; }
    }
}