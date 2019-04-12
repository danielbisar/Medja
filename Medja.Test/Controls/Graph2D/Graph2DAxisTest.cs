using Medja.Controls;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class Graph2DAxisTest
    {
        [Fact]
        public void CreatesViaControlFactory()
        {
            var axis = new ControlFactory().Create<Graph2DAxis>();
            Assert.NotNull(axis);
        }
    }
}