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
                p.Text = "Botton";
                p.Visibility = Visibility.Collapsed;
            });
            
            var result = _controlFactory.Create<DockPanel>();
            result.Add(Dock.Top, _controlFactory.Create<Button>(p =>
            {
                p.Text = "Top (toggle visibility)";
                p.InputState.MouseClicked += (s, e) =>
                {
                    bottomButton.Visibility = bottomButton.IsVisible 
                            ? Visibility.Collapsed 
                            : Visibility.Visible;
                };
            }));
            result.Add(Dock.Bottom, bottomButton);
            result.Add(Dock.Left, _controlFactory.Create<Button>(p => p.Text = "Left"));
            result.Add(Dock.Right, _controlFactory.Create<Button>(p => p.Text = "Right"));
            result.Add(Dock.Fill, _controlFactory.Create<Button>(p => p.Text = "Fill"));

            return result;
        }
    }
}