using Entities.Asteroids.Asteroid;
using Pulls;

namespace Entities.Asteroids.Pull
{
    public interface IAsteroidsPullsCollection
    {
        IPull<IAsteroidView> SmallAsteroidPull { get; }
        IPull<IAsteroidView> MediumAsteroidPull { get; }
        IPull<IAsteroidView> BigAsteroidPull { get; }
        IPull<IAsteroidView> FireAsteroidPull { get; }

        IPull<IAsteroidView> this[string key] { get; }
    }
}