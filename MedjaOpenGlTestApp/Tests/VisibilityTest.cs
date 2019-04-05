using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class VisibilityTest
    {
        private readonly IControlFactory _controlFactory;

        public VisibilityTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var bottomButton = _controlFactory.Create<Button>(p =>
            {
                p.Text = "Bottom";
                p.Visibility = Visibility.Collapsed;
            });

            var leftButton = _controlFactory.Create<Button>(p =>
            {
                p.Text = "Left";
                p.Position.Width = 200;
            });
            
            var rightButton = _controlFactory.Create<Button>(p =>
            {
                p.Text = "Right";
                p.Position.Width = 200;
            });

            var toggleVisibilityButton = _controlFactory.Create<Button>(p =>
            {
                p.Text = "Toggle left button visibility";
                p.InputState.Clicked += (s, e) =>
                {
                    leftButton.Visibility = leftButton.IsVisible 
                        ? Visibility.Collapsed 
                        : Visibility.Visible;
                };
            });
            
            var result = _controlFactory.Create<DockPanel>();
            result.Add(Dock.Top, _controlFactory.Create<Button>(p => p.Text = "Top"));
            result.Add(Dock.Top, toggleVisibilityButton);
            result.Add(Dock.Bottom, bottomButton);
            result.Add(Dock.Left, leftButton);
            result.Add(Dock.Right, rightButton);
            result.Add(Dock.Fill, _controlFactory.Create<Button>(p => p.Text = "Fill"));

            return result;
        }
    }
}