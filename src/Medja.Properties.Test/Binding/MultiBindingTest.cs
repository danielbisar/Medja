using System.Linq;
using System.Text;
using Medja.Properties.Binding;
using Xunit;

namespace Medja.Properties.Test.Binding
{
    public class MultiBindingTest
    {
        [Fact]
        public void GeneralFunctionality()
        {
            var targetStr = new Property<string>();
            var source1 = new Property<string>();
            var source2 = new Property<string>();
            var source3 = new Property<string>();
            
            var sb = new StringBuilder();
            using (var binding = new MultiBinding<string>(targetStr, properties =>
            {
                sb.Clear();

                foreach (var property in properties.Cast<Property<string>>())
                    sb.Append(property.Get());

                return sb.ToString();
            }))
            {

                binding.AddSource(source1);
                binding.AddSource(source2);
                binding.AddSource(source3);

                source2.Set("2");

                Assert.Equal("2", targetStr.Get());

                source1.Set("1");

                Assert.Equal("12", targetStr.Get());

                source3.Set("3");

                Assert.Equal("123", targetStr.Get());
            }
        }

        [Fact]
        public void UnregistersPropertyOnRemove()
        {
            var targetStr = new Property<string>();
            var source1 = new Property<string>();

            var sb = new StringBuilder();
            using (var binding = new MultiBinding<string>(targetStr, properties =>
            {
                sb.Clear();

                foreach (var property in properties.Cast<Property<string>>())
                    sb.Append(property.Get());

                return sb.ToString();
            }))
            {

                binding.AddSource(source1);

                source1.Set("1");

                Assert.Equal("1", targetStr.Get());

                binding.RemoveSource(source1);
                source1.Set("");
                
                Assert.Equal("1", targetStr.Get());
            }
        }

        [Fact]
        public void UnregistersPropertiesOnDispose()
        {
            var targetStr = new Property<string>();
            var source1 = new Property<string>();
            var source2 = new Property<string>();

            var sb = new StringBuilder();
            using (var binding = new MultiBinding<string>(targetStr, properties =>
            {
                sb.Clear();

                foreach (var property in properties.Cast<Property<string>>())
                    sb.Append(property.Get());

                return sb.ToString();
            }))
            {

                binding.AddSource(source1);
                binding.AddSource(source2);

                binding.Dispose();

                source1.Set("1");
                source2.Set("2");
                
                Assert.Null(targetStr.Get());
            }
        }

        [Fact]
        public void Updates()
        {
            var targetStr = new Property<string>();
            var source1 = new Property<string>();
            var source2 = new Property<string>();

            var sb = new StringBuilder();
            using (var binding = new MultiBinding<string>(targetStr, properties =>
            {
                sb.Clear();

                foreach (var property in properties.Cast<Property<string>>())
                    sb.Append(property.Get());

                return sb.ToString();
            }))
            {

                binding.AddSource(source1);
                binding.AddSource(source2);

                source1.SetSilent("1");
                source2.SetSilent("2");

                Assert.Null(targetStr.Get());
                
                binding.Update();
                
                Assert.Equal("12", targetStr.Get());
            }
        }
    }
}