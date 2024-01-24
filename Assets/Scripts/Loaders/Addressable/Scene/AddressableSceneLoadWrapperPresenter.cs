﻿using LocationBuilder;
using Presenter;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Loaders.Addressable.Scene
{
    public class AddressableSceneLoadWrapperPresenter : IPresenter
    {
        private readonly GameModel _environment;
        private readonly AddressableSceneLoadWrapperModel _model;
        
        private readonly PresentersList _presenters = new();
        
        public AddressableSceneLoadWrapperPresenter(GameModel environment, AddressableSceneLoadWrapperModel model)
        {
            _environment = environment;
            _model = model;
        }
        
        public void Init()
        {
            _model.LoadAsyncOperation = Addressables.LoadSceneAsync(_model.LoadObjectToWrapperModel.Specification.SceneId, LoadSceneMode.Additive);
            _model.LoadAsyncOperation.Completed += OnCompleted;
        }

        public void Dispose()
        {
            _model.LoadAsyncOperation.Completed -= OnCompleted;
            
            _presenters.Dispose();
            _presenters.Clear();
        }

        private void OnCompleted(AsyncOperationHandle<SceneInstance> op)
        {
            new LocationLogicBuilder(_environment, _presenters, _model.LoadObjectToWrapperModel.Specification).Build();
            
            _presenters.Init();

            _model.LoadObjectToWrapperModel.CompleteLoad();
        }
    }
}