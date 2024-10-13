using System.Collections.Generic;
using System.Linq;
using CameraView.Ship;
using UnityEngine;
using Updater;
using Random = UnityEngine.Random;

namespace Chunk.Collection
{
    public class ChunkCollectionUpdater : IUpdater
    {
        private readonly ChunkCollection _chunkCollection;
        private readonly IShipCameraView _shipCameraView;

        private ChunkModel _activeChunk;
        private readonly List<Vector2> _toCreateChunkPositions = new();
        private readonly float _renderDistance;
        private float _timeToDestroy;
        private bool _isActiveChunkChange;

        public ChunkCollectionUpdater(ChunkCollection chunkCollection, IShipCameraView shipCameraView)
        {
            _chunkCollection = chunkCollection;
            _shipCameraView = shipCameraView;
            _renderDistance = GameConst.ForwardDrawDistance;
        }
        
        public void Update(float deltaTime)
        {
            // GenerateAdditionalChunksUpdate();
            RenderingUpdate();
            // RemoveFarChunksUpdate(deltaTime);
        }

        private void GenerateAdditionalChunksUpdate()
        {
            if (!_isActiveChunkChange) return;
            
            // foreach (var position in _toCreateChunkPositions)
            // {
            //     _chunkCollection.Add(position);
            // }
            
            _toCreateChunkPositions.Clear();
            
            List<ChunkModel> chunksList = new(_chunkCollection.Chunks.Values);

            var activeChunk = chunksList.Find(chunk => chunk.IsTargetInside);
            
            if (activeChunk == null)
            {
                return;
            }

            var activeChunkPosition = activeChunk.Position;
            var fromX = activeChunkPosition.x - _renderDistance;
            var toX = activeChunkPosition.x + _renderDistance;
            var fromZ = activeChunkPosition.y - _renderDistance;
            var toZ = activeChunkPosition.y + _renderDistance;
            var xChunksCount = (int)(Mathf.Abs(fromX) + toX) / (int)ChunkCollection.ChunkSize.x;
            var zChunksCount = (int)(Mathf.Abs(fromZ) + toZ) / (int)ChunkCollection.ChunkSize.z;
            var startPosition = new Vector2(fromX, fromZ);

            for (var i = 0; i < xChunksCount; i++)
            {
                for (var j = 0; j < zChunksCount; j++)
                {
                    var newPosition = new Vector2(startPosition.x + ChunkCollection.ChunkSize.x * i, startPosition.y + ChunkCollection.ChunkSize.z * j);
                    
                    if (chunksList.Find(chunk => chunk.Position == newPosition) != null)
                    {
                        continue;
                    }
                    
                    _toCreateChunkPositions.Add(newPosition);
                }    
            }

            _isActiveChunkChange = false;
        }

        private void RemoveFarChunksUpdate(float deltaTime)
        {
            if (_timeToDestroy >= _chunkCollection.DestroyRate)
            {
                List<ChunkModel> chunksList = new(_chunkCollection.Chunks.Values.Where(chunk => chunk.RenderingState.Value == ChunkRenderingState.Disabled));
                
                if (chunksList.Count == 0)
                {
                    return;
                }
                
                _chunkCollection.RemoveChunk(chunksList[Random.Range(0, chunksList.Count)].Id);
                _timeToDestroy = 0;
            }
            else
            {
                _timeToDestroy += deltaTime;
            }
        }

        private void RenderingUpdate()
        {
            foreach (var chunk in _chunkCollection.Chunks.Values)        
            {
                if (CheckTargetInsideChunk(chunk))
                {
                    if (_activeChunk == chunk)
                    {
                        _isActiveChunkChange = false;
                        break;                       
                    }

                    _activeChunk = chunk;
                    chunk.IsTargetInside = true;

                    _isActiveChunkChange = true;
                    continue;
                }

                chunk.IsTargetInside = false;
            }

            if (_activeChunk == null)
            {
                return;
            }
            
            foreach (var chunk in _chunkCollection.Chunks.Values)            
            {
                if (chunk.Position.x <= _activeChunk.Position.x + _renderDistance &&
                    chunk.Position.y <= _activeChunk.Position.y + _renderDistance &&
                    chunk.Position.x >= _activeChunk.Position.x - _renderDistance &&
                    chunk.Position.y >= _activeChunk.Position.y - _renderDistance)
                {
                    chunk.RenderingState.Value = CheckChunkBehindTarget(chunk) ? ChunkRenderingState.Prepared : ChunkRenderingState.Enabled;
                }
                else
                {
                    chunk.RenderingState.Value = ChunkRenderingState.Disabled;
                }
            }
        }

        private bool CheckTargetInsideChunk(ChunkModel chunk)
        {
            var targetPosition = _shipCameraView.Target.Position;
            var chunkPosition = chunk.Position;
            
            return targetPosition.x >= chunkPosition.x && 
                   targetPosition.x <= chunkPosition.x + ChunkCollection.ChunkSize.x &&
                   targetPosition.z >= chunkPosition.y && 
                   targetPosition.z <= chunkPosition.y + ChunkCollection.ChunkSize.z;
        }

        private bool CheckChunkBehindTarget(ChunkModel chunk)
        {
            var target = _shipCameraView.Target;
            var forward = target.TransformDirection(Vector3.forward);
            var toOther = new Vector3(chunk.Position.x, target.Position.y, chunk.Position.y) - target.Position;
            var dot = Vector3.Dot(forward, toOther);

            return dot < 0;
        }
    }
}