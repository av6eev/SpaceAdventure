using UnityEngine;

namespace Utilities.Voronoi
{
    public static class BoundsCheck
    {
        public static readonly int Top = 1;
        public static readonly int Bottom = 2;
        public static readonly int Left = 4;
        public static readonly int Right = 8;

        public static int Check(Vector2 point, Rect bounds)
        {
            var value = 0;
            
            if (point.x == bounds.xMin)
            {
                value |= Left;
            }

            if (point.x == bounds.xMax)
            {
                value |= Right;
            }

            if (point.y == bounds.yMin)
            {
                value |= Top;
            }

            if (point.y == bounds.yMax)
            {
                value |= Bottom;
            }

            return value;
        }
    }
}