using System;
using System.Collections.Generic;
using System.Linq;
using Biome;
using UnityEngine;
using Utilities.Voronoi;

namespace Space.Preview
{
    public class SpaceMapGraph
    {
       public readonly List<SpaceMapNode> VoidNodes = new();
        public readonly Dictionary<string, List<SpaceMapNode>> MeteorCircleNodes = new();
        public readonly Dictionary<string, List<SpaceMapNode>> InnerMeteorCircleNodes = new();

        public Rect PlotBounds;
        public Dictionary<Vector3, SpaceMapPoint> Points;
        public Dictionary<Vector3, SpaceMapNode> NodesByCenterPosition;
        public List<SpaceMapNodeHalfEdge> Edges;

        public SpaceMapGraph(Voronoi voronoi, float snapDistance)
        {
            CreateFromVoronoi(voronoi);

            if (snapDistance > 0)
            {
                SnapPoints(snapDistance);
            }
        }

        public void AddNodeToList(string id, SpaceMapNode node)
        {
            switch (node.BiomeType)
            {
                case BiomeType.Void:
                    VoidNodes.Add(node);
                    break;
                case BiomeType.MeteorCircle:
                    if (MeteorCircleNodes.ContainsKey(id))
                    {
                        MeteorCircleNodes[id].Add(node);
                    }
                    else
                    {
                        MeteorCircleNodes.Add(id, new List<SpaceMapNode> {node});
                    }
                    break;
                case BiomeType.InnerMeteorCircle:
                    if (InnerMeteorCircleNodes.ContainsKey(id))
                    {
                        InnerMeteorCircleNodes[id].Add(node);
                    }
                    else
                    {
                        InnerMeteorCircleNodes.Add(id, new List<SpaceMapNode> {node});
                    }
                    break;
            }
        }

