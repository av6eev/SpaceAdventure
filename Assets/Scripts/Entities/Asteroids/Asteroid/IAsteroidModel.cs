using Specifications.Asteroid;

namespace Entities.Asteroids.Asteroid
{
    public interface IAsteroidModel : IEntity
    {
        AsteroidSpecification Specification { get; }
        float Speed { get; }
        bool IsVisibleFromCamera { get; }
    }
}