using Medja.Controls;
using Medja.Primitives;
using Xunit;

namespace Medja.Test.Controls
{
    public class ControlTest
    {
        [Fact]
        public void VisibilityUpdatesIsVisible()
        {
            var control = new Control();

            Assert.True(control.IsVisible);

            control.Visibility = Visibility.Hidden;
            
            Assert.False(control.IsVisible);

            control.Visibility = Visibility.Collapsed;
            
            Assert.False(control.IsVisible);

            control.Visibility = Visibility.Visible;
            
            Assert.True(control.IsVisible);
        }
    }
}