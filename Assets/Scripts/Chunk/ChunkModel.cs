using System.Collections.Generic;
using Entities;
using ReactiveField;
using UnityEngine;

namespace Chunk
{
    public class ChunkModel : IChunkModel
    {
        public readonly string Id;
        public Vector2 Position { get; }
        public ReactiveField<ChunkRenderingState> RenderingState { get; } = new(ChunkRenderingState.Uncertain);
        public bool IsTargetInside;

        public readonly List<Entity> Elements = new();
        public readonly Dictionary<Entity, Vector3> ElementPositions = new();
        
        public ChunkModel(string id, Vector2 position)
        {
            Id = id;
            Position = position;
        }

        public void AddElement(Entity entity)
        {
            ElementPositions[entity] = entity.Position.Value;

            if (Elements.Contains(entity)) return;
            
            Elements.Add(entity);
        }
        
        public void RemoveElement(Entity entity)
        {
            if (!Elements.Contains(entity)) return;
            
            Elements.Remove(entity);
        }
    }
}