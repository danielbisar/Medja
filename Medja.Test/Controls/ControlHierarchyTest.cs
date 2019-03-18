using System.Linq;
using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Controls
{
    public class ControlHierarchyTest
    {
        [Fact]
        public void IgnoresInvisibleControls()
        {
            var factory = new ControlFactory();
            
            var button = factory.Create<Button>();
            var contentControl = factory.Create<ContentControl>();
            contentControl.Content = button; 
            
            var hierarchy = new ControlHierarchy(contentControl);
            var controls = hierarchy.GetInRenderingOrder().ToList();
            
            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p), 
                p => Assert.Same(button, p));

            
            button.Visibility = Visibility.Hidden;
            controls = hierarchy.GetInRenderingOrder().ToList();
            
            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p));

            
            button.Visibility = Visibility.Collapsed;
            controls = hierarchy.GetInRenderingOrder().ToList();
            
            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p));

            button.Visibility = Visibility.Visible;
            controls = hierarchy.GetInRenderingOrder().ToList();
            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p), 
                p => Assert.Same(button, p));
        }
    }
}