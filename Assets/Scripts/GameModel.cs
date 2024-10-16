﻿using Entities.Ship;
using Input;
using Loader.Object;
using Loader.Scene;
using Save;
using SceneManagement;
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
    public ISceneManagementModel SceneManagementModel { get; set; }
    public ISaveModel SaveModel { get; set; }
    public IInputModel InputModel { get; set; }
    public IShipModel ShipModel { get; set; }

    public GameModel() {}
    
    protected GameModel(IGameModel gameModel)
    {
        Specifications = gameModel.Specifications;
        UpdatersEngine = gameModel.UpdatersEngine;
        FixedUpdatersEngine = gameModel.FixedUpdatersEngine;
        LateUpdatersEngine = gameModel.LateUpdatersEngine;
        LoadObjectsModel = gameModel.LoadObjectsModel;
        LoadScenesModel = gameModel.LoadScenesModel;
        SceneManagementModel = gameModel.SceneManagementModel;
        SaveModel = gameModel.SaveModel;
        InputModel = gameModel.InputModel;
        ShipModel = gameModel.ShipModel;
    }
}