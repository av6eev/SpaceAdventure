using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Voronoi
{
    public sealed class SiteList : IDisposable
    {
        private List<Site> _sites = new();
        private int _currentIndex;

        private bool _sorted;
        public int Count => _sites.Count;

        public void Dispose()
        {
            if (_sites == null)
            {
                return;
            }
            
            for (var i = 0; i < _sites.Count; i++)
            {
                var site = _sites[i];
                site.Dispose();
            }

            _sites.Clear();
            _sites = null;
        }

        public void Add(Site site)
        {
            _sorted = false;
            _sites.Add(site);
        }

        public void ResetSiteIndex()
        {
            _currentIndex = 0;
        }

        public Site Next()
        {
            if (_sorted == false)
            {
                Debug.LogError("SiteList::next():  sites have not been sorted");
            }

            return _currentIndex < _sites.Count ? _sites[_currentIndex++] : null;
        }

        public Rect GetSitesBounds()
        {
            float ymin;
            float ymax;
            var xmin = float.MaxValue;
            var xmax = float.MinValue;
            
            if (_sorted == false)
            {
                Site.SortSites(_sites);
                _currentIndex = 0;
                _sorted = true;
            }

            
            if (_sites.Count == 0)
            {
                return new Rect(0, 0, 0, 0);
            }

            
            for (var i = 0; i < _sites.Count; i++)
            {
                var site = _sites[i];
                if (site.X < xmin)
                {
                    xmin = site.X;
                }

                if (site.X > xmax)
                {
                    xmax = site.X;
                }
            }

            ymin = _sites[0].Y;
            ymax = _sites[^1].Y;

            return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
        }

        public List<Vector2> SiteCoords()
        {
            var coords = new List<Vector2>();
            
            for (var i = 0; i < _sites.Count; i++)
            {
                var site = _sites[i];
                coords.Add(site.Coordinate);
            }

            return coords;
        }

        public Vector2? NearestSitePoint(float x, float y)
        {
            var point = new Vector2(x, y);
            Vector2? foundSite = null;
            
            foreach (var site in _sites)
            {
                if (foundSite == null || Vector2.Distance(foundSite.Value, point) > Vector2.Distance(site.Coordinate, point))
                {
                    foundSite = site.Coordinate;
                }
            }

            return foundSite;
        }
    }
}