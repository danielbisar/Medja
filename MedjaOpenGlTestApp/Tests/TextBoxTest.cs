using Medja.Controls;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class TextBoxTest
    {
        private readonly ControlFactory _controlFactory;

        public TextBoxTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            var result = _controlFactory.Create<TablePanel>();
            result.Columns.Add(new ColumnDefinition(200));
            result.Columns.Add(new ColumnDefinition(200));
            result.Rows.Add(new RowDefinition(30));
            result.Rows.Add(new RowDefinition(30));

            result.Children.Add(_controlFactory.CreateTextBlock("Test1"));
            result.Children.Add(_controlFactory.Create<TextBox>(p => p.Text = "Box 1"));
            result.Children.Add(_controlFactory.CreateTextBlock("Test1"));
            result.Children.Add(_controlFactory.Create<TextBox>(p => p.Text = "Box 2"));
            
            return result;
        }
    }
}