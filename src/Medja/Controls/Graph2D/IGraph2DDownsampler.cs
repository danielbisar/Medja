using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls.Graph2D
{
    public interface IGraph2DDownsampler
    {
        IReadOnlyList<Point> Downsample(float xMin, float xMax, float minDistance, float mThreshold);
    }
}
