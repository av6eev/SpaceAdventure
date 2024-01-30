using System;
using Awaiter;
using Input;
using Input.Windows;
using Presenter;
using UnityEngine;

namespace Session.Setup
{
    public class SessionSetupInputPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly PresentersList _presenters;
        private readonly ISessionSceneView _view;

        public readonly CustomAwaiter LoadAwaiter = new();

        public SessionSetupInputPresenter(SessionLocationGameModel gameModel, PresentersList presenters, ISessionSceneView view)
        {
            _gameModel = gameModel;
            _presenters = presenters;
            _view = view;
        }
        
        public async void Init()
        {
            IInputModel inputModel = null;
            BaseInputPresenter presenter = null;
            
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    inputModel = new WindowsInputModel();
                    
                    var windowsInputGo = _gameModel.LoadObjectsModel.Load<GameObject>("windows_input_view");
                    await windowsInputGo.LoadAwaiter;
            
                    presenter = new WindowsInputPresenter(_gameModel, (WindowsInputModel)inputModel, windowsInputGo.Result.GetComponent<IWindowsInputView>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{Application.platform} is not defined correctly!");
            }

            presenter.Init();
            
            _gameModel.InputModel = inputModel;
            _presenters.Add(presenter);
            
            LoadAwaiter.Complete();
        }

        public void Dispose()
        {
            LoadAwaiter.Dispose();
        }
    }
}