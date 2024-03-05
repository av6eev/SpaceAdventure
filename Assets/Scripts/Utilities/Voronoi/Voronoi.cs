using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * The author of this software is Steven Fortune.  Copyright (c) 1994 by AT&T
 * Bell Laboratories.
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */

namespace Utilities.Voronoi
{
    public class Voronoi : IDisposable
    {
        private SiteList _sites;
        private Dictionary<Vector2, Site> _sitesIndexedByLocation;
        private List<Edge> _edges;
        private Site _fortunesAlgorithmBottomMostSite;
        
        private Rect _plotBounds;
        public Rect PlotBounds => _plotBounds;

        public void Dispose()
        {
            int i, n;
            
            if (_sites != null)
            {
                _sites.Dispose();
                _sites = null;
            }

            if (_edges != null)
            {
                n = _edges.Count;
                
                for (i = 0; i < n; ++i)
                {
                    _edges[i].Dispose();
                }

                _edges.Clear();
                _edges = null;
            }

            _sitesIndexedByLocation = null;
        }

        public Voronoi(List<Vector2> points, Rect plotBounds, int lloydIterations)
        {
            Init(points, plotBounds);
            LloydRelaxation(lloydIterations);
        }

        private void Init(List<Vector2> points, Rect plotBounds)
        {
            _sites = new SiteList();
            _sitesIndexedByLocation = new Dictionary<Vector2, Site>();
            _plotBounds = plotBounds;
            _edges = new List<Edge>();

            AddSites(points);
            CalculateFortunesAlgorithm();
        }

        private void LloydRelaxation(int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                var newPoints = new List<Vector2>();

                _sites.ResetSiteIndex();
                var site = _sites.Next();

                while (site != null)
                {
                    var region = site.Region(PlotBounds);

                    if (region.Count < 1)
                    {
                        site = _sites.Next();
                        continue;
                    }

                    var centroid = Vector2.zero;
                    float signedArea = 0;
                    float x0;
                    float y0;
                    float x1;
                    float y1;
                    float a;

                    for (var j = 0; j < region.Count - 1; j++)
                    {
                        x0 = region[j].x;
                        y0 = region[j].y;
                        x1 = region[j + 1].x;
                        y1 = region[j + 1].y;
                        a = x0 * y1 - x1 * y0;
                        signedArea += a;
                        centroid.x += (x0 + x1) * a;
                        centroid.y += (y0 + y1) * a;
                    }

                    x0 = region[^1].x;
                    y0 = region[^1].y;
                    x1 = region[0].x;
                    y1 = region[0].y;

                    a = x0 * y1 - x1 * y0;
                    signedArea += a;
                    centroid.x += (x0 + x1) * a;
                    centroid.y += (y0 + y1) * a;

                    signedArea *= 0.5f;
                    centroid.x /= (6 * signedArea);
                    centroid.y /= (6 * signedArea);

                    newPoints.Add(centroid);
                    site = _sites.Next();
                }

                var origPlotBounds = PlotBounds;

                Dispose();
                Init(newPoints, origPlotBounds);
            }
        }

        private void AddSites(List<Vector2> points)
        {
            for (var i = 0; i < points.Count; ++i)
            {
                AddSite(points[i], i);
            }
        }

        private void AddSite(Vector2 p, int index)
        {
            if (_sitesIndexedByLocation.ContainsKey(p))
            {
                return;
            }

            var site = Site.Create(p, (uint)index);

            _sites.Add(site);
            _sitesIndexedByLocation[p] = site;
        }

        public List<Edge> Edges()
        {
            return _edges;
        }

        public Vector2? NearestSitePoint(float x, float y)
        {
            return _sites.NearestSitePoint(x, y);
        }

        public List<Vector2> SiteCoords()
        {
            return _sites.SiteCoords();
        }

