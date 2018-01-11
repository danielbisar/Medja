using System.Collections.Generic;
using System.Linq;
using Medja.Controls;
using Medja.Controls.Panels;

namespace Medja.Rendering
{
    public class StackPanelRenderer : IControlRenderer
    {
        public void Render(Control control, RenderContext context)
        {
            var stackPanel = control as StackPanel;
            IEnumerable<Control> children = stackPanel.Children;

            if (!context.ForceRender)
                children = children.Where(p => !p.IsRendered);

            foreach(var item in children)
                item.Render(context);
        }
    }
}
