using UnityEngine;

namespace Space.Preview
{
    public class SpaceMapNodeHalfEdge
    {
        public SpaceMapNode Node;
        public SpaceMapPoint Destination;
        public SpaceMapNodeHalfEdge Next;
        public SpaceMapNodeHalfEdge Previous;
        public SpaceMapNodeHalfEdge Opposite;

        public Vector3 GetStartPosition()
        {
            return Previous.Destination.Position;
        }

        public Vector3 GetEndPosition()
        {
            return Destination.Position;
        }
        
        public override string ToString()
        {
            return "HalfEdge: " + Previous.Destination.Position + " -> " + Destination.Position;
        }
    }
}