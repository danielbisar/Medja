using System;

namespace Medja.Controls
{
    public interface IControlRenderer
    {
		void Render(object context, Control control);
    }
}
