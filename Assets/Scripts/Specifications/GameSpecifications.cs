using Awaiter;
using Loader.Object;
using Specification.Scene;
using Specification.Startup;
using Specifications.Asteroid;
using Specifications.Bullet;
using Specifications.Collection;
using Specifications.LoadWrapper;
using Specifications.Ship;
using Specifications.Weapon;

namespace Specifications
{
    public class GameSpecifications : IGameSpecifications
    {
        public INewSpecificationsCollection<BulletSpecification> BulletSpecifications { get; } = new NewSpecificationsCollection<BulletSpecification>();
        public INewSpecificationsCollection<AsteroidSpecification> AsteroidSpecifications { get; } = new NewSpecificationsCollection<AsteroidSpecification>();
        public INewSpecificationsCollection<ShipSpecification> ShipSpecifications { get; } = new NewSpecificationsCollection<ShipSpecification>();
        public INewSpecificationsCollection<WeaponSpecification> WeaponSpecifications { get; } = new NewSpecificationsCollection<WeaponSpecification>();
        public INewSpecificationsCollection<SceneSpecification> SceneSpecifications { get; } = new NewSpecificationsCollection<SceneSpecification>();
        
        public readonly CustomAwaiter LoadAwaiter = new();
        
        public GameSpecifications(StartupSpecification startupSpecification, ILoadObjectsModel loadObjectsModel)
        {
            Load(startupSpecification, loadObjectsModel);
        }

        private async void Load(StartupSpecification startupSpecification, ILoadObjectsModel loadObjectsModel)
        {
            await new LoadSpecificationsWrapper<BulletSpecification>(loadObjectsModel, "bullets", BulletSpecifications, startupSpecification).LoadAwaiter;
            await new LoadSpecificationsWrapper<AsteroidSpecification>(loadObjectsModel, "asteroids", AsteroidSpecifications, startupSpecification).LoadAwaiter;
            await new LoadSpecificationsWrapper<ShipSpecification>(loadObjectsModel, "ships", ShipSpecifications, startupSpecification).LoadAwaiter;
            await new LoadSpecificationsWrapper<WeaponSpecification>(loadObjectsModel, "weapons", WeaponSpecifications, startupSpecification).LoadAwaiter;
            await new LoadSpecificationsWrapper<SceneSpecification>(loadObjectsModel, "scenes", SceneSpecifications, startupSpecification).LoadAwaiter;
            
            LoadAwaiter.Complete();
        }
    }
}