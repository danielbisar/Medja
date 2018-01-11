using System.Collections.Generic;
using System.Linq;
using Medja.Controls;
using Medja.Rendering;

namespace Medja.OpenTk
{
    // TODO should be a StackPanel to be more generic, and should be inside of Medja.Controls.Panels
    class Menu
    {
        private readonly IList<Button> _buttons;

        public float Width { get; set; }

        public Menu()
        {
            _buttons = new List<Button>();
            AddButton(0, 0, 1, 0.5f);
        }

        private void AddButton(float x, float y, float width, float height)
        {
            var button = new Button
            {
                Width = width,
                Height = height,
                X = x,
                Y = y
            };

            _buttons.Add(button);
        }

        public void Render(RenderContext renderContext)
        {
            foreach (var button in _buttons.Where(p => !p.IsRendered || renderContext.ForceRender))
                button.Render(renderContext);
        }
    }
}
