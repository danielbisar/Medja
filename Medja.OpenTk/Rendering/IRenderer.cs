using System;
using System.Collections.Generic;
using System.Drawing;
using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public interface IRenderer : IDisposable
    {
		void SetSize(Rectangle rectangle);
		void Render(IEnumerable<Control> controls);
    }
}
