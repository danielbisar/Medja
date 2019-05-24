using Medja.Controls;
using Medja.OpenTk.Themes;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests.Performance
{
    public class ButtonRendererPerformance
    {
        private readonly IControlFactory _controlFactory;

        public ButtonRendererPerformance(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }
        
        public Control Create()
        {
            var result = _controlFactory.Create<VerticalStackPanel>();
            result.SpaceBetweenChildren = 1;
            result.ChildrenHeight = 10;
            
            for(int i = 0; i < 1000; i++)
                result.Add(_controlFactory.Create<Button>(p => p.Text = i.ToString()));
            
            result.Add(new Control() { Renderer = new FramesPerSecondCounterRenderer()} );
            
            return result;
        }
    }
}