using System.Linq;
using Medja.Controls;
using Medja.Controls.Panels;
using Medja.Theming;
using Xunit;

namespace Medja.Test.Theming
{
    public class ControlFactoryTest
    {
        [Fact]
        public void NoDuplicateKeysTest()
        {
            // the constructor would throw an exception if any element is added multiple times to the internal dict
            var instance = new ControlFactory();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CreateControlTest()
        {
            var control = new ControlFactory().Create<Control>();
            Assert.NotNull(control);
        }

        [Fact]
        public void CreateGenericControlTest()
        {
            var control = new ControlFactory().Create<TouchItemList<string>>();
            Assert.NotNull(control);
        }

        [Fact]
        public void HasControlTest()
        {
            Assert.True(new ControlFactory().HasControl<Control>());
        }

        /// <summary>
        /// This test verifies that all controls that are present in Medja are also registered in the
        /// <see cref="ControlFactory"/> class.
        /// </summary>
        [Fact]
        public void HasAllControlClassesTest()
        {
            var assembly = typeof(ControlFactory).Assembly;
            var controls = assembly.ExportedTypes.Where(p => p.IsSubclassOf(typeof(Control))
                                                             && !p.IsGenericType
                                                             && !p.IsAbstract
                                                             && p != typeof(Panel));
            var factory = new ControlFactory();

            foreach (var control in controls)
            {
                Assert.True(factory.HasControl(control),
                    $"The control {control.FullName} cannot be created with the ControlFactory");
            }
        }
    }
}
