using Updater;

namespace Input
{
    public abstract class BaseInputUpdater : IUpdater
    {
        public abstract void Update(float deltaTime);
    }
}