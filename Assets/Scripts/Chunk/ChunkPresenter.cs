﻿using System;
using System.Collections.Generic;
using Biome;
using Entities;
using Entities.Asteroids.Asteroid;
using Entities.Asteroids.Collection;
using Presenter;
using Session;
using UnityEngine;

namespace Chunk
{
    public class ChunkPresenter : IPresenter
    {
        private readonly SessionLocationGameModel _gameModel;
        private readonly ChunkModel _model;
        private readonly IChunkView _view;
        
        private ChunkUpdater _chunkUpdater;

        public ChunkPresenter(SessionLocationGameModel gameModel, ChunkModel model, IChunkView view)
        {
            _gameModel = gameModel;
            _model = model;
            _view = view;
        }

        public void Init()
        {
            switch (_model.BiomeType)
            {
                case BiomeType.Uncertain:
                    break;
                case BiomeType.Void:
                    _view.Disable();
                    break;
                case BiomeType.MeteorCircle:
                    _view.Enable();
                    break;
                case BiomeType.InnerMeteorCircle:
                    _view.Prepare();
                    break;
            }    
            
            _model.RenderingState.OnChanged += ChangeChunkView;
            
            _chunkUpdater = new ChunkUpdater(_model);
            _gameModel.UpdatersEngine.Add(_chunkUpdater);
        }

        public void Dispose()
        {
            _model.RenderingState.OnChanged -= ChangeChunkView;

            _gameModel.UpdatersEngine.Remove(_chunkUpdater);

            _view.Dispose();
        }

        private void ChangeChunkView(ChunkRenderingState state)
        {
            switch (state)
            {
                case ChunkRenderingState.Uncertain:
                    break;
                case ChunkRenderingState.Enabled:
                    _view.Enable();

                    if (_gameModel.SpaceModel.AsteroidCollection == null) return;
                    
                    var positions = new Dictionary<Entity, Vector3>(_model.ElementPositions);
                    var remainCount = AsteroidCollection.MaxCount - _gameModel.SpaceModel.AsteroidCollection.Count;
                    var toCreateCount = positions.Count > remainCount ? remainCount : positions.Count;
                    
                    foreach (var element in positions)
                    {
                        if (toCreateCount == 0) break;

                        if (element.Key is AsteroidModel { IsDisabled: true, IsVisibleFromCamera: false } asteroid)
                        {
                            asteroid.Position.Value = element.Value;
                            _gameModel.SpaceModel.AsteroidCollection.CreateAsteroid(asteroid);
                            
                            toCreateCount--;
                        }
                    }
                    break;
                case ChunkRenderingState.Prepared:
                    foreach (var element in _model.Elements)
                    {
                        if (element is AsteroidModel asteroid)
                        {
                            _gameModel.SpaceModel.AsteroidCollection.DestroyAsteroid(asteroid);
                        }
                    }
                    
                    _model.Elements.Clear();
                    _view.Prepare();
                    break;
                case ChunkRenderingState.Disabled:
                    foreach (var element in _model.Elements)
                    {
                        if (element is AsteroidModel asteroid)
                        {
                            _gameModel.SpaceModel.AsteroidCollection.DestroyAsteroid(asteroid);
                        }
                    }
                    
                    _model.Elements.Clear();
                    _view.Disable();
                    break;
            }
        }
    }
}