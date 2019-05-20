using Medja.Controls;

namespace Medja.OpenTk.Themes.BlackRed
{
	public class TextBlockRenderer : TextControlRendererBase<TextBlock>
	{
		public TextBlockRenderer(TextBlock control)
			: base(control)
		{
		}
		
		protected override void DrawTextControlBackground()
		{
			RenderBackground();
		}
	}
}
