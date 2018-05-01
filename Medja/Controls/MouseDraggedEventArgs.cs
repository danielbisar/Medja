using System;
using Medja.Primitives;

namespace Medja.Controls
{
    public class MouseDraggedEventArgs : EventArgs
    {
        private Lazy<Vector2D> _vector;
        public Vector2D Vector { get { return _vector.Value; } }
        public Point Source { get; }
        public Point Target { get; }

        public MouseDraggedEventArgs(Point source, Point target)
        {
            Source = source;
            Target = target;
            _vector = new Lazy<Vector2D>(() => new Vector2D(Source, Target));
        }
    }
}
