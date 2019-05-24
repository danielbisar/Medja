using System;
using Medja.Primitives;

namespace Medja.Controls
{
    public class MouseDraggedEventArgs : EventArgs
    {
        private readonly Lazy<Vector2D> _vector;
        public Vector2D Vector { get { return _vector.Value; } }
        
        /// <summary>
        /// The starting point of the drag operation.
        /// </summary>
        public Point Source { get; }
        
        /// <summary>
        /// The current point of the drag operation.
        /// </summary>
        public Point Target { get; }

        public MouseDraggedEventArgs(Point source, Point target)
        {
            Source = source;
            Target = target;
            _vector = new Lazy<Vector2D>(() => new Vector2D(Source, Target));
        }
    }
}
