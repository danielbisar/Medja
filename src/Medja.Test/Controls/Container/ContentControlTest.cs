using Medja.Controls;
using Xunit;

namespace Medja.Test.Controls.Container
{
    public class ContentControlTest
    {
        [Fact]
        public void IsEnabledIsForwardedToChild()
        {
            var content = new Control();
            
            var contentControl = new ContentControl();
            contentControl.Content = content;

            Assert.True(contentControl.IsEnabled);
            Assert.True(content.IsEnabled);
            
            contentControl.IsEnabled = false;
            
            Assert.False(contentControl.IsEnabled);
            Assert.False(content.IsEnabled);
        }
        
        [Fact]
        public void UpdateIsEnabledOfContentOnAssignment()
        {
            var content = new Control();
            
            var contentControl = new ContentControl();
            contentControl.IsEnabled = false;
            contentControl.Content = content;

            Assert.False(content.IsEnabled);

            contentControl.Content = null;
            
            Assert.True(content.IsEnabled);

            content.IsEnabled = false;

            contentControl.IsEnabled = true;
            contentControl.Content = content;
            
            Assert.False(content.IsEnabled);

            contentControl.Content = null;
            
            Assert.False(content.IsEnabled);
        }
    }
}