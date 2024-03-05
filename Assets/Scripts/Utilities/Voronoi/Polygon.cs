using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class Polygon
    {
        private readonly List<Vector2> _vertices;

        public Polygon(List<Vector2> vertices)
        {
            _vertices = vertices;
        }

        public Winding Winding()
        {
            var signedDoubleArea = SignedDoubleArea();

            return signedDoubleArea switch
            {
                < 0 => Utilities.Voronoi.Winding.CLOCKWISE,
                > 0 => Utilities.Voronoi.Winding.COUNTERCLOCKWISE,
                _ => Utilities.Voronoi.Winding.NONE
            };
        }

        private float SignedDoubleArea()
        {
            int index, nextIndex;
            var n = _vertices.Count;
            Vector2 point, next;
            float signedDoubleArea = 0; 
            
            for (index = 0; index < n; ++index)
            {
                nextIndex = (index + 1) % n;
                point = _vertices[index];
                next = _vertices[nextIndex];
                signedDoubleArea += point.x * next.y - next.x * point.y;
            }

            return signedDoubleArea;
        }
    }
}