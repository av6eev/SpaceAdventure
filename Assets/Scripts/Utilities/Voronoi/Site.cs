using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class Site : ICoordinate, IComparable
    {
        private static readonly float EPSILON = .005f;
        private static readonly Stack<Site> _pool = new();
        
        private uint _siteIndex;
        private List<Edge> _edges;
        private List<Side> _edgeOrientations;
        private List<Vector2> _region;
        
        private Vector2 _coord;
        public Vector2 Coordinate => _coord;
        
        public float X => _coord.x;
        public float Y => _coord.y;

        private Site(Vector2 p, uint index)
        {
            Init(p, index);
        }

        public static Site Create(Vector2 p, uint index)
        {
            return _pool.Count > 0 ? _pool.Pop().Init(p, index) : new Site(p, index);
        }

        public static void SortSites(List<Site> sites)
        {
            sites.Sort();
        }

        public int CompareTo(System.Object obj)
        {
            var s2 = (Site)obj;
            var returnValue = Voronoi.CompareByYThenX(this, s2);
            uint tempIndex;
            
            if (returnValue == -1)
            {
                if (_siteIndex > s2._siteIndex)
                {
                    tempIndex = _siteIndex;
                    _siteIndex = s2._siteIndex;
                    s2._siteIndex = tempIndex;
                }
            }
            else if (returnValue == 1)
            {
                if (s2._siteIndex > _siteIndex)
                {
                    tempIndex = s2._siteIndex;
                    s2._siteIndex = _siteIndex;
                    _siteIndex = tempIndex;
                }
            }

            return returnValue;
        }

        private static bool CloseEnough(Vector2 p0, Vector2 p1)
        {
            return Vector2.Distance(p0, p1) < EPSILON;
        }

        private Site Init(Vector2 p, uint index)
        {
            _coord = p;
            _siteIndex = index;
            _edges = new List<Edge>();
            _region = null;
            return this;
        }

        public override string ToString()
        {
            return "Site " + _siteIndex + ": " + Coordinate;
        }

        public void Dispose()
        {
            Clear();
            _pool.Push(this);
        }

        private void Clear()
        {
            if (_edges != null)
            {
                _edges.Clear();
                _edges = null;
            }

            if (_edgeOrientations != null)
            {
                _edgeOrientations.Clear();
                _edgeOrientations = null;
            }

            if (_region != null)
            {
                _region.Clear();
                _region = null;
            }
        }

        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);
        }

        public List<Vector2> Region(Rect clippingBounds)
        {
            if (_edges == null || _edges.Count == 0)
            {
                return new List<Vector2>();
            }

            if (_edgeOrientations == null)
            {
                ReorderEdges();
                _region = ClipToBounds(clippingBounds);
                if ((new Polygon(_region)).Winding() == Winding.CLOCKWISE)
                {
                    _region.Reverse();
                }
            }

            return _region;
        }

        private void ReorderEdges()
        {
            var reorderer = new EdgeReorderer(_edges, VertexOrSite.Vertex);
            _edges = reorderer.Edges;
            _edgeOrientations = reorderer.EdgeOrientations;
            reorderer.Dispose();
        }

        private List<Vector2> ClipToBounds(Rect bounds)
        {
            var points = new List<Vector2>();
            var n = _edges.Count;
            var i = 0;
            Edge edge;
            
            while (i < n && (_edges[i].Visible == false))
            {
                ++i;
            }

            if (i == n)
            {
                return new List<Vector2>();
            }

            edge = _edges[i];
            var orientation = _edgeOrientations[i];

            if (edge.ClippedEnds[orientation] == null)
            {
                Debug.LogError("XXX: Null detected when there should be a Vector2!");
            }

            if (edge.ClippedEnds[SideHelper.Other(orientation)] == null)
            {
                Debug.LogError("XXX: Null detected when there should be a Vector2!");
            }

            points.Add((Vector2)edge.ClippedEnds[orientation]);
            points.Add((Vector2)edge.ClippedEnds[SideHelper.Other(orientation)]);

            for (var j = i + 1; j < n; ++j)
            {
                edge = _edges[j];
                if (edge.Visible == false)
                {
                    continue;
                }

                Connect(points, j, bounds);
            }

            Connect(points, i, bounds, true);

            return points;
        }

        private void Connect(List<Vector2> points, int j, Rect bounds, bool closingUp = false)
        {
            var rightPoint = points[points.Count - 1];
            var newEdge = _edges[j] as Edge;
            var newOrientation = _edgeOrientations[j];
            if (newEdge.ClippedEnds[newOrientation] == null)
            {
                Debug.LogError("XXX: Null detected when there should be a Vector2!");
            }

            var newPoint = (Vector2)newEdge.ClippedEnds[newOrientation];
            if (!CloseEnough(rightPoint, newPoint))
            {
                if (rightPoint.x != newPoint.x
                    && rightPoint.y != newPoint.y)
                {
                    var rightCheck = BoundsCheck.Check(rightPoint, bounds);
                    var newCheck = BoundsCheck.Check(newPoint, bounds);
                    float px, py;
                    if ((rightCheck & BoundsCheck.Right) != 0)
                    {
                        px = bounds.xMax;
                        if ((newCheck & BoundsCheck.Bottom) != 0)
                        {
                            py = bounds.yMax;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Top) != 0)
                        {
                            py = bounds.yMin;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Left) != 0)
                        {
                            if (rightPoint.y - bounds.y + newPoint.y - bounds.y < bounds.height)
                            {
                                py = bounds.yMin;
                            }
                            else
                            {
                                py = bounds.yMax;
                            }

                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(bounds.xMin, py));
                        }
                    }
                    else if ((rightCheck & BoundsCheck.Left) != 0)
                    {
                        px = bounds.xMin;
                        if ((newCheck & BoundsCheck.Bottom) != 0)
                        {
                            py = bounds.yMax;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Top) != 0)
                        {
                            py = bounds.yMin;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Right) != 0)
                        {
                            if (rightPoint.y - bounds.y + newPoint.y - bounds.y < bounds.height)
                            {
                                py = bounds.yMin;
                            }
                            else
                            {
                                py = bounds.yMax;
                            }

                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(bounds.xMax, py));
                        }
                    }
                    else if ((rightCheck & BoundsCheck.Top) != 0)
                    {
                        py = bounds.yMin;
                        if ((newCheck & BoundsCheck.Right) != 0)
                        {
                            px = bounds.xMax;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Left) != 0)
                        {
                            px = bounds.xMin;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Bottom) != 0)
                        {
                            if (rightPoint.x - bounds.x + newPoint.x - bounds.x < bounds.width)
                            {
                                px = bounds.xMin;
                            }
                            else
                            {
                                px = bounds.xMax;
                            }

                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(px, bounds.yMax));
                        }
                    }
                    else if ((rightCheck & BoundsCheck.Bottom) != 0)
                    {
                        py = bounds.yMax;
                        if ((newCheck & BoundsCheck.Right) != 0)
                        {
                            px = bounds.xMax;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Left) != 0)
                        {
                            px = bounds.xMin;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & BoundsCheck.Top) != 0)
                        {
                            if (rightPoint.x - bounds.x + newPoint.x - bounds.x < bounds.width)
                            {
                                px = bounds.xMin;
                            }
                            else
                            {
                                px = bounds.xMax;
                            }

                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(px, bounds.yMin));
                        }
                    }
                }

                if (closingUp)
                {
                    return;
                }

                points.Add(newPoint);
            }

            if (newEdge.ClippedEnds[SideHelper.Other(newOrientation)] == null)
            {
                Debug.LogError("XXX: Null detected when there should be a Vector2!");
            }

            var newRightPoint = (Vector2)newEdge.ClippedEnds[SideHelper.Other(newOrientation)];
            if (!CloseEnough(points[0], newRightPoint))
            {
                points.Add(newRightPoint);
            }
        }

        public float Dist(ICoordinate p)
        {
            return Vector2.Distance(p.Coordinate, _coord);
        }
    }
}