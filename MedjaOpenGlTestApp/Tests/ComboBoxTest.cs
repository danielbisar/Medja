using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ComboBoxTest
    {
        private readonly IControlFactory _controlFactory;

        public ComboBoxTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var result = _controlFactory.Create<ComboBox<string>>();
            result.Position.Height = 30;
            result.Position.Width = 200;
            result.VerticalAlignment = VerticalAlignment.Top;
            result.HorizontalAlignment = HorizontalAlignment.Left;
            /*result.InitializeButtonFromItem = (text, button) => button.Text = text;
            result.IsSelectable = true;
            result.PageSize = 5;*/

            int i = 0;
            
            for(; i < 10; i++)
                result.AddItem("Item " + i);

            var addItemButton = _controlFactory.Create<Button>();
            addItemButton.InputState.Clicked += (s, e) => result.AddItem("New Item " + i++);
            addItemButton.Text = "Add new item";

            var clearButton = _controlFactory.Create<Button>();
            clearButton.InputState.Clicked += (s, e) => result.Clear();
            clearButton.Text = "Clear";

            var stackPanel = _controlFactory.Create<VerticalStackPanel>();
            stackPanel.SpaceBetweenChildren = 50;
            stackPanel.Children.Add(result);
            stackPanel.Children.Add(addItemButton);
            stackPanel.Children.Add(clearButton);
            
            return stackPanel;
        }
    }
}