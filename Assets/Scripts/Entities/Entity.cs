using System;
using ReactiveField;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public abstract class Entity : IEntity
    {
        public event Action OnDestroy;
        
        public string Id { get; } = Random.Range(0, 10000000).ToString();
        public ReactiveField<Vector3> Position { get; } = new(Vector3.zero);
        public ReactiveField<float> CurrentHealth { get; protected set; } = new(0);

        public virtual void ApplyDamage(float damage)
        {
            CurrentHealth.Value -= damage;
            
            if (CurrentHealth.Value <= 0)
            {
                OnDestroy?.Invoke();
            }
        }
    }
}