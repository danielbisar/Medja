
using System;
using System.Collections.Generic;
using System.Linq;
using Medja.Primitives;
using Medja.Utils;

namespace Medja.Controls
{
    public class DataPoints
    {
        private readonly List<Point> _points;
        private Point _last;
        
        public IGraph2DDownsampler Downsampler { get; }

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
    }
}