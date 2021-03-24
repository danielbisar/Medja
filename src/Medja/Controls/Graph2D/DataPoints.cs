using System;
using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls.Graph2D
{
    /// <summary>
    /// Contains a list of ordered points. These points must be ordered according to their X-value.
    /// </summary>
    public class DataPoints
    {
        // The last added point, is used to compare with new point see Add(...)
        private Point _last;

        private readonly List<Point> _points;
        /// <summary>
        /// The ordered points (raw, not down sampled)
        /// </summary>
        public IReadOnlyList<Point> Points
        {
            get { return _points;}
        }

        /// <summary>
        /// Gets the count of points in the list.
        /// </summary>
        public int Count
        {
            get { return _points.Count; }
        }

        public DataPoints()
        {
            _points = new List<Point>();
        }

        /// <summary>
        /// Adds a point to the list.
        /// </summary>
        /// <param name="p">The point to add.</param>
        /// <exception cref="NotSupportedException">If the X value of <see cref="p"/> is smaller the previous added one.
        /// </exception>
        public void Add(Point p)
        {
            if (_last != null && _last.X > p.X)
                throw new NotSupportedException("Points must be added in correct order (x must be <= the previous value).");

            _points.Add(p);
            _last = p;
        }

        /// <summary>
        /// Adds multiple points. The points must be sorted by ascending X values.
        /// </summary>
        /// <param name="points">The points to add.</param>
        public void AddRange(IEnumerable<Point> points)
        {
            foreach(var point in points)
                Add(point);
        }

        public void Clear()
        {
            _points.Clear();
            _last = null;
        }

//        /// <summary>
//        /// Get all points relevant for rendering with the given settings.
//        /// </summary>
//        /// <param name="settings">The rendering settings.</param>
//        /// <returns>The points relevant for rendering.</returns>
//        public virtual List<Point> GetForRendering(DataPointsRenderingSettings settings)
//        {
//            // very naive implementation
//            var result = _points.Where(p => p.X >= settings.MinX
//                                      && p.X <= settings.MaxX
//                                      && p.Y >= settings.MinY
//                                      && p.Y <= settings.MaxY).ToList();
//            
//            if(result.Distinct(p => p.X).Count() > settings.WidthPixels)
//                // reduce 
//        }
    }
}
