using System;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class EdgeList : IDisposable
    {
        private readonly float _deltaX;
        private readonly float _xMin;

        private readonly int _hashSize;
        
        private HalfEdge[] _hash;
        public HalfEdge LeftEnd { get; private set; }
        public HalfEdge RightEnd { get; private set; }

        public void Dispose()
        {
            var halfEdge = LeftEnd;
            HalfEdge prevHe;
            
            while (halfEdge != RightEnd)
            {
                prevHe = halfEdge;
                halfEdge = halfEdge.EdgeListRightNeighbor;
                prevHe.Dispose();
            }

            LeftEnd = null;
            RightEnd.Dispose();
            RightEnd = null;

            for (var i = 0; i < _hashSize; ++i)
            {
                _hash[i] = null;
            }

            _hash = null;
        }

        public EdgeList(float xMin, float deltaX, int sqrtNSites)
        {
            _xMin = xMin;
            _deltaX = deltaX;
            _hashSize = 2 * sqrtNSites;

            _hash = new HalfEdge[_hashSize];

            LeftEnd = HalfEdge.CreateDummy();
            RightEnd = HalfEdge.CreateDummy();
            LeftEnd.EdgeListLeftNeighbor = null;
            LeftEnd.EdgeListRightNeighbor = RightEnd;
            RightEnd.EdgeListLeftNeighbor = LeftEnd;
            RightEnd.EdgeListRightNeighbor = null;
            _hash[0] = LeftEnd;
            _hash[_hashSize - 1] = RightEnd;
        }

        public void Insert(HalfEdge lb, HalfEdge newHalfEdge)
        {
            newHalfEdge.EdgeListLeftNeighbor = lb;
            newHalfEdge.EdgeListRightNeighbor = lb.EdgeListRightNeighbor;
            lb.EdgeListRightNeighbor.EdgeListLeftNeighbor = newHalfEdge;
            lb.EdgeListRightNeighbor = newHalfEdge;
        }

        public void Remove(HalfEdge halfEdge)
        {
            halfEdge.EdgeListLeftNeighbor.EdgeListRightNeighbor = halfEdge.EdgeListRightNeighbor;
            halfEdge.EdgeListRightNeighbor.EdgeListLeftNeighbor = halfEdge.EdgeListLeftNeighbor;
            halfEdge.Edge = Edge.Deleted;
            halfEdge.EdgeListLeftNeighbor = halfEdge.EdgeListRightNeighbor = null;
        }

        public HalfEdge EdgeListLeftNeighbor(Vector2 p)
        {
            int i, bucket;
            HalfEdge halfEdge;

            bucket = (int)((p.x - _xMin) / _deltaX * _hashSize);
            if (bucket < 0)
            {
                bucket = 0;
            }

            if (bucket >= _hashSize)
            {
                bucket = _hashSize - 1;
            }

            halfEdge = GetHash(bucket);
            if (halfEdge == null)
            {
                for (i = 1; true; ++i)
                {
                    if ((halfEdge = GetHash(bucket - i)) != null)
                        break;
                    if ((halfEdge = GetHash(bucket + i)) != null)
                        break;
                }
            }

            if (halfEdge == LeftEnd || (halfEdge != RightEnd && halfEdge.IsLeftOf(p)))
            {
                do
                {
                    halfEdge = halfEdge.EdgeListRightNeighbor;
                } while (halfEdge != RightEnd && halfEdge.IsLeftOf(p));

                halfEdge = halfEdge.EdgeListLeftNeighbor;
            }
            else
            {
                do
                {
                    halfEdge = halfEdge.EdgeListLeftNeighbor;
                } while (halfEdge != LeftEnd && !halfEdge.IsLeftOf(p));
            }

            if (bucket > 0 && bucket < _hashSize - 1)
            {
                _hash[bucket] = halfEdge;
            }

            return halfEdge;
        }

        private HalfEdge GetHash(int b)
        {
            HalfEdge halfEdge;

            if (b < 0 || b >= _hashSize)
            {
                return null;
            }

            halfEdge = _hash[b];
            if (halfEdge != null && halfEdge.Edge == Edge.Deleted)
            {
                _hash[b] = null;
                return null;
            }
            else
            {
                return halfEdge;
            }
        }
    }
}