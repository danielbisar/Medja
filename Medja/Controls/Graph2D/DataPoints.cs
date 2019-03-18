
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

            // todo how to handle the first point correctly
            var aggregatedPoint = new Point();
            var aggregatedPointsCount = 1;
            var aggregationStartX = _points[i].X;
            var curX = aggregationStartX;
            
            // maybe we miss one point here...
            do
            {
                if (_points[i].Y >= yMin && _points[i].Y <= yMax)
                {
                    if (curX - aggregationStartX > xMinDist)
                    {
                        aggregatedPoint.X = aggregatedPoint.X / aggregatedPointsCount;
                        aggregatedPoint.Y = aggregatedPoint.Y / aggregatedPointsCount;

                        result.Add(aggregatedPoint);

                        aggregatedPoint = new Point(curX, _points[i].Y);
                        aggregatedPointsCount = 1;
                        aggregationStartX = curX;
                    }
                    else
                    {
                        aggregatedPointsCount++;
                        aggregatedPoint.X += curX; // todo overflow handling?
                        aggregatedPoint.Y += _points[i].Y;
                    }
                }

                i++;

                if (i < length)
                    curX = _points[i].X;
                else
                    break;
                
            } while (curX <= xMax);

            // we also might miss some points here
            
            return result;
        }
    }
}