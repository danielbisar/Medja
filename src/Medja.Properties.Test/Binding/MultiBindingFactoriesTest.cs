using Medja.Properties.Binding;
using Xunit;

namespace Medja.Properties.Test.Binding
{
    public class MultiBindingFactoriesTest
    {
        [Fact]
        public void BasicFunctionTest()
        {
            var target = new Property<int>();
            var sourceA = new Property<int>();
            var sourceB = new Property<int>();

            using (var binding = target.UpdateFrom(sourceA, sourceB, (a, b) => a + b))
            {
                sourceA.Set(1);

                Assert.Equal(1, target.Get());

                sourceB.Set(2);
                
                Assert.Equal(3, target.Get());
            }
        }
    }
}