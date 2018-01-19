using Medja.Controls;
using Medja.Controls.Panels;

namespace Medja.OpenTk
{
    // TODO should be a StackPanel to be more generic, and should be inside of Medja.Controls.Panels
    public class Menu : StackPanel
    {
        public Menu(float width, float height)
        {
            Width = width;
            Height = height;

            AddButton();
            AddButton();
            AddButton();
            AddButton();
        }

        private void AddButton()
        {
            var button = new Button();
            Add(button);
        }
    }
}
