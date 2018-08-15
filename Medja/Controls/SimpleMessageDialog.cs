using System;
using Medja.Primitives;

namespace Medja.Controls
{
	public class SimpleMessageDialog : Dialog
	{
		private readonly ControlFactory _controlFactory;
		private readonly TextBlock _messageTextBlock;

		public readonly Property<string> PropertyText;
		public string Text
		{
			get { return PropertyText.Get(); }
			set { PropertyText.Set(value); }
		}

		public SimpleMessageDialog(ControlFactory controlFactory)
		{
			PropertyText = new Property<string>();
			PropertyText.PropertyChanged += OnTextChanged;

			_controlFactory = controlFactory;
			_messageTextBlock = _controlFactory.Create<TextBlock>();

			Padding = new Thickness(5);
			Content = CreateContent();
		}

		private void OnTextChanged(IProperty property)
		{
			_messageTextBlock.Text = Text;
		}

		private Control CreateContent()
		{
			var innerContentControl = _controlFactory.Create<ContentControl>();
			innerContentControl.Padding = new Thickness(10);
			innerContentControl.Content = _messageTextBlock;

			var okButton = CreateButton(Globalization.OK);
			okButton.InputState.MouseClicked += OnOkButtonClicked;

			var buttonDockPanel = _controlFactory.Create<DockPanel>();
			buttonDockPanel.Position.Height = 60;
			//buttonDockPanel.Padding = new Thickness(50, 0);
			buttonDockPanel.Add(Dock.Bottom, okButton);

			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Bottom, buttonDockPanel);
			dockPanel.Add(Dock.Fill, innerContentControl);

			return dockPanel;
		}

		private void OnOkButtonClicked(object sender, EventArgs e)
		{
			DialogParent.IsDialogVisible = false;
		}

		private Button CreateButton(string text)
		{
			var result = _controlFactory.Create<Button>();
			result.Text = text;
			result.Position.Width = 100;

			return result;
		}
	}
}
