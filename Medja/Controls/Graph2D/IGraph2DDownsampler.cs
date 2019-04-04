using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
    public interface IGraph2DDownsampler
    {
        List<Point> Downsample(float xMin, float xMax, float minDistance, float mThreshold);
    }
}