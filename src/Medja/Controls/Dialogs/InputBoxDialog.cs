using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;

namespace Medja.Controls
{
	/// <summary>
	/// A dialog that displays an input box to the user.
	/// </summary>
	public class InputBoxDialog : ConfirmableDialog
	{
		private TextBlock _messageTextBlock;
		private TextBox _inputTextBox;

		public readonly Property<string> PropertyMessage;
		
		/// <summary>
		/// Gets or sets the message that is display to the user.
		/// </summary>
		public string Message
		{
			get { return PropertyMessage.Get(); }
			set { PropertyMessage.Set(value); }
		}

		public readonly Property<string> PropertyInputText;
		/// <summary>
		/// Gets or sets the value of the <see cref="TextBox"/> of the dialog.
		/// </summary>
		public string InputText
		{
			get { return PropertyInputText.Get(); }
			set { PropertyInputText.Set(value); }
		}

		public InputBoxDialog(IControlFactory controlFactory)
		: base(controlFactory)
		{
			PropertyMessage = new Property<string>();
			PropertyMessage.PropertyChanged += OnTextChanged;
			PropertyInputText = new Property<string>();
			PropertyInputText.PropertyChanged += OnInputTextChanged;
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

		protected override Control CreateContent()
		{
			_messageTextBlock = _controlFactory.Create<TextBlock>();
			_inputTextBox = _controlFactory.Create<TextBox>();
			_inputTextBox.PropertyText.PropertyChanged += OnTextBoxTextChanged;
			
			_messageTextBlock.Position.Height = 50;
			_messageTextBlock.TextWrapping = TextWrapping.Auto;
			
			var stackPanel = _controlFactory.Create<VerticalStackPanel>();
			stackPanel.Padding = new Thickness(10);
			stackPanel.Children.Add(_messageTextBlock);
			stackPanel.Children.Add(_inputTextBox);
			
			var result = (DockPanel)base.CreateContent();
			result.Add(Dock.Fill, stackPanel);
			
			return result;
		}
	}
}