        public bool CheckIfNodesInAreaVoid(Vector3 startPoint, Vector3 endPoint, out List<SpaceMapNode> resultNodes)
        {
            resultNodes = new List<SpaceMapNode>();

            foreach (var (nodePosition, node) in NodesByCenterPosition)
            {
                if (nodePosition.x >= startPoint.x && nodePosition.x <= endPoint.x &&
                    nodePosition.z >= startPoint.z && nodePosition.z <= endPoint.z)
                {
                    if (node.BiomeType == BiomeType.Void)
                    {
                        resultNodes.Add(node);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void SnapPoints(float snapDistance)
        {
            var keys = Points.Keys.ToList();

            foreach (var key in keys)
            {
                if (Points.TryGetValue(key, out var point))
                {
                    var neighbors = point.GetEdges();
                    foreach (var neighbor in neighbors)
                    {
                        if (snapDistance > Vector3.Distance(point.Position, neighbor.Destination.Position))
                        {
                            SnapPoints(point, neighbor);
                        }
                    }
                }
            }
        }

        private void SnapPoints(SpaceMapPoint point, SpaceMapNodeHalfEdge edge)
        {
            if (edge.Node.GetEdges().Count() <= 3 || edge.Opposite == null ||
                edge.Opposite.Node.GetEdges().Count() <= 3) return;

            if (point.GetEdges().Any(x => x.Opposite == null) ||
                edge.Destination.GetEdges().Any(x => x.Opposite == null)) return;

            Edges.Remove(edge);
            Points.Remove(new Vector3(edge.Destination.Position.x, 0, edge.Destination.Position.z));

            var otherEdges = edge.Destination.GetEdges().ToList();

            if (point.LeavingEdge == edge) point.LeavingEdge = edge.Opposite.Next;
            if (edge.Node.StartEdge == edge) edge.Node.StartEdge = edge.Previous;
            edge.Next.Previous = edge.Previous;
            edge.Previous.Next = edge.Next;

            if (edge.Opposite != null && edge.Opposite.Node.GetEdges().Count() > 3)
            {
                Edges.Remove(edge.Opposite);

                edge.Opposite.Next.Previous = edge.Opposite.Previous;
                edge.Opposite.Previous.Next = edge.Opposite.Next;
                if (edge.Opposite.Node.StartEdge == edge.Opposite)
                    edge.Opposite.Node.StartEdge = edge.Opposite.Previous;
            }

            foreach (var otherEdge in otherEdges)
            {
                if (otherEdge.Opposite != null) otherEdge.Opposite.Destination = point;
            }
        }

        public Vector3 GetCenter()
        {
            return ToVector3(PlotBounds.center);
        }

        private static void AddLeavingEdge(SpaceMapNodeHalfEdge edge)
        {
            if (edge.Previous.Destination.LeavingEdge == null)
            {
                edge.Previous.Destination.LeavingEdge = edge;
            }
        }

        private List<LineSegment> GetBoundariesForSite(Dictionary<Vector2, List<LineSegment>> siteEdges, Vector2 site)
        {
            var boundaries = siteEdges[site];

            boundaries = FlipClockwise(boundaries, site);
            boundaries = SortClockwise(boundaries, site);
            boundaries = SnapBoundaries(boundaries, 0.001f);

            return boundaries;
        }

        private static List<LineSegment> SnapBoundaries(List<LineSegment> boundries, float snapDistance)
        {
            for (int i = boundries.Count - 1; i >= 0; i--)
            {
                if (Vector2.Distance(boundries[i].P0.Value, boundries[i].P1.Value) < snapDistance)
                {
                    var previous = i - 1;
                    var next = i + 1;
                    if (previous < 0) previous = boundries.Count - 1;
                    if (next >= boundries.Count) next = 0;

                    if (Vector2.Distance(boundries[previous].P1.Value, boundries[next].P0.Value) < snapDistance)
                    {
                        boundries[previous].P1 = boundries[next].P0;
                    }

                    boundries.Remove(boundries[i]);
                }
            }

            return boundries;
        }

        public SpaceMapNode GetClosestNode(float x, float y, out float offset)
        {
            var position = new Vector3(x, 0, y);
            SpaceMapNode closestNode = null;
            float lowestDistanceSqr = int.MaxValue;
            
            foreach (var node in NodesByCenterPosition.Values)
            {
                var direction = node.CenterPoint - position;
                var distanceSqr = direction.sqrMagnitude;

                if (distanceSqr < lowestDistanceSqr)
                {
                    closestNode = node;
                    lowestDistanceSqr = distanceSqr;
                }
            }

            offset = (float)Math.Sqrt(lowestDistanceSqr);
            return closestNode;
        }

        private void ConnectOpposites(Dictionary<Vector3, List<SpaceMapNodeHalfEdge>> edgesByStartPosition)
        {
            foreach (var edge in Edges)
            {
                if (edge.Opposite == null)
                {
                    var startEdgePosition = edge.Previous.Destination.Position;
                    var endEdgePosition = edge.Destination.Position;

                    if (edgesByStartPosition.TryGetValue(endEdgePosition, out var list))
                    {
                        SpaceMapNodeHalfEdge opposite = null;

                        foreach (var item in list)
                        {
                            if (Math.Abs(item.Destination.Position.x - startEdgePosition.x) < 0.5f &&
                                Math.Abs(item.Destination.Position.z - startEdgePosition.z) < 0.5f)
                            {
                                opposite = item;
                            }
                        }

                        if (opposite != null)
                        {
                            edge.Opposite = opposite;
                            opposite.Opposite = edge;
                        }
                        else
                        {
                            var isAtEdge = endEdgePosition.x == 0 || endEdgePosition.x == PlotBounds.width ||
                                           endEdgePosition.z == 0 || endEdgePosition.z == PlotBounds.height ||
                                           startEdgePosition.x == 0 || startEdgePosition.x == PlotBounds.width ||
                                           startEdgePosition.z == 0 || startEdgePosition.z == PlotBounds.height;

                            if (!isAtEdge)
                            {
                                edge.Node.BiomeType = BiomeType.Void;
                                Debug.Assert(isAtEdge, "Edges without opposites must be at the boundary edge");
                            }
                        }
                    }
                }
            }
        }

        private List<LineSegment> SortClockwise(List<LineSegment> segments, Vector2 center)
        {
            segments.Sort((line1, line2) =>
            {
                var firstVector = line1.P0.Value - center;
                var secondVector = line2.P0.Value - center;
                var angle = Vector2.SignedAngle(firstVector, secondVector);

                return angle > 0 ? 1 : (angle < 0 ? -1 : 0);
            });
            return segments;
        }

        private List<LineSegment> FlipClockwise(List<LineSegment> segments, Vector2 center)
        {
            var newSegments = new List<LineSegment>();
            foreach (var line in segments)
            {
                var firstVector = line.P0.Value - center;
                var secondVector = line.P1.Value - center;
                var angle = Vector2.SignedAngle(firstVector, secondVector);

                if (angle > 0) newSegments.Add(new LineSegment(line.P1, line.P0));
                else newSegments.Add(new LineSegment(line.P0, line.P1));
            }

            return newSegments;
        }

        private SpaceMapNodeHalfEdge AddEdge(Dictionary<Vector3, List<SpaceMapNodeHalfEdge>> edgesByStartPosition,
            SpaceMapNodeHalfEdge previous, Vector3 start, Vector3 end, SpaceMapNode node)
        {
            if (start == end)
            {
                Debug.Assert(start != end, "Start and end vectors must not be the same");
            }

            var currentEdge = new SpaceMapNodeHalfEdge { Node = node };

            if (!Points.ContainsKey(start))
                Points.Add(start, new SpaceMapPoint { Position = start, LeavingEdge = currentEdge });
            if (!Points.ContainsKey(end)) Points.Add(end, new SpaceMapPoint { Position = end });

            currentEdge.Destination = Points[end];

            if (!edgesByStartPosition.ContainsKey(start))
                edgesByStartPosition.Add(start, new List<SpaceMapNodeHalfEdge>());
            edgesByStartPosition[start].Add(currentEdge);
            Edges.Add(currentEdge);

            if (previous != null)
            {
                previous.Next = currentEdge;
                currentEdge.Previous = previous;
                AddLeavingEdge(currentEdge);
            }

            return currentEdge;
        }

        private Vector3 ToVector3(Vector2 vector)
        {
            return new Vector3(Mathf.Round(vector.x * 1000f) / 1000f, 0, Mathf.Round(vector.y * 1000f) / 1000f);
        }

        private void CreateFromVoronoi(Voronoi voronoi)
        {
            Points = new Dictionary<Vector3, SpaceMapPoint>();
            NodesByCenterPosition = new Dictionary<Vector3, SpaceMapNode>();
            var edgesByStartPosition = new Dictionary<Vector3, List<SpaceMapNodeHalfEdge>>();
            Edges = new List<SpaceMapNodeHalfEdge>();

            PlotBounds = voronoi.PlotBounds;
            var bottomLeftSite = voronoi.NearestSitePoint(voronoi.PlotBounds.xMin, voronoi.PlotBounds.yMin);
            var bottomRightSite = voronoi.NearestSitePoint(voronoi.PlotBounds.xMax, voronoi.PlotBounds.yMin);
            var topLeftSite = voronoi.NearestSitePoint(voronoi.PlotBounds.xMin, voronoi.PlotBounds.yMax);
            var topRightSite = voronoi.NearestSitePoint(voronoi.PlotBounds.xMax, voronoi.PlotBounds.yMax);

            var topLeft = new Vector3(voronoi.PlotBounds.xMin, 0, voronoi.PlotBounds.yMax);
            var topRight = new Vector3(voronoi.PlotBounds.xMax, 0, voronoi.PlotBounds.yMax);
            var bottomLeft = new Vector3(voronoi.PlotBounds.xMin, 0, voronoi.PlotBounds.yMin);
            var bottomRight = new Vector3(voronoi.PlotBounds.xMax, 0, voronoi.PlotBounds.yMin);

            var siteEdges = new Dictionary<Vector2, List<LineSegment>>();

            var edgePointsRemoved = 0;

            foreach (var edge in voronoi.Edges())
            {
                if (edge.Visible)
                {
                    var p1 = edge.ClippedEnds[Side.Left];
                    var p2 = edge.ClippedEnds[Side.Right];
                    var segment = new LineSegment(p1, p2);

                    if (Vector2.Distance(p1.Value, p2.Value) < 0.001f)
                    {
                        edgePointsRemoved++;
                        continue;
                    }

                    if (edge.LeftSite != null)
                    {
                        if (!siteEdges.ContainsKey(edge.LeftSite.Coordinate))
                            siteEdges.Add(edge.LeftSite.Coordinate, new List<LineSegment>());
                        siteEdges[edge.LeftSite.Coordinate].Add(segment);
                    }

                    if (edge.RightSite != null)
                    {
                        if (!siteEdges.ContainsKey(edge.RightSite.Coordinate))
                            siteEdges.Add(edge.RightSite.Coordinate, new List<LineSegment>());
                        siteEdges[edge.RightSite.Coordinate].Add(segment);
                    }
                }
            }

            Debug.Assert(edgePointsRemoved == 0, $"{edgePointsRemoved} edge points too close and have been removed");

            foreach (var site in voronoi.SiteCoords())
            {
                var boundries = GetBoundariesForSite(siteEdges, site);
                var center = ToVector3(site);
                var currentNode = new SpaceMapNode { CenterPoint = center };

                NodesByCenterPosition.Add(center, currentNode);

                SpaceMapNodeHalfEdge firstEdge = null;
                SpaceMapNodeHalfEdge previousEdge = null;

                for (var i = 0; i < boundries.Count; i++)
                {
                    var edge = boundries[i];

                    var start = ToVector3(edge.P0.Value);
                    var end = ToVector3(edge.P1.Value);
                    if (start == end) continue;

                    previousEdge = AddEdge(edgesByStartPosition, previousEdge, start, end, currentNode);
                    if (firstEdge == null) firstEdge = previousEdge;
                    if (currentNode.StartEdge == null) currentNode.StartEdge = previousEdge;

                    var insertEdges = false;
                    if (i < boundries.Count - 1)
                    {
                        start = ToVector3(boundries[i].P1.Value);
                        end = ToVector3(boundries[i + 1].P0.Value);
                        insertEdges = start != end;
                    }
                    else if (i == boundries.Count - 1)
                    {
                        start = ToVector3(boundries[i].P1.Value);
                        end = ToVector3(boundries[0].P0.Value);
                        insertEdges = start != end;
                    }

                    if (insertEdges)
                    {
                        var startIsTop = start.z == voronoi.PlotBounds.yMax;
                        var startIsBottom = start.z == voronoi.PlotBounds.yMin;
                        var startIsLeft = start.x == voronoi.PlotBounds.xMin;
                        var startIsRight = start.x == voronoi.PlotBounds.xMax;

                        var hasTopLeft = site == topLeftSite && !(startIsTop && startIsLeft);
                        var hasTopRight = site == topRightSite && !(startIsTop && startIsRight);
                        var hasBottomLeft = site == bottomLeftSite && !(startIsBottom && startIsLeft);
                        var hasBottomRight = site == bottomRightSite && !(startIsBottom && startIsRight);

                        if (startIsTop)
                        {
                            if (hasTopRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge, start, topRight,
                                    currentNode);
                            if (hasBottomRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, bottomRight, currentNode);
                            if (hasBottomLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, bottomLeft, currentNode);
                            if (hasTopLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, topLeft, currentNode);
                        }
                        else if (startIsRight)
                        {
                            if (hasBottomRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge, start, bottomRight,
                                    currentNode);
                            if (hasBottomLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, bottomLeft, currentNode);
                            if (hasTopLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, topLeft, currentNode);
                            if (hasTopRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, topRight, currentNode);
                        }
                        else if (startIsBottom)
                        {
                            if (hasBottomLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge, start, bottomLeft,
                                    currentNode);
                            if (hasTopLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, topLeft, currentNode);
                            if (hasTopRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, topRight, currentNode);
                            if (hasBottomRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, bottomRight, currentNode);
                        }
                        else if (startIsLeft)
                        {
                            if (hasTopLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge, start, topLeft, currentNode);
                            if (hasTopRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, topRight, currentNode);
                            if (hasBottomRight)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, bottomRight, currentNode);
                            if (hasBottomLeft)
                                previousEdge = AddEdge(edgesByStartPosition, previousEdge,
                                    previousEdge.Destination.Position, bottomLeft, currentNode);
                        }

                        previousEdge = AddEdge(edgesByStartPosition, previousEdge, previousEdge.Destination.Position,
                            end, currentNode);
                    }
                }

                previousEdge.Next = firstEdge;
                firstEdge.Previous = previousEdge;
                AddLeavingEdge(firstEdge);
            }

            ConnectOpposites(edgesByStartPosition);
        }
    }
}