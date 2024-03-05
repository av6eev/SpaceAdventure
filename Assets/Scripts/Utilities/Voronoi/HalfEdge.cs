using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class HalfEdge : IDisposable
    {
        private static readonly Stack<HalfEdge> _pool = new();

        public HalfEdge EdgeListLeftNeighbor;
        public HalfEdge EdgeListRightNeighbor;
        public HalfEdge NextInPriorityQueue;

        public Edge Edge;
        public Side? LeftRight;
        public Vertex Vertex;

        public float YStar;

        public HalfEdge(Edge edge = null, Side? lr = null)
        {
            Init(edge, lr);
        }

        public static HalfEdge Create(Edge edge, Side? lr)
        {
            return _pool.Count > 0 ? _pool.Pop().Init(edge, lr) : new HalfEdge(edge, lr);
        }

        public static HalfEdge CreateDummy()
        {
            return Create(null, null);
        }
        
        private HalfEdge Init(Edge edge, Side? lr)
        {
            Edge = edge;
            LeftRight = lr;
            NextInPriorityQueue = null;
            Vertex = null;
            
            return this;
        }

        public override string ToString()
        {
            return "HalfEdge (leftRight: " + LeftRight + "; vertex: " + Vertex + ")";
        }

        public void Dispose()
        {
            if (EdgeListLeftNeighbor != null || EdgeListRightNeighbor != null)
            {
                return;
            }

            if (NextInPriorityQueue != null)
            {
                return;
            }

            Edge = null;
            LeftRight = null;
            Vertex = null;
            
            _pool.Push(this);
        }

        public void ReallyDispose()
        {
            EdgeListLeftNeighbor = null;
            EdgeListRightNeighbor = null;
            NextInPriorityQueue = null;
            Edge = null;
            LeftRight = null;
            Vertex = null;
            
            _pool.Push(this);
        }

        internal bool IsLeftOf(Vector2 p)
        {
            Site topSite;
            bool rightOfSite, above, fast;
            float dxp, dyp, dxs, t1, t2, t3, yl;

            topSite = Edge.RightSite;
            rightOfSite = p.x > topSite.X;
            
            switch (rightOfSite)
            {
                case true when LeftRight == Side.Left:
                    return true;
                case false when LeftRight == Side.Right:
                    return false;
            }

            if (Edge.A == 1.0)
            {
                dyp = p.y - topSite.Y;
                dxp = p.x - topSite.X;
                fast = false;
                
                if ((!rightOfSite && Edge.B < 0.0) || (rightOfSite && Edge.B >= 0.0))
                {
                    above = dyp >= Edge.B * dxp;
                    fast = above;
                }
                else
                {
                    above = p.x + p.y * Edge.B > Edge.C;
                    if (Edge.B < 0.0)
                    {
                        above = !above;
                    }

                    if (!above)
                    {
                        fast = true;
                    }
                }

                if (!fast)
                {
                    dxs = topSite.X - Edge.LeftSite.X;
                    above = Edge.B * (dxp * dxp - dyp * dyp) <
                            dxs * dyp * (1.0 + 2.0 * dxp / dxs + Edge.B * Edge.B);
                    if (Edge.B < 0.0)
                    {
                        above = !above;
                    }
                }
            }
            else
            {
                yl = Edge.C - Edge.A * p.x;
                t1 = p.y - yl;
                t2 = p.x - topSite.X;
                t3 = yl - topSite.Y;
                above = t1 * t1 > t2 * t2 + t3 * t3;
            }

            return LeftRight == Side.Left ? above : !above;
        }
    }
}