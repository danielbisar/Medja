using Medja.Controls;
using Medja.Debug;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class Control3DTest
    {
        private readonly ControlFactory _controlFactory;

        public Control3DTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var controls = new OpenGlTestControl[4];

            for (int i = 0; i < 4; i++)
            {
                controls[i] = new OpenGlTestControl();
                controls[i].Renderer = new OpenGlTestControlRenderer();
                controls[i].Margin.SetAll(10);
            }

            var ct = _controlFactory.Create<ScrollingGrid>();
            ct.SpacingX = 10;
            ct.SpacingY = 10;
            ct.RowHeight = 200;
            ct.Background = Colors.LightGray;
            ct.Margin.SetAll(10);
            ct.Children.Add(controls[0]);
            ct.Children.Add(controls[1]);
            ct.Children.Add(controls[2]);
            ct.Children.Add(controls[3]);

            return ct;
        }
    }
}