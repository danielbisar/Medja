
using System;
using System.Collections.Generic;
using System.Linq;
using Medja.Primitives;

namespace Medja.Controls
{
    public class DataPoints
    {
        private readonly List<Point> _points;
        private Point _last;

        public DataPoints()
        {
            _points = new List<Point>();
        }
        
        public void Add(Point p)
        {
            if(_last == null || _last.X <= p.X)
                _points.Add(p);
            else
                throw new NotSupportedException("Points must be added in correct order (x must <= the previous value)");
            
            _last = p;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <param name="xMinDist">The minimal distant expected per x coordinate. If we have n points fitting in this
        /// distance they are summarized.</param>
        /// <returns></returns>
        public List<Point> GetForDrawing(float xMin, float xMax, float yMin, float yMax, float xMinDist)
        {
            var result = new List<Point>();
            
            if(_points.Count == 0)
                return result;

            var length = _points.Count;
            var i = 0;

            while (i < length && _points[i].X < xMin)
                i++;

            // todo how to handle points when aggregating that are over the limit Y-wise?

            if (i >= length)
                return result;

            for (; i < length && _points[i].X <= xMax; i++)
            {
                if (_points[i].Y >= yMin && _points[i].Y <= yMax)
                    result.Add(_points[i]);
            }

            return result;

            // todo how to handle the first point correctly
            var aggregatedPoint = new Point();
            var aggregatedPointsCount = 1;
            var aggregationStartX = _points[i].X;
            
            // maybe we miss one point here...
            for (;i < length && _points[i].X <= xMax; i++)
            {
                if (_points[i].Y >= yMin && _points[i].Y <= yMax)
                {
                    if (aggregationStartX + _points[i].X >= xMinDist)
                    {
                        aggregatedPoint.X = aggregatedPoint.X / aggregatedPointsCount;
                        aggregatedPoint.Y = aggregatedPoint.Y / aggregatedPointsCount;
                        
                        result.Add(aggregatedPoint);
                        
                        aggregatedPoint = new Point(_points[i].X, _points[i].Y);
                        aggregatedPointsCount = 1;
                        aggregationStartX = _points[i].X;
                    }
                    else
                    {
                        aggregatedPointsCount++;
                        aggregatedPoint.X += _points[i].X; // todo overflow handling?
                        aggregatedPoint.Y += _points[i].Y;
                    }
                }
            }

            return result;
        }
    }
}