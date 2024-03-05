using System.Collections.Generic;
using System.Linq;
using Biome;
using UnityEngine;

namespace Space.Preview
{
    public class SpaceMapNode
    {
        private Rect? _boundingRectangle;
        public SpaceMapNodeHalfEdge StartEdge;
        public Vector3 CenterPoint;

        public BiomeType BiomeType = BiomeType.Void;
        public string BiomeId = string.Empty;

        public IEnumerable<SpaceMapNodeHalfEdge> GetEdges()
        {
            yield return StartEdge;

            var next = StartEdge.Next;
            while (next != StartEdge)
            {
                yield return next;
                next = next.Next;
            }
        }

        private IEnumerable<SpaceMapPoint> GetCorners()
        {
            yield return StartEdge.Destination;

            var next = StartEdge.Next;
            while (next != StartEdge)
            {
                yield return next.Destination;
                next = next.Next;
            }
        }

        public bool IsEdge()
        {
            foreach (var edge in GetEdges())
            {
                if (edge.Opposite == null) return true;
            }

            return false;
        }

        public List<SpaceMapNode> GetNeighborNodes()
        {
            return GetEdges().Where(x => x.Opposite != null && x.Opposite.Node != null).Select(x => x.Opposite.Node)
                .ToList();
        }

        public Rect GetBoundingRectangle()
        {
            if (!_boundingRectangle.HasValue)
            {
                var minX = float.MaxValue;
                var maxX = float.MinValue;
                var minY = float.MaxValue;
                var maxY = float.MinValue;

                foreach (var corner in GetCorners())
                {
                    if (corner.Position.x < minX) minX = corner.Position.x;
                    if (corner.Position.x > maxX) maxX = corner.Position.x;
                    if (corner.Position.z < minY) minY = corner.Position.z;
                    if (corner.Position.z > maxY) maxY = corner.Position.z;
                }

                _boundingRectangle = new Rect(minX, minY, maxX - minX, maxY - minY);
            }

            return _boundingRectangle.Value;
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {CenterPoint}";
        }
    }
}