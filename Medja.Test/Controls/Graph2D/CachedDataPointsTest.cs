using Medja.Controls;
using Xunit;

namespace Medja.Test.Controls
{
    public class CachedDataPointsTest
    {
        [Fact]
        public void CanInvalidateCache()
        {
            var points = new CachedDataPoints();
            
            points.InvalidateCache();
        }
    }
}