using System;
using Medja.Primitives;

namespace Medja.Controls
{
	public class InputBoxDialog : Dialog
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

		public readonly Property<bool> PropertyIsConfirmed;
		public bool IsConfirmed
		{
			get { return PropertyIsConfirmed.Get(); }
			set { PropertyIsConfirmed.Set(value); }
		}

		public InputBoxDialog(ControlFactory controlFactory)
		{
			PropertyMessage = new Property<string>();
			PropertyMessage.PropertyChanged += OnTextChanged;
			PropertyIsConfirmed = new Property<bool>();
			PropertyInputText = new Property<string>();
			PropertyInputText.PropertyChanged += OnInputTextChanged;

			_controlFactory = controlFactory;
			_messageTextBlock = _controlFactory.Create<TextBlock>();
			_inputTextBox = _controlFactory.Create<TextBox>();
			_inputTextBox.PropertyText.PropertyChanged += OnTextBoxTextChanged;

			Padding = new Thickness(5);
			Content = CreateContent();
		}

		private void OnTextBoxTextChanged(IProperty property)
		{
			InputText = _inputTextBox.Text;
		}

		private void OnInputTextChanged(IProperty property)
		{
			_inputTextBox.Text = InputText;
		}

		private void OnTextChanged(IProperty property)
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

			var okButton = CreateButton(Globalization.OK);
			okButton.InputState.MouseClicked += OnOkButtonClicked;

			var cancelButton = CreateButton(Globalization.Cancel);
			cancelButton.InputState.MouseClicked += OnCancelButtonClicked;

			var buttonDockPanel = _controlFactory.Create<DockPanel>();
			buttonDockPanel.Position.Height = 60;
			buttonDockPanel.Add(Dock.Left, okButton);
			buttonDockPanel.Add(Dock.Right, cancelButton);

			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Bottom, buttonDockPanel);
			dockPanel.Add(Dock.Fill, stackPanel);

			return dockPanel;
		}

		private void OnCancelButtonClicked(object sender, EventArgs e)
		{
			IsConfirmed = false;
			DialogParent.IsDialogVisible = false;
		}

		public override void Arrange(Size availableSize)
		{
			base.Arrange(availableSize);
		}

		private void OnOkButtonClicked(object sender, EventArgs e)
		{
			IsConfirmed = true;
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
