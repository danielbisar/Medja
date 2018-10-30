using Medja.Controls;
using Medja.Primitives;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class ComboBoxTest
    {
        private readonly ControlFactory _controlFactory;

        public ComboBoxTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public ContentControl Create()
        {
            var result = _controlFactory.Create<ComboBox<string>>();
            result.Position.Height = 30;
            result.Position.Width = 200;
            result.VerticalAlignment = VerticalAlignment.Top;
            result.HorizontalAlignment = HorizontalAlignment.Left;
            /*result.InitializeButtonFromItem = (text, button) => button.Text = text;
            result.IsSelectable = true;
            result.PageSize = 5;*/
            
            /*for(int i = 0; i < 10; i++)
                result.AddItem("Item " + i);*/

            //result.SelectedItem = result.Items[2];
            
            return result;
        }
    }
}