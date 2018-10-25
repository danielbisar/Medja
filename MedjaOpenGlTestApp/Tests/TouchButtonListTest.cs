using Medja.Controls;
using Medja.Theming;

namespace MedjaOpenGlTestApp.Tests
{
    public class TouchButtonListTest
    {
        private readonly ControlFactory _controlFactory;

        public TouchButtonListTest(ControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public ContentControl Create()
        {
            var result = _controlFactory.Create<TouchButtonList<string>>();
            result.InitializeButtonFromItem = (text, button) => button.Text = text;
            result.IsSelectable = true;
            result.PageSize = 5;
            
            for(int i = 0; i < 10; i++)
                result.AddItem("Item " + i);

            result.SelectedItem = result.Items[2];
            
            return result;
        }
    }
}