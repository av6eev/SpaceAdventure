using Presenter;
using Specification.Startup;
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
    
    private void Start()
    {
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