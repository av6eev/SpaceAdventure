using ReactiveField;
using Specifications.Asteroid;

namespace Entities.Asteroids.Asteroid
{
    public class AsteroidModel : Entity, IAsteroidModel
    {
        public string ChunkId;
        public AsteroidSpecification Specification { get; }
        public float Speed { get; }
        public bool IsVisibleFromCamera { get; set; }
        public bool IsDisabled { get; set; }

        public AsteroidModel(AsteroidSpecification specification, float speedShift)
        {
            Specification = specification;
            CurrentHealth = new ReactiveField<float>(specification.Health);
            Speed = specification.Speed * speedShift;
        }
    }
}