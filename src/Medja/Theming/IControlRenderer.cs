using System;

namespace Medja.Theming
{
	public interface IControlRenderer : IDisposable
    {
		void Render(object context);
    }
}
