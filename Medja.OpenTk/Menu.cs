using Medja.Controls;
using Medja.Controls.Panels;

namespace Medja.OpenTk
{
    // TODO should be a StackPanel to be more generic, and should be inside of Medja.Controls.Panels
    public class Menu : StackPanel
    {
        public Menu(float width)
        {
            Width = width;
            AddButton(0, 0, 0.5f);
        }

        private void AddButton(float x, float y, float height)
        {
            var button = new Button
            {
                Width = Width,
                Height = height,
                X = x,
                Y = y
            };

            Add(button);
        }
    }
}
