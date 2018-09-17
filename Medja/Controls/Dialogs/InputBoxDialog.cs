using System;
using Medja.Primitives;

namespace Medja.Controls
{
	public class InputBoxDialog : ConfirmableDialog
	{
		private readonly ControlFactory _controlFactory;
		private readonly TextBlock _messageTextBlock;
		private readonly TextBox _inputTextBox;

		public readonly Property<string> PropertyMessage;
		public string Message
		{
			get { return PropertyMessage.Get(); }
			set { PropertyMessage.Set(value); }
		}

		public readonly Property<string> PropertyInputText;
		public string InputText
		{
			get { return PropertyInputText.Get(); }
			set { PropertyInputText.Set(value); }
		}

		public InputBoxDialog(ControlFactory controlFactory)
		{
			PropertyMessage = new Property<string>();
			PropertyMessage.PropertyChanged += OnTextChanged;
			PropertyInputText = new Property<string>();
			PropertyInputText.PropertyChanged += OnInputTextChanged;

			_controlFactory = controlFactory;
			_messageTextBlock = _controlFactory.Create<TextBlock>();
			_inputTextBox = _controlFactory.Create<TextBox>();
			_inputTextBox.PropertyText.PropertyChanged += OnTextBoxTextChanged;

			Padding = new Thickness(5);
			Content = CreateContent();
		}

		private void OnTextBoxTextChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			InputText = _inputTextBox.Text;
		}

		private void OnInputTextChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			_inputTextBox.Text = InputText;
		}

		private void OnTextChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			_messageTextBlock.Text = Message;
		}

		private Control CreateContent()
		{
			_messageTextBlock.Position.Height = 50;

			var stackPanel = _controlFactory.Create<VerticalStackPanel>();
			stackPanel.Padding = new Thickness(10);
			stackPanel.Children.Add(_messageTextBlock);
			stackPanel.Children.Add(_inputTextBox);

			var buttons = _controlFactory.Create<DialogButtonsControl>();
			buttons.Buttons = DialogButtons.OkCancel;
			buttons.CreateContent();
			buttons.Button1.InputState.MouseClicked += OnOkButtonClicked;
			buttons.Button2.InputState.MouseClicked += OnCancelButtonClicked;

			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Bottom, buttons);
			dockPanel.Add(Dock.Fill, stackPanel);

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
