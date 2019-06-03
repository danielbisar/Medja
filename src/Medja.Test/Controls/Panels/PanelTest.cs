using Medja.Controls;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class PanelTest
    {
        [Fact]
        public void DisposesChildrenOnDefault()
        {
            var controlFactory = new ControlFactory();
            var panel = new Panel();

            var child1 = controlFactory.Create<Control>();
            
            panel.Add(child1);
            
            panel.Dispose();
            
            Assert.True(panel.IsDisposed);
            Assert.True(child1.IsDisposed);
        }
        
        [Fact]
        public void DoesNotDisposesChildrenIfSet()
        {
            var controlFactory = new ControlFactory();
            var panel = new Panel();

            panel.DisposeChildren = false;
            
            var child1 = controlFactory.Create<Control>();
            
            panel.Add(child1);
            
            panel.Dispose();
            
            Assert.True(panel.IsDisposed);
            Assert.False(child1.IsDisposed);
        }

        [Fact]
        public void DisposeChildrenOnClear()
        {
            var controlFactory = new ControlFactory();
            var panel = new Panel();

            var child1 = controlFactory.Create<Control>();
            
            panel.Add(child1);
            panel.Clear();
            
            Assert.True(child1.IsDisposed);
        }
    }
}