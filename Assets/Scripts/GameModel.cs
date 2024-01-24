using Loader.Object;
using Loader.Scene;
using Specifications;
using Updater;

public class GameModel : IGameModel
{
    public IUpdatersList UpdatersEngine { get; set; }
    public IUpdatersList FixedUpdatersEngine { get; set; }
    public IUpdatersList LateUpdatersEngine { get; set; }
    public ILoadObjectsModel LoadObjectsModel { get; set; }
    public IGameSpecifications Specifications { get; set; }
    public ILoadScenesModel LoadScenesModel { get; set; }
}