using Medja.Controls;
using Medja.Controls.Buttons;
using Medja.Controls.Container;
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
            hierarchy.UpdateLists();
            var controls = hierarchy.Lists.All();
            
            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p), 
                p => Assert.Same(button, p));

            
            button.Visibility = Visibility.Hidden;
            hierarchy.UpdateLists();
            controls = hierarchy.Lists.All();
            
            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p));

            
            button.Visibility = Visibility.Collapsed;
            hierarchy.UpdateLists();
            controls = hierarchy.Lists.All();
            
            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p));

            button.Visibility = Visibility.Visible;
            hierarchy.UpdateLists();
            controls = hierarchy.Lists.All();

            Assert.Collection(controls, 
                p => Assert.Same(contentControl, p), 
                p => Assert.Same(button, p));
        }
    }
}
