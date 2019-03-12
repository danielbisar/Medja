using Medja.Controls;
using Medja.Theming;

namespace Medja.OpenTk.Rendering
{
	public class OpenTkTheme : ControlFactory
	{
		protected override Button CreateButton()
		{
			var result = base.CreateButton();
			result.Renderer = new ButtonRenderer(result);
			result.Font.Name = "Monospace";
			result.Position.Height = 50;

			return result;
		}

		protected override CheckBox CreateCheckBox()
		{
			var result = base.CreateCheckBox();
			result.Renderer = new CheckBoxRenderer(result);
			result.Position.Height = 26;
			
			return result;
		}

		protected override Control CreateControl()
		{
			var result = base.CreateControl();
			result.Renderer = new ControlRenderer(result);

			return result;
		}

		protected override ContentControl CreateContentControl()
		{
			var result = base.CreateContentControl();
			result.Renderer = new ControlRenderer(result);

			return result;
		}

		protected override Dialog CreateDialog()
		{
			var result = base.CreateDialog();
			result.Renderer = new ControlRenderer(result);

			return result;
		}

		protected override DialogButtonsControl CreateDialogButtonsControl()
		{
			var result = base.CreateDialogButtonsControl();
			result.ButtonWidth = 150;

			return result;
		}

		protected override Image CreateImage()
		{
			var result = base.CreateImage();
			result.Renderer = new ImageRenderer(result);
			
			return result;
		}

		protected override InputBoxDialog CreateInputBoxDialog()
		{
			var result = base.CreateInputBoxDialog();
			result.Renderer = new ControlRenderer(result);
			result.Background = ColorMap.Primary;

			return result;
		}

		protected override NumericKeypad CreateNumericKeypad()
		{
			var result = base.CreateNumericKeypad();
			result.Renderer = new ControlRenderer(result);
			result.Background = ColorMap.Primary;
			
			return result;
		}
		
		protected override NumericKeypadDialog CreateNumericKeypadDialog()
		{
			var result = base.CreateNumericKeypadDialog();
			result.Renderer = new ControlRenderer(result);
			result.Background = ColorMap.Primary;
			
			return result;
		}

		protected override ScrollingGrid CreateScrollingGrid()
		{
			var result = base.CreateScrollingGrid();
			result.Renderer = new ControlRenderer(result);

			return result;
		}

		protected override SimpleMessageDialog CreateSimpleMessageDialog()
		{
			var result = base.CreateSimpleMessageDialog();
			result.Renderer = new ControlRenderer(result);
			result.Background = ColorMap.Primary;

			return result;
		}

		protected override Slider CreateSlider()
		{
			var result = base.CreateSlider();
			result.Renderer = new SliderRenderer(result);
			result.Position.Height = 30;

			return result;
		}

		protected override TabControl CreateTabControl()
		{
			var result = base.CreateTabControl();
			result.Renderer = new TabControlRenderer(result);

			return result;
		}

		protected override TextBox CreateTextBox()
		{
			var result = base.CreateTextBox();
			result.Renderer = new TextBoxRenderer(result);
			result.TextColor = ColorMap.PrimaryText;
			result.Background = ColorMap.Primary;
			result.Font.Name = "Monospace";
			result.Position.Height = 26;

			return result;
		}

		protected override TextBlock CreateTextBlock()
		{
			var result = base.CreateTextBlock();
			result.Renderer = new TextBlockRenderer(result);
			result.TextColor = ColorMap.PrimaryText;
			result.Font.Name = "Monospace";
			result.Position.Height = 23;

			return result;
		}

		protected override ProgressBar CreateProgressBar()
		{
			var result = base.CreateProgressBar();
			result.Renderer = new ProgressBarRenderer(result);

			return result;
		}

		protected override QuestionDialog CreateQuestionDialog()
		{
			var result = base.CreateQuestionDialog();
			result.Renderer = new ControlRenderer(result);
			result.Background = ColorMap.Primary;

			return result;
		}

		protected override ComboBox<T> CreateComboBox<T>()
		{
			var result = base.CreateComboBox<T>();
			result.Renderer = new ControlRenderer(result);
			result.Background = ColorMap.Primary;

			return result;
		}

		protected override VerticalScrollBar CreateVerticalScrollBar()
		{
			var result = base.CreateVerticalScrollBar();
			result.Renderer = new VerticalScrollBarRenderer(result);
			result.Position.Width = 20;
			result.Background = ColorMap.Primary;

			return result;
		}

		protected override ScrollableContainer CreateScrollableContainer()
		{
			var result = base.CreateScrollableContainer();
			result.Renderer = new ControlRenderer(result);

			return result;
		}
	}
}
