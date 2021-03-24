using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls.Graph2D
{
    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <remarks>Author: Daniel Klafft</remarks>
    public class DummyDownsampler : IGraph2DDownsampler
    {
        private readonly IReadOnlyList<Point> _points;

        public DummyDownsampler(IReadOnlyList<Point> points)
        {
            _points = points;
        }

        public IReadOnlyList<Point> Downsample(float xMin, float xMax, float minDistance, float mThreshold)
        {
            return _points;
        }
    }
}
