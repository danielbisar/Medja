using System;
using System.Collections.Generic;
using System.Text;
using Medja.Controls;

namespace Medja.Rendering
{
    public interface IControlRenderer
    {
        void Render(Control control, RenderContext context);
    }
}
