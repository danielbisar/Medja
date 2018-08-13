using Medja.Controls;

namespace Medja.Theming
{
	public abstract class ControlRendererBase<TContext, TControl> : IControlRenderer
		where TContext : class
		where TControl : Control
	{
		protected abstract void Render(TContext context, TControl control);

		public void Render(object context, Control control)
		{
			Render(context as TContext, control as TControl);
		}
	}
}
