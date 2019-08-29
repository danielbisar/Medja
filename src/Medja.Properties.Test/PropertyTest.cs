using Xunit;

namespace Medja.Properties.Test
{
    public class PropertyTest
    {
        [Fact]
        public void HasDefaultValueOnConstruction()
        {
            var property = new Property<int>();
            
            Assert.Equal(default(int), property.Get());
        }

        [Fact]
        public void UnnotifiedSetDoesNotNotify()
        {
            var property = new Property<int>();
            var wasNotified = false;

            property.PropertyChanged += (s, e) => wasNotified = true;
            property.SetSilent(10);
            
            Assert.False(wasNotified);
        }
        
        [Fact]
        public void UnnotifiedSetSetValue()
        {
            var wasNotified = false;
            var property = new Property<int>();
            property.PropertyChanged += (s, e) => wasNotified = true;
            property.SetSilent(10);
            Assert.Equal(10, property.Get());
            Assert.False(wasNotified);
        }
        
        [Fact]
        public void SetDoesNotifyOnChangeOnly()
        {
            var property = new Property<int>();
            var wasNotified = false;

            property.PropertyChanged += (s, e) => wasNotified = true;
            property.Set(default(int));
            
            Assert.False(wasNotified);
            
            property.Set(10);
            
            Assert.True(wasNotified);
        }
        
        [Fact]
        public void SetSetsValue()
        {
            var property = new Property<int>();
            property.Set(10);
            Assert.Equal(10, property.Get());
        }
        
        [Fact]
        public void NotifyPropertyChangedTriggers()
        {
            var property = new Property<object>();
            property.Set(new object());
            
            var wasNotified = false;

            property.PropertyChanged += (s, e) =>
            {
                wasNotified = true;
                Assert.Same(e.OldValue, e.NewValue);
            };
            
            property.NotifyPropertyChanged();
            
            Assert.True(wasNotified);
        }
    }
}