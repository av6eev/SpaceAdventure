using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class Vertex : ICoordinate
    {
        public static readonly Vertex VertexAtInfinity = new(float.NaN, float.NaN);
        private static readonly Stack<Vertex> _pool = new();

        private static int _nVertices;

        private Vector2 _coordinate;
        public Vector2 Coordinate => _coordinate;

        public int VertexIndex { get; private set; }

        public float X => _coordinate.x;
        public float Y => _coordinate.y;

        public Vertex(float x, float y)
        {
            Init(x, y);
        }

        private static Vertex Create(float x, float y)
        {
            if (float.IsNaN(x) || float.IsNaN(y))
            {
                return VertexAtInfinity;
            }

            if (_pool.Count > 0)
            {
                return _pool.Pop().Init(x, y);
            }
            else
            {
                return new Vertex(x, y);
            }
        }

        private Vertex Init(float x, float y)
        {
            _coordinate = new Vector2(x, y);
            return this;
        }

        public void Dispose()
        {
            _pool.Push(this);
        }

        public void SetIndex()
        {
            VertexIndex = _nVertices++;
        }

        public override string ToString()
        {
            return "Vertex (" + VertexIndex + ")";
        }

        public static Vertex Intersect(HalfEdge halfedge0, HalfEdge halfedge1)
        {
            Edge edge0, edge1, edge;
            HalfEdge halfEdge;
            float determinant, intersectionX, intersectionY;
            bool rightOfSite;

            edge0 = halfedge0.Edge;
            edge1 = halfedge1.Edge;
            if (edge0 == null || edge1 == null)
            {
                return null;
            }

            if (edge0.RightSite == edge1.RightSite)
            {
                return null;
            }

            determinant = edge0.A * edge1.B - edge0.B * edge1.A;
            
            if (-1.0e-10 < determinant && determinant < 1.0e-10)
            {
                return null;
            }

            intersectionX = (edge0.C * edge1.B - edge1.C * edge0.B) / determinant;
            intersectionY = (edge1.C * edge0.A - edge0.C * edge1.A) / determinant;

            if (Voronoi.CompareByYThenX(edge0.RightSite, edge1.RightSite) < 0)
            {
                halfEdge = halfedge0;
                edge = edge0;
            }
            else
            {
                halfEdge = halfedge1;
                edge = edge1;
            }

            rightOfSite = intersectionX >= edge.RightSite.X;
            if ((rightOfSite && halfEdge.LeftRight == Side.Left) || (!rightOfSite && halfEdge.LeftRight == Side.Right))
            {
                return null;
            }

            return Create(intersectionX, intersectionY);
        }
    }
}