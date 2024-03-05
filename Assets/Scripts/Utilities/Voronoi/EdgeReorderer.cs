using System;
using System.Collections.Generic;

namespace Utilities.Voronoi
{
    public sealed class EdgeReorderer : IDisposable
    {
        public List<Edge> Edges { get; private set; }

        public List<Side> EdgeOrientations { get; private set; }

        public EdgeReorderer(List<Edge> origEdges, VertexOrSite criterion)
        {
            Edges = new List<Edge>();
            EdgeOrientations = new List<Side>();
            
            if (origEdges.Count > 0)
            {
                Edges = ReorderEdges(origEdges, criterion);
            }
        }

        public void Dispose()
        {
            Edges = null;
            EdgeOrientations = null;
        }

        private List<Edge> ReorderEdges(List<Edge> origEdges, VertexOrSite criterion)
        {
            var n = origEdges.Count;
            var done = new bool[n];
            var nDone = 0;
            
            for (var j = 0; j < n; j++)
            {
                done[j] = false;
            }

            var newEdges = new List<Edge>();
            var i = 0;
            var edge = origEdges[i];
            
            newEdges.Add(edge);
            EdgeOrientations.Add(Side.Left);
            
            var firstPoint = (criterion == VertexOrSite.Vertex) ? (ICoordinate)edge.LeftVertex : (ICoordinate)edge.LeftSite;
            var lastPoint = (criterion == VertexOrSite.Vertex) ? (ICoordinate)edge.RightVertex : (ICoordinate)edge.RightSite;

            if (firstPoint == Vertex.VertexAtInfinity || lastPoint == Vertex.VertexAtInfinity)
            {
                return new List<Edge>();
            }

            done[i] = true;
            ++nDone;

            while (nDone < n)
            {
                for (i = 1; i < n; ++i)
                {
                    if (done[i])
                    {
                        continue;
                    }

                    edge = origEdges[i];
                    var leftPoint = (criterion == VertexOrSite.Vertex)
                        ? (ICoordinate)edge.LeftVertex
                        : (ICoordinate)edge.LeftSite;
                    var rightPoint = (criterion == VertexOrSite.Vertex)
                        ? (ICoordinate)edge.RightVertex
                        : (ICoordinate)edge.RightSite;
                    if (leftPoint == Vertex.VertexAtInfinity || rightPoint == Vertex.VertexAtInfinity)
                    {
                        return new List<Edge>();
                    }

                    if (leftPoint == lastPoint)
                    {
                        lastPoint = rightPoint;
                        EdgeOrientations.Add(Side.Left);
                        newEdges.Add(edge);
                        done[i] = true;
                    }
                    else if (rightPoint == firstPoint)
                    {
                        firstPoint = leftPoint;
                        EdgeOrientations.Insert(0, Side.Left);
                        newEdges.Insert(0, edge);
                        done[i] = true;
                    }
                    else if (leftPoint == firstPoint)
                    {
                        firstPoint = rightPoint;
                        EdgeOrientations.Insert(0, Side.Right);
                        newEdges.Insert(0, edge);
                        done[i] = true;
                    }
                    else if (rightPoint == lastPoint)
                    {
                        lastPoint = leftPoint;
                        EdgeOrientations.Add(Side.Right);
                        newEdges.Add(edge);
                        done[i] = true;
                    }

                    if (done[i])
                    {
                        ++nDone;
                    }
                }
            }

            return newEdges;
        }
    }
}