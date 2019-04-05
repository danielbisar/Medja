using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class TabControlTest
    {
        private readonly IControlFactory _controlFactory;

        public TabControlTest(IControlFactory controlFactory)
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
            result.Background = Colors.White;
            result.AddTab(new TabItem("Tab1", tab1));
            result.AddTab(new TabItem("Tab2", tab2));
            result.Margin.SetAll(50);
            result.Padding.SetAll(50);

            return result;
        }
    }
}