using System;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
	public class QuestionDialog : ConfirmableDialog
	{
		private readonly ControlFactory _controlFactory;
		private readonly TextBlock _messageTextBlock;

		public readonly Property<string> PropertyMessage;
		public string Message
		{
			get { return PropertyMessage.Get(); }
			set { PropertyMessage.Set(value); }
		}

		public QuestionDialog(ControlFactory controlFactory)
		{
			PropertyMessage = new Property<string>();
			PropertyMessage.PropertyChanged += OnTextChanged;

			_controlFactory = controlFactory;
			_messageTextBlock = _controlFactory.Create<TextBlock>();

			Padding = new Thickness(5);
			Content = CreateContent();
		}

		private void OnTextChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			_messageTextBlock.Text = Message;
		}

		private Control CreateContent()
		{
			_messageTextBlock.Position.Height = 100;
			_messageTextBlock.TextWrapping = TextWrapping.Auto;

			var buttons = _controlFactory.Create<DialogButtonsControl>();
			buttons.Buttons = DialogButtons.OkCancel;
			buttons.CreateContent();
			buttons.HorizontalAlignment = HorizontalAlignment.Stretch;
			buttons.Button1.InputState.MouseClicked += OnOkButtonClicked;
			buttons.Button2.InputState.MouseClicked += OnCancelButtonClicked;

			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Bottom, buttons);
			dockPanel.Add(Dock.Fill, _messageTextBlock);
			dockPanel.VerticalAlignment = VerticalAlignment.Stretch;
			dockPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

			return dockPanel;
		}

		private void OnCancelButtonClicked(object sender, EventArgs e)
		{
			Dismiss();
		}

		private void OnOkButtonClicked(object sender, EventArgs e)
		{
			Confirm();
		}
	}
}
