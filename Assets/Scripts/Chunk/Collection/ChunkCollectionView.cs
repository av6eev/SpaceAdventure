using UnityEngine;

namespace Chunk.Collection
{
    public class ChunkCollectionView : MonoBehaviour, IChunkCollectionView
    {
        public IChunkView InstantiateChunkView(Vector2 position, string goName)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var chunkView = go.AddComponent<ChunkView>();
            
            go.transform.SetParent(transform);
            go.transform.localScale = new Vector3(20f, 20f, 20f);

            chunkView.SetPosition(position);
            chunkView.SetName(goName);
            
            return chunkView;
        }
    }
}