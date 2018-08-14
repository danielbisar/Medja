using Medja.Controls;
using Medja.Primitives;

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

		protected override Control CreateControl()
		{
			var result = base.CreateControl();
			result.Renderer = new ControlRenderer();

			return result;
		}

		protected override TextBox CreateTextBox()
		{
			var result = base.CreateTextBox();
			result.Renderer = new TextBoxRenderer();
			result.Background = Colors.LightGray;

			return result;
		}

		protected override TextBlock CreateTextBlock()
		{
			var result = base.CreateTextBlock();
			result.Renderer = new TextBlockRenderer();

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
