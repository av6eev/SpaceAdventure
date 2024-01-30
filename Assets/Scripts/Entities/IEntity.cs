using ReactiveField;

namespace Entities
{
    public interface IEntity : IDamageable
    {
        ReactiveField<float> CurrentHealth { get; }
    }
}