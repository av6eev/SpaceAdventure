using Specifications.Asteroid;

namespace Entities.Asteroids.Asteroid
{
    public interface IAsteroidModel : IEntity, IMovable
    {
        AsteroidSpecification Specification { get; }
        float Speed { get; }
        bool IsVisibleFromCamera { get; }
    }
}