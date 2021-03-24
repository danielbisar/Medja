using Medja.Controls;
using Medja.Controls.Panels;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls.Panels
{
    public class VerticalStackPanelTest
    {
        [Fact]
        public void SetsAndUnsetsParentOfChild()
        {
            var factory = new ControlFactory();
            var control = factory.Create<Control>();
            var panel = factory.Create<VerticalStackPanel>();
            
            panel.Children.Add(control);

            Assert.Equal(panel, control.Parent);
            
            panel.Children.RemoveAt(0);
            
            Assert.Null(control.Parent);
        }
    }
}
