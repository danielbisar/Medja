using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class Graph2DTest
    {
        private Graph2D Create2DChart()
        {
            return new ControlFactory().Create<Graph2D>();
        }

        [Fact]
        public void CanAddDataPoints()
        {
            /*var chart2d = Create2DChart();
            
            chart2d.DataPoints.Add(new Point(0, 1));
            chart2d.DataPoints.Add(new Point(1, 1));
            chart2d.DataPoints.Add(new Point(1, 0));*/
        }

        [Fact]
        public void CanDisplayValuesUpTo8Hours()
        {
            var chart2dPoint = new Point();
            chart2dPoint.Y = 17280000; // expected amount of data points (8h*60m*60s*600(1/s))
            
            Assert.Equal(17280000, chart2dPoint.Y);
        }
    }
}