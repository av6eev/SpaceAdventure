using Loader.Object;
using Loader.Scene;
using Loaders.Addressable;
using Loaders.Addressable.Scene;
using Presenter;
using Save;
using SceneManagement;
using Specification.Startup;
using Specifications;
using UnityEngine;
using Updater;

public class Startup : MonoBehaviour
{
    [SerializeField] private StartupSpecification _startupSpecification;
    
    private readonly PresentersList _presenters = new();
    private readonly UpdatersList _updatersList = new();
    private readonly UpdatersList _fixedUpdatersList = new();
    private readonly UpdatersList _lateUpdatersList = new();
    private GameModel _gameModel;
    
    private async void Start()
    {
        var loadObjectsModel = new LoadObjectsModel(new AddressableObjectLoadWrapper());
        var specifications = new GameSpecifications(_startupSpecification, loadObjectsModel);

        _gameModel = new GameModel
        {
            UpdatersEngine = _updatersList,
            FixedUpdatersEngine = _fixedUpdatersList,
            LateUpdatersEngine = _lateUpdatersList,
            LoadObjectsModel = loadObjectsModel,
            Specifications = specifications,
            SceneManagementModel = new SceneManagementModel(_startupSpecification.StartSceneSpecificationId),
            SaveModel = new SaveModel()
        };
            
        _gameModel.LoadScenesModel = new LoadScenesModel(new AddressableSceneLoadWrapper(_gameModel));
        
        await specifications.LoadAwaiter;
        
        _presenters.Add(new SceneManagementPresenter(_gameModel, (SceneManagementModel)_gameModel.SceneManagementModel));
        _presenters.Add(new SavePresenter(_gameModel, (SaveModel)_gameModel.SaveModel));
        _presenters.Init();
    }
    
    private void Update()
    {
        _updatersList.Update(Time.deltaTime);
    }
    
    private void FixedUpdate()
    {
        _fixedUpdatersList.Update(Time.fixedDeltaTime);
    }
    
    private void LateUpdate()
    {
        _lateUpdatersList.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {
        _presenters.Dispose();
        _presenters.Clear();

        _updatersList.Clear();
        _fixedUpdatersList.Clear();
    }
}