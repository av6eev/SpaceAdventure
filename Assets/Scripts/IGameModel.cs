using Entities.Ship;
using Input;
using Loader.Object;
using Loader.Scene;
using Save;
using SceneManagement;
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
    ISceneManagementModel SceneManagementModel { get; }
    ISaveModel SaveModel { get; }
    IInputModel InputModel { get; }
    IShipModel ShipModel { get; }
}