using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Voronoi
{
    public static class VoronoiHelper
    {
        public static List<Vector2> GetVector2Points(int seed, int number, int max)
        {
            var points = new List<Vector2>();
            
            Random.InitState(seed);

            for (var i = 0; i < number; i++)
            {
                points.Add(new Vector2(Random.Range(0, max), Random.Range(0, max)));
            }
            
            return points;
        }
    }
}
