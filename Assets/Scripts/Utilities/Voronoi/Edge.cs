using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class Edge
    {
        private static readonly Stack<Edge> _pool = new();
        private static int _nEdges;
        private readonly int _edgeIndex;
        public static readonly Edge Deleted = new();
        public float A, B, C;

        public Vertex LeftVertex { get; private set; }
        public Vertex RightVertex { get; private set; }
        
        public Dictionary<Side, Vector2?> ClippedEnds { get; private set; }

        public bool Visible => ClippedEnds != null;

        private Dictionary<Side, Site> _sites;

        public Site LeftSite
        {
            get => _sites[Side.Left];
            set => _sites[Side.Left] = value;
        }

        public Site RightSite
        {
            get => _sites[Side.Right];
            set => _sites[Side.Right] = value;
        }
        
        private Edge()
        {
            _edgeIndex = _nEdges++;
            Init();
        }
        
        public static Edge CreateBisectingEdge(Site site0, Site site1)
        {
            float dx, dy, absdx, absdy;
            float a, b, c;

            dx = site1.X - site0.X;
            dy = site1.Y - site0.Y;
            absdx = dx > 0 ? dx : -dx;
            absdy = dy > 0 ? dy : -dy;
            c = site0.X * dx + site0.Y * dy + (dx * dx + dy * dy) * 0.5f;
            if (absdx > absdy)
            {
                a = 1.0f;
                b = dy / dx;
                c /= dx;
            }
            else
            {
                b = 1.0f;
                a = dx / dy;
                c /= dy;
            }

            var edge = Create();

            edge.LeftSite = site0;
            edge.RightSite = site1;
            site0.AddEdge(edge);
            site1.AddEdge(edge);

            edge.LeftVertex = null;
            edge.RightVertex = null;

            edge.A = a;
            edge.B = b;
            edge.C = c;

            return edge;
        }

        private static Edge Create()
        {
            Edge edge;
            if (_pool.Count > 0)
            {
                edge = _pool.Pop();
                edge.Init();
            }
            else
            {
                edge = new Edge();
            }

            return edge;
        }

        public LineSegment DelaunayLine()
        {
            return new LineSegment(LeftSite.Coordinate, RightSite.Coordinate);
        }

        public void SetVertex(Side leftRight, Vertex v)
        {
            if (leftRight == Side.Left)
            {
                LeftVertex = v;
            }
            else
            {
                RightVertex = v;
            }
        }

        public Site Site(Side leftRight)
        {
            return _sites[leftRight];
        }

        public void Dispose()
        {
            LeftVertex = null;
            RightVertex = null;
            if (ClippedEnds != null)
            {
                ClippedEnds[Side.Left] = null;
                ClippedEnds[Side.Right] = null;
                ClippedEnds = null;
            }

            _sites[Side.Left] = null;
            _sites[Side.Right] = null;
            _sites = null;

            _pool.Push(this);
        }

        private void Init()
        {
            _sites = new Dictionary<Side, Site>();
        }

        public override string ToString()
        {
            return "Edge " + _edgeIndex + "; sites " + _sites[Side.Left] + ", " +
                   _sites[Side.Right]
                   + "; endVertices " + ((LeftVertex != null) ? LeftVertex.VertexIndex.ToString() : "null") + ", "
                   + ((RightVertex != null) ? RightVertex.VertexIndex.ToString() : "null") + "::";
        }

        public void ClipVertices(Rect bounds)
        {
            var xMin = bounds.xMin;
            var yMin = bounds.yMin;
            var xMax = bounds.xMax;
            var yMax = bounds.yMax;

            Vertex vertex0, vertex1;
            float x0, x1, y0, y1;

            if (A == 1.0 && B >= 0.0)
            {
                vertex0 = RightVertex;
                vertex1 = LeftVertex;
            }
            else
            {
                vertex0 = LeftVertex;
                vertex1 = RightVertex;
            }

            if (A == 1.0)
            {
                y0 = yMin;
                if (vertex0 != null && vertex0.Y > yMin)
                {
                    y0 = vertex0.Y;
                }

                if (y0 > yMax)
                {
                    return;
                }

                x0 = C - B * y0;

                y1 = yMax;
                if (vertex1 != null && vertex1.Y < yMax)
                {
                    y1 = vertex1.Y;
                }

                if (y1 < yMin)
                {
                    return;
                }

                x1 = C - B * y1;

                if ((x0 > xMax && x1 > xMax) || (x0 < xMin && x1 < xMin))
                {
                    return;
                }

                if (x0 > xMax)
                {
                    x0 = xMax;
                    y0 = (C - x0) / B;
                }
                else if (x0 < xMin)
                {
                    x0 = xMin;
                    y0 = (C - x0) / B;
                }

                if (x1 > xMax)
                {
                    x1 = xMax;
                    y1 = (C - x1) / B;
                }
                else if (x1 < xMin)
                {
                    x1 = xMin;
                    y1 = (C - x1) / B;
                }
            }
            else
            {
                x0 = xMin;
                if (vertex0 != null && vertex0.X > xMin)
                {
                    x0 = vertex0.X;
                }

                if (x0 > xMax)
                {
                    return;
                }

                y0 = C - A * x0;

                x1 = xMax;
                if (vertex1 != null && vertex1.X < xMax)
                {
                    x1 = vertex1.X;
                }

                if (x1 < xMin)
                {
                    return;
                }

                y1 = C - A * x1;

                if ((y0 > yMax && y1 > yMax) || (y0 < yMin && y1 < yMin))
                {
                    return;
                }

                if (y0 > yMax)
                {
                    y0 = yMax;
                    x0 = (C - y0) / A;
                }
                else if (y0 < yMin)
                {
                    y0 = yMin;
                    x0 = (C - y0) / A;
                }

                if (y1 > yMax)
                {
                    y1 = yMax;
                    x1 = (C - y1) / A;
                }
                else if (y1 < yMin)
                {
                    y1 = yMin;
                    x1 = (C - y1) / A;
                }
            }

            ClippedEnds = new Dictionary<Side, Vector2?>();
            
            if (vertex0 == LeftVertex)
            {
                ClippedEnds[Side.Left] = new Vector2(x0, y0);
                ClippedEnds[Side.Right] = new Vector2(x1, y1);
            }
            else
            {
                ClippedEnds[Side.Right] = new Vector2(x0, y0);
                ClippedEnds[Side.Left] = new Vector2(x1, y1);
            }
        }
    }
}