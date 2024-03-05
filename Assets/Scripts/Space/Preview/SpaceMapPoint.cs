using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Space.Preview
{
    public class SpaceMapPoint
    {
        public Vector3 Position;
        public SpaceMapNodeHalfEdge LeavingEdge;
        
        public IEnumerable<SpaceMapNodeHalfEdge> GetEdges()
        {
            var firstEdge = LeavingEdge;
            var nextEdge = firstEdge;
            var maxIterations = 20;
            var iterations = 0;

            do
            {
                yield return nextEdge;
                nextEdge = nextEdge.Opposite?.Next;
                iterations++;
            } while (nextEdge != firstEdge && nextEdge != null && iterations < maxIterations);
        }

        private List<SpaceMapNode> GetNodes()
        {
            return GetEdges().Select(x => x.Node).ToList();
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {Position}";
        }
    }
}