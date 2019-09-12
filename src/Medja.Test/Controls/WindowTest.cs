using Medja.Controls;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class WindowTest
    {
        [Fact]
        public void SetsParentOfContent()
        {
            var factory = new ControlFactory();
            var window = factory.Create<Window>();
            window.Content = factory.Create<Control>();

            Assert.Equal(window, window.Content.Parent);
        }
        
        [Fact]
        public void UnsetsParentOfContent()
        {
            var factory = new ControlFactory();
            var control = factory.Create<Control>();
            
            var window = factory.Create<Window>();
            window.Content = control;
            
            Assert.NotNull(control.Parent);
            
            window.Content = null;

            Assert.Null(control.Parent);
        }
    }
}