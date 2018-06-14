using Medja.Controls;

namespace Medja.OpenTk.Rendering
{
	public class OpenTkTheme : ControlFactory
	{
		protected override Button CreateButton()
		{
			var result = base.CreateButton();
			result.Renderer = new ButtonRenderer();

			return result;
		}

		protected override ProgressBar CreateProgressBar()
		{
			var result = base.CreateProgressBar();
			result.Renderer = new ProgressBarRenderer();

			return result;
		}
	}
}
