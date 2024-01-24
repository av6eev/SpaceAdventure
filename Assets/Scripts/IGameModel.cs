using Loader.Object;
using Loader.Scene;
using Specifications;
using Updater;

public interface IGameModel : IBaseGameModel
{
    IUpdatersList UpdatersEngine { get; }
    IUpdatersList FixedUpdatersEngine { get; }
    IUpdatersList LateUpdatersEngine { get; }
    ILoadObjectsModel LoadObjectsModel { get; }
    ILoadScenesModel LoadScenesModel { get; }
    IGameSpecifications Specifications { get; }
}