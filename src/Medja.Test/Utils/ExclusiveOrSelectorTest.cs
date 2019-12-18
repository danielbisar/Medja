using Medja.Controls;
using Medja.Properties;
using Medja.Utils;
using Xunit;

namespace Medja.Test.Utils
{
    internal class Selectable : Control
    {
        public readonly Property<bool> PropertyIsSelected;
        public bool IsSelected
        {
            get => PropertyIsSelected.Get();
            set => PropertyIsSelected.Set(value);
        }

        public Selectable()
        {
            PropertyIsSelected = new Property<bool>();
        }
    }
    
    public class ExclusiveOrSelectorTest
    {
        [Fact]
        public void SetSelectedControlTest()
        {
            var selector = new ExclusiveOrSelector<Selectable>(p => p.PropertyIsSelected);
            var s1 = new Selectable();
            var s2 = new Selectable();
            var s3 = new Selectable();

            selector.Add(s1);
            selector.Add(s2);
            selector.Add(s3);

            Assert.Null(selector.SelectedControl);

            selector.SelectedControl = s2;

            Assert.True(s2.IsSelected);

            selector.SelectedControl = s1;

            Assert.False(s2.IsSelected);
            Assert.True(s1.IsSelected);
        }

        [Fact]
        public void SetIsSelectedTest()
        {
            var selector = new ExclusiveOrSelector<Selectable>(p => p.PropertyIsSelected);
            var s1 = new Selectable();
            var s2 = new Selectable();
            var s3 = new Selectable();

            selector.Add(s1);
            selector.Add(s2);
            selector.Add(s3);

            Assert.Null(selector.SelectedControl);

            s2.IsSelected = true;
            
            Assert.Equal(s2, selector.SelectedControl);

            s1.IsSelected = true;
            
            Assert.False(s2.IsSelected);
            Assert.True(s1.IsSelected);
            Assert.Equal(s1, selector.SelectedControl);
        }
    }
}