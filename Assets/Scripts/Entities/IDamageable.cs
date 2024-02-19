using System;

namespace Entities
{
    public interface IDamageable
    {
        event Action OnDestroy;
        void ApplyDamage(float damage);
    }
}