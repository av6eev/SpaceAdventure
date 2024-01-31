using Entities.Asteroids.Asteroid;
using Pulls;

namespace Entities.Asteroids.Pull
{
    public class AsteroidsPullsCollection : IAsteroidsPullsCollection
    {
        public IPull<IAsteroidView> SmallAsteroidPull { get; set; }
        public IPull<IAsteroidView> MediumAsteroidPull { get; set; }
        public IPull<IAsteroidView> BigAsteroidPull { get; set; }
        public IPull<IAsteroidView> FireAsteroidPull { get; set; }
        
        public IPull<IAsteroidView> GetById(string key)
        {
            return key switch
            {
                "small_asteroid" => SmallAsteroidPull,
                "medium_asteroid" => MediumAsteroidPull,
                "big_asteroid" => BigAsteroidPull,
                "fire_asteroid" => FireAsteroidPull,
                _ => null
            };
        }
    }
}