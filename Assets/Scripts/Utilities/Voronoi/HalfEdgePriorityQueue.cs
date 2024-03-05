using System;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class HalfEdgePriorityQueue : IDisposable
    {
        private HalfEdge[] _hash;
        
        private int _count;
        private int _minBucket;
        private readonly int _hashSize;
        
        private readonly float _yMin;
        private readonly float _deltaY;

        public HalfEdgePriorityQueue(float yMin, float deltaY, int sqrtNSites)
        {
            _yMin = yMin;
            _deltaY = deltaY;
            _hashSize = 4 * sqrtNSites;
            
            Initialize();
        }

        public void Dispose()
        {
            for (var i = 0; i < _hashSize; ++i)
            {
                _hash[i].Dispose();
                _hash[i] = null;
            }

            _hash = null;
        }

        private void Initialize()
        {
            _count = 0;
            _minBucket = 0;
            _hash = new HalfEdge[_hashSize];
            
            for (var i = 0; i < _hashSize; ++i)
            {
                _hash[i] = HalfEdge.CreateDummy();
                _hash[i].NextInPriorityQueue = null;
            }
        }

        public void Insert(HalfEdge halfEdge)
        {
            HalfEdge previous, next;
            var insertionBucket = Bucket(halfEdge);
            
            if (insertionBucket < _minBucket)
            {
                _minBucket = insertionBucket;
            }

            previous = _hash[insertionBucket];
            
            while ((next = previous.NextInPriorityQueue) != null && (halfEdge.YStar > next.YStar || (halfEdge.YStar == next.YStar && halfEdge.Vertex.X > next.Vertex.X)))
            {
                previous = next;
            }

            halfEdge.NextInPriorityQueue = previous.NextInPriorityQueue;
            previous.NextInPriorityQueue = halfEdge;
            ++_count;
        }

        public void Remove(HalfEdge halfEdge)
        {
            HalfEdge previous;
            var removalBucket = Bucket(halfEdge);

            if (halfEdge.Vertex != null)
            {
                previous = _hash[removalBucket];
                
                while (previous.NextInPriorityQueue != halfEdge)
                {
                    previous = previous.NextInPriorityQueue;
                }

                previous.NextInPriorityQueue = halfEdge.NextInPriorityQueue;
                _count--;
                
                halfEdge.Vertex = null;
                halfEdge.NextInPriorityQueue = null;
                halfEdge.Dispose();
            }
        }

        private int Bucket(HalfEdge halfEdge)
        {
            var theBucket = (int)((halfEdge.YStar - _yMin) / _deltaY * _hashSize);
            
            if (theBucket < 0)
            {
                theBucket = 0;
            }

            if (theBucket >= _hashSize)
            {
                theBucket = _hashSize - 1;
            }
            
            return theBucket;
        }

        private bool IsEmpty(int bucket)
        {
            return _hash[bucket].NextInPriorityQueue == null;
        }
        
        private void AdjustMinBucket()
        {
            while (_minBucket < _hashSize - 1 && IsEmpty(_minBucket))
            {
                ++_minBucket;
            }
        }

        public bool Empty()
        {
            return _count == 0;
        }

        public Vector2 Min()
        {
            AdjustMinBucket();
            
            var answer = _hash[_minBucket].NextInPriorityQueue;
            return new Vector2(answer.Vertex.X, answer.YStar);
        }

        public HalfEdge ExtractMin()
        {
            var answer = _hash[_minBucket].NextInPriorityQueue;

            _hash[_minBucket].NextInPriorityQueue = answer.NextInPriorityQueue;
            _count--;
            answer.NextInPriorityQueue = null;

            return answer;
        }
    }
}