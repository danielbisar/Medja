using System;
using System.Collections.Generic;
using Medja.Primitives;
using Medja.Utils;

namespace Medja.Controls.Graph2D
{
    /// <summary>
    /// Down samples ordered (x-Axis ascending) points based on the relative slope between these points.
    /// </summary>
    /// <remarks>Author: Daniel Klafft</remarks> 
    public class Graph2DSlopDownsampler : IGraph2DDownsampler
    {
        private readonly IReadOnlyList<Point> _points;
        private List<Point> _result;
        private int _currentPointIndex;

        private bool HasMore
        {
            get { return _currentPointIndex + 1 < _points.Count; }
        }
        
        public Graph2DSlopDownsampler(IReadOnlyList<Point> points)
        {
            _points = points;
        }
        
        /// <summary>
        /// Down sample the internal list of points and returns the result. The internal list is not modified.
        /// </summary>
        /// <param name="xMin">The minimum value for x, other values will be discarded.</param>
        /// <param name="xMax">The maximum value for x, other values will be discarded.</param>
        /// <param name="minDistance">The minimum distance between too points so that they will be taken into account.
        /// This has over the mThreshold.</param>
        /// <param name="mThreshold">The min/max allowed change of m (see remarks) between points to be recognized.
        /// </param>
        /// <returns>The down sampled list of points.</returns>
        public IReadOnlyList<Point> Downsample(float xMin, float xMax, float minDistance, float mThreshold)
        {
            // use Return() to return the list
            _result = new List<Point>();
            _currentPointIndex = 0;

            if (_points.Count == 0)
                return Return();

            SkipPointsSmaller(xMin);

            // hint: _currentPointIndex is modified by methods called inside the loop
            for (; HasMore; _currentPointIndex++)
            {
                var startPoint = _points[_currentPointIndex];
                var startI = _currentPointIndex;

                if (startPoint.X > xMax)
                    return Return();
                
                _result.Add(startPoint);

                // ignore points that are too close together; do this step before calculating the slope
                // because points that are further away from each other should have slopes more similar to each other
                SkipPointsTooClose(startPoint, minDistance, xMax);

                // no point was close but we need the next point
                if (_currentPointIndex == startI)
                    _currentPointIndex++;

                if (HasMore)
                {
                    var currentPoint = _points[_currentPointIndex];
                    var currentPointI = _currentPointIndex;
                    var m1 = GetSlope(startPoint, currentPoint);

                    SkipPointsTooClose(currentPoint, minDistance, xMax);

                    if (HasMore)
                    {
                        if (_currentPointIndex == currentPointI)
                            _currentPointIndex++;

                        var m2 = GetSlope(startPoint, _points[_currentPointIndex]);

                        while (HasMore && Math.Abs(m1 - m2) < mThreshold)
                        {
                            currentPoint = _points[_currentPointIndex];
                            currentPointI = _currentPointIndex;

                            SkipPointsTooClose(currentPoint, minDistance, xMax);

                            if (HasMore)
                            {
                                if (_currentPointIndex == currentPointI)
                                    _currentPointIndex++;

                                m2 = GetSlope(startPoint, _points[_currentPointIndex]);
                            }
                        }
                    }

                    _result.Add(currentPoint);
                }
            }

            return Return();
        }

        private void SkipPointsSmaller(float xMin)
        {
            while (_currentPointIndex < _points.Count && _points[_currentPointIndex].X < xMin)
            {
                _currentPointIndex++;
            }
        }
        
        private void SkipPointsTooClose(Point currentPoint, float minDistance, float xMax)
        {
            for (; _currentPointIndex < _points.Count; _currentPointIndex++)
            {
                if (_points[_currentPointIndex].X > xMax)
                    return;
                
                if (currentPoint.Distance(_points[_currentPointIndex]) >= minDistance)
                    return;
            }
        }

        private float GetSlope(Point p1, Point p2)
        {
            var dy = p2.Y - p1.Y;
            var dx = p2.X - p1.X;

            if (dx == 0)
                return 0;
            
            return dy / dx;
        }

        /// <summary>
        /// When returning we must take care that we do not keep a reference to list. That's what this function does.
        /// </summary>
        /// <returns>_result</returns>
        private List<Point> Return()
        {
            var result = _result;
            return result;
        }
    }
}
