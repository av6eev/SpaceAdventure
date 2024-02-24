using UnityEngine;

namespace Chunk
{
    public class ChunkView : MonoBehaviour, IChunkView
    {
        private Material _material;
        
        private void OnEnable() => _material = GetComponent<MeshRenderer>().material;
        public void SetName(string goName) => transform.name = goName;
        public void Enable() => _material.color = Color.green;
        public void Disable() => _material.color = Color.red;
        public void Prepare() => _material.color = Color.yellow;
        public void Dispose() => Destroy(gameObject);
        public void SetPosition(Vector2 position) => transform.position = new Vector3(position.x, 0, position.y);
    }
}