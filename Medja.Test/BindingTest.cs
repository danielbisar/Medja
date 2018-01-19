using Medja.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medja.Test
{
    [TestClass]
    public class BindingTest
    {
        [TestMethod]
        public void SimpleBinding()
        {
            var source = new MTestObject();
            var target = new MTestObject();

            BindingFactory.Create(target._testInt0, source._testInt0);

            source.TestInt0 = 10;

            Assert.AreEqual(10, target.TestInt0, "Binding did not update the targets value");
        }
    }
}
