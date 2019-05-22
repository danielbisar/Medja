using System;
using Medja.Controls;

namespace Medja.Theming
{
	public abstract class ControlRendererBase<TContext, TControl> : IControlRenderer
		where TContext : class
		where TControl : Control
	{
		protected TControl _control;
		
		protected ControlRendererBase(TControl control)
		{
			_control = control ?? throw new ArgumentNullException(nameof(control));
		}
		
		protected abstract void Render(TContext context);

		public void Render(object context)
		{
			Render(context as TContext);
		}

		protected virtual void Dispose(bool disposing)
		{
			_control = null;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
