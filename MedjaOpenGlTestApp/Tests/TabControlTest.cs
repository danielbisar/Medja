using Medja.Controls;
using Medja.Primitives;

namespace MedjaOpenGlTestApp.Tests
{
    public class TabControlTest
    {
        private readonly ControlFactory _controlFactory;

        public TabControlTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var tab1 = _controlFactory.Create<Control>();
            tab1.Background = Colors.Green;
            
            var tab2 = _controlFactory.Create<Control>();
            tab2.Background = Colors.Blue;
            
            var result = _controlFactory.Create<TabControl>();
            result.AddTab(new TabItem("Tab1", tab1));
            result.AddTab(new TabItem("Tab2", tab2));

            return result;
        }
    }
}