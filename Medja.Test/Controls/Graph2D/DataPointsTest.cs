using System;
using Medja.Controls;
using Medja.Primitives;
using Xunit;

namespace Medja.Test.Controls
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

            var result = items.Downsampler.Downsample(1, 3, 0, 1);
            
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].X);
            Assert.Equal(2, result[1].X);
        }
    }
}