using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class LineSegment
    {
        public Vector2? P0;
        public Vector2? P1;

        public LineSegment(Vector2? p0, Vector2? p1)
        {
            P0 = p0;
            P1 = p1;
        }
        
        public static int CompareLengths_MAX(LineSegment segment0, LineSegment segment1)
        {
            var length0 = Vector2.Distance((Vector2)segment0.P0, (Vector2)segment0.P1);
            var length1 = Vector2.Distance((Vector2)segment1.P0, (Vector2)segment1.P1);
            
            if (length0 < length1)
            {
                return 1;
            }

            if (length0 > length1)
            {
                return -1;
            }

            return 0;
        }

        public static int CompareLengths(LineSegment edge0, LineSegment edge1)
        {
            return -CompareLengths_MAX(edge0, edge1);
        }
    }
}