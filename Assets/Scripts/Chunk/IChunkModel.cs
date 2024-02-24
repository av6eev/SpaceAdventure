using Entities;
using ReactiveField;
using UnityEngine;

namespace Chunk
{
    public interface IChunkModel
    {
        Vector2 Position { get; }
        ReactiveField<ChunkRenderingState> RenderingState { get; }

        public void AddElement(Entity entity);
        public void RemoveElement(Entity entity);
    }
}