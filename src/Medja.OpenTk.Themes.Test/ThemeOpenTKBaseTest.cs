using System;
using System.Linq;
using Medja.Controls;
using Medja.OpenTk.Controls;
using Medja.Theming;
using Xunit;
using Xunit.Abstractions;

namespace Medja.OpenTk.Themes.Test
{
    public class ThemeOpenTKBaseTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ThemeOpenTKBaseTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// This test verifies that all controls that are present in Medja.OpenTk are also registered in the
        /// <see cref="ControlFactory"/> class.
        /// </summary>
        [Fact]
        public void HasAllControlClassesTest()
        {
            var assembly = typeof(OpenGLEventControl).Assembly;
            var controls = assembly.ExportedTypes.Where(p => p.IsSubclassOf(typeof(Control))
                                                             && !p.IsGenericType
                                                             && !p.IsAbstract
                                                             && p != typeof(Panel));
            
            // filter classes - f.e. OpenTkWindow because it inherits from Window
            controls = controls.Where(p => p != typeof(OpenTkWindow));
            
            var factory = new ThemeOpenTKBase();

            foreach (var control in controls)
            {
                Assert.True(factory.HasControl(control),
                    $"The control {control.FullName} cannot be created with the ThemeOpenTKBase control factory.");
            }
        }
    }
}