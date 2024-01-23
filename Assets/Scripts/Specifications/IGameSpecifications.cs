using Specification.Scene;
using Specifications.Asteroid;
using Specifications.Bullet;
using Specifications.Collection;
using Specifications.Ship;
using Specifications.Weapon;

namespace Specifications
{
    public interface IGameSpecifications
    {
        INewSpecificationsCollection<BulletSpecification> BulletSpecifications { get; }
        INewSpecificationsCollection<AsteroidSpecification> AsteroidSpecifications { get; }
        INewSpecificationsCollection<ShipSpecification> ShipSpecifications { get; }
        INewSpecificationsCollection<WeaponSpecification> WeaponSpecifications { get; } 
        INewSpecificationsCollection<SceneSpecification> SceneSpecifications { get; }
    }
}