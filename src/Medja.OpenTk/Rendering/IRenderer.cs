using System;
using System.Collections.Generic;
using System.Drawing;
using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public interface IRenderer : IDisposable
    {
	    /// <summary>
	    /// Sets the size of the target to render on.
	    /// </summary>
	    /// <param name="rectangle">The new size.</param>
		void SetSize(Rectangle rectangle);
		
		/// <summary>
		/// Renders a list of controls.
		/// </summary>
		/// <param name="controls">The controls to render.</param>
		/// <returns>true if any <see cref="Control"/> was rendered.</returns>
		bool Render(IList<Control> controls);
    }
}
