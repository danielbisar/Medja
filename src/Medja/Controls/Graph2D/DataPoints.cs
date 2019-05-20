
using System;
using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
    public class DataPoints
    {
        private Point _last;

        private readonly List<Point> _points;
        public IReadOnlyList<Point> Points
        {
            get { return _points;}
        }
        
        public IGraph2DDownsampler Downsampler { get; set; }

        public int Count
        {
            get { return _points.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataPoints()
        {
            _points = new List<Point>();
            Downsampler = new Graph2DSlopDownsampler(_points);
        }

        public void Add(Point p)
        {
            if (_last == null || _last.X <= p.X)
                _points.Add(p);
            else
                throw new NotSupportedException("Points must be added in correct order (x must <= the previous value)");

            _last = p;
        }

        public void Clear()
        {
            _points.Clear();
            _points.TrimExcess();
            _last = null;
        }
    }
}