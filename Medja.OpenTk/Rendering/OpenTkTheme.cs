﻿using Medja.Controls;
using Medja.Primitives;

namespace Medja.OpenTk.Rendering
{
	public class OpenTkTheme : ControlFactory
	{
		protected override Button CreateButton()
		{
			var result = base.CreateButton();
			result.Renderer = new ButtonRenderer();
			result.Font.Name = "Sans";
			result.Position.Height = 50;

			return result;
		}

		protected override Control CreateControl()
		{
			var result = base.CreateControl();
			result.Renderer = new ControlRenderer();

			return result;
		}

		protected override Dialog CreateDialog()
		{
			var result = base.CreateDialog();
			result.Renderer = new ControlRenderer();

			return result;
		}

		protected override InputBoxDialog CreateInputBoxDialog()
		{
			var result = base.CreateInputBoxDialog();
			result.Renderer = new ControlRenderer();
			result.Background = ColorMap.Primary;

			return result;
		}

		protected override SimpleMessageDialog CreateSimpleMessageDialog()
		{
			var result = base.CreateSimpleMessageDialog();
			result.Renderer = new ControlRenderer();
			result.Background = ColorMap.Primary;

			return result;
		}

		protected override TextBox CreateTextBox()
		{
			var result = base.CreateTextBox();
			result.Renderer = new TextBoxRenderer();
			result.Foreground = ColorMap.PrimaryText;
			result.Background = ColorMap.Primary;
			result.Font.Name = "Sans";
			result.Position.Height = 26;

			return result;
		}

		protected override TextBlock CreateTextBlock()
		{
			var result = base.CreateTextBlock();
			result.Renderer = new TextBlockRenderer();
			result.Foreground = ColorMap.PrimaryText;
			result.Font.Name = "Sans";

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
