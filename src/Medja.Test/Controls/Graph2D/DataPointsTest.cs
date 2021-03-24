using System;
using Medja.Controls.Graph2D;
using Medja.Primitives;
using Xunit;

namespace Medja.Test.Controls.Graph2D
{
    public class DataPointsTest
    {
        // what do i need
        // a list of points that are sorted based on their x value
        // should be fast to access for rendering (get the parts that should be rendered, this range changes on zoom etc)
        // allow a fast way of getting only the points relevant for rendering (two points at 1,1 can be reduced to one;
        // points too close to each other in general)

        [Fact]
        public void CanAdd()
        {
            var items = new DataPoints();
            items.Add(new Point(1,2));

            // todo collection assert?
        }

        [Fact]
        public void CannotAddUnsorted()
        {
            var items = new DataPoints();
            items.Add(new Point(2, 1));

            Assert.Throws<NotSupportedException>(() => items.Add(new Point(1, 1)));
        }

        [Fact]
        public void CanAddWithSameX()
        {
            var items = new DataPoints();
            items.Add(new Point(0, 1));
            items.Add(new Point(0, 1));
        }

        [Fact]
        public void GetPointsInLimits()
        {
            var items = new DataPoints();

            items.Add(new Point(0, 1));
            items.Add(new Point(1, 1));
            items.Add(new Point(2, 0));
            items.Add(new Point(3, 2));
            items.Add(new Point(4, 1.5f));

            var settings = new DataPointsRenderingSettings();
            settings.MinX = 1;
            settings.MaxX = 3;
            settings.MinY = 1;
            settings.MaxY = 2;

            /*var result = items.GetForRendering(settings);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].X);
            Assert.Equal(3, result[1].X);*/
        }
    }
}