        private void CalculateFortunesAlgorithm()
        {
            var newintstar = Vector2.zero;
            Site newSite, bottomSite, topSite, tempSite;
            Vertex v, vertex;
            Side leftRight;
            HalfEdge lbnd, rbnd, llbnd, rrbnd, bisector;
            Edge edge;

            var dataBounds = _sites.GetSitesBounds();

            var sqrtNSites = (int)(Mathf.Sqrt(_sites.Count + 4));
            var heap = new HalfEdgePriorityQueue(dataBounds.y, dataBounds.height, sqrtNSites);
            var edgeList = new EdgeList(dataBounds.x, dataBounds.width, sqrtNSites);
            var halfEdges = new List<HalfEdge>();
            var vertices = new List<Vertex>();

            _fortunesAlgorithmBottomMostSite = _sites.Next();
            newSite = _sites.Next();

            for (;;)
            {
                if (heap.Empty() == false)
                {
                    newintstar = heap.Min();
                }

                if (newSite != null && (heap.Empty() || CompareByYThenX(newSite, newintstar) < 0))
                {
                    lbnd = edgeList.EdgeListLeftNeighbor(newSite.Coordinate);
                    rbnd = lbnd.EdgeListRightNeighbor;
                    bottomSite = FortunesAlgorithm_rightRegion(lbnd);

                    edge = Edge.CreateBisectingEdge(bottomSite, newSite);
                    _edges.Add(edge);

                    bisector = HalfEdge.Create(edge, Side.Left);
                    halfEdges.Add(bisector);
                    edgeList.Insert(lbnd, bisector);

                    if ((vertex = Vertex.Intersect(lbnd, bisector)) != null)
                    {
                        vertices.Add(vertex);
                        heap.Remove(lbnd);
                        lbnd.Vertex = vertex;
                        lbnd.YStar = vertex.Y + newSite.Dist(vertex);
                        heap.Insert(lbnd);
                    }

                    lbnd = bisector;
                    bisector = HalfEdge.Create(edge, Side.Right);
                    halfEdges.Add(bisector);
                    edgeList.Insert(lbnd, bisector);

                    if ((vertex = Vertex.Intersect(bisector, rbnd)) != null)
                    {
                        vertices.Add(vertex);
                        bisector.Vertex = vertex;
                        bisector.YStar = vertex.Y + newSite.Dist(vertex);
                        heap.Insert(bisector);
                    }

                    newSite = _sites.Next();
                }
                else if (heap.Empty() == false)
                {
                    lbnd = heap.ExtractMin();
                    llbnd = lbnd.EdgeListLeftNeighbor;
                    rbnd = lbnd.EdgeListRightNeighbor;
                    rrbnd = rbnd.EdgeListRightNeighbor;
                    bottomSite = FortunesAlgorithm_leftRegion(lbnd);
                    topSite = FortunesAlgorithm_rightRegion(rbnd);

                    v = lbnd.Vertex;
                    v.SetIndex();
                    lbnd.Edge.SetVertex((Side)lbnd.LeftRight, v);
                    rbnd.Edge.SetVertex((Side)rbnd.LeftRight, v);
                    edgeList.Remove(lbnd);
                    heap.Remove(rbnd);
                    edgeList.Remove(rbnd);
                    leftRight = Side.Left;

                    if (bottomSite.Y > topSite.Y)
                    {
                        tempSite = bottomSite;
                        bottomSite = topSite;
                        topSite = tempSite;
                        leftRight = Side.Right;
                    }

                    edge = Edge.CreateBisectingEdge(bottomSite, topSite);
                    _edges.Add(edge);
                    bisector = HalfEdge.Create(edge, leftRight);
                    halfEdges.Add(bisector);
                    edgeList.Insert(llbnd, bisector);
                    edge.SetVertex(SideHelper.Other(leftRight), v);

                    if ((vertex = Vertex.Intersect(llbnd, bisector)) != null)
                    {
                        vertices.Add(vertex);
                        heap.Remove(llbnd);
                        llbnd.Vertex = vertex;
                        llbnd.YStar = vertex.Y + bottomSite.Dist(vertex);
                        heap.Insert(llbnd);
                    }

                    if ((vertex = Vertex.Intersect(bisector, rrbnd)) != null)
                    {
                        vertices.Add(vertex);
                        bisector.Vertex = vertex;
                        bisector.YStar = vertex.Y + bottomSite.Dist(vertex);
                        heap.Insert(bisector);
                    }
                }
                else
                {
                    break;
                }
            }

            heap.Dispose();
            edgeList.Dispose();

            for (var hIndex = 0; hIndex < halfEdges.Count; hIndex++)
            {
                var halfEdge = halfEdges[hIndex];
                halfEdge.ReallyDispose();
            }

            halfEdges.Clear();

            for (var eIndex = 0; eIndex < _edges.Count; eIndex++)
            {
                edge = _edges[eIndex];
                edge.ClipVertices(_plotBounds);
            }

            for (var vIndex = 0; vIndex < vertices.Count; vIndex++)
            {
                vertex = vertices[vIndex];
                vertex.Dispose();
            }

            vertices.Clear();
        }

        private Site FortunesAlgorithm_leftRegion(HalfEdge he)
        {
            var edge = he.Edge;

            if (edge == null)
            {
                return _fortunesAlgorithmBottomMostSite;
            }

            return edge.Site((Side)he.LeftRight);
        }

        private Site FortunesAlgorithm_rightRegion(HalfEdge he)
        {
            var edge = he.Edge;

            if (edge == null)
            {
                return _fortunesAlgorithmBottomMostSite;
            }

            return edge.Site(SideHelper.Other((Side)he.LeftRight));
        }

        public static int CompareByYThenX(Site s1, Site s2)
        {
            if (s1.Y < s2.Y)
                return -1;
            if (s1.Y > s2.Y)
                return 1;
            if (s1.X < s2.X)
                return -1;
            if (s1.X > s2.X)
                return 1;
            return 0;
        }

        private static int CompareByYThenX(Site s1, Vector2 s2)
        {
            if (s1.Y < s2.y)
                return -1;
            if (s1.Y > s2.y)
                return 1;
            if (s1.X < s2.x)
                return -1;
            if (s1.X > s2.x)
                return 1;
            return 0;
        }
    }
}