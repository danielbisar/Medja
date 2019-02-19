using System;
using Xunit;

namespace Medja.Test.Binding
{
    public class BindingTest
    {
        [Fact]
        public void DisposeClearsPropertyConnection()
        {
            var property1 = new Property<string>();
            var property2 = new Property<string>();

            using (var binding = new Binding<string, string>(property1, property2, p => p))
            {
                property2.Set("MyNewValue");
            
                Assert.Equal("MyNewValue", property1.Get());
            }
            
            property2.Set("AnotherValue");
            
            Assert.Equal("MyNewValue", property1.Get());
        }

        [Fact]
        public void FailWithEmptyConverter()
        {
            var property1 = new Property<string>();
            var property2 = new Property<string>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Binding<string, string>(property1, property2, null);
            });
        }

        [Fact]
        public void ValueConverterIsUsed()
        {
            var property1 = new Property<string>();
            var property2 = new Property<string>();

            using (var binding = new Binding<string, string>(property1, property2, p => p + "_target"))
            {
                property2.Set("source");
                
                Assert.Equal("source_target", property1.Get());
            }
        }
        
        [Fact]
        public void TargetDoesNotUpdateSource()
        {
            var property1 = new Property<string>();
            var property2 = new Property<string>();

            using (var binding = new Binding<string, string>(property1, property2, p => p))
            {
                property1.Set("target");
                
                Assert.Null(property2.Get());
            }
        }
    }
}