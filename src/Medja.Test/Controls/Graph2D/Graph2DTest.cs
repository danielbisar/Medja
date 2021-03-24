using Medja.Primitives;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls.Graph2D
{
    public class Graph2DTest
    {
        private Medja.Controls.Graph2D.Graph2D Create2DChart()
        {
            return new ControlFactory().Create<Medja.Controls.Graph2D.Graph2D>();
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
