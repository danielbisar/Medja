using Medja.Controls.Panels;
using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;

namespace Medja.Controls.Dialogs
{
	/// <summary>
	/// A dialog that displays a question to the user. Can be confirmed with Ok/Cancel. Later maybe yes/no.
	/// </summary>
	public class QuestionDialog : ConfirmableDialog
	{
		private TextBlock _messageTextBlock;

		public readonly Property<string> PropertyMessage;
		public string Message
		{
			get { return PropertyMessage.Get(); }
			set { PropertyMessage.Set(value); }
		}

		/// <summary>
		/// Creates a new instance. Please use <see cref="ControlFactory"/> instead.
		/// </summary>
		/// <param name="controlFactory">The <see cref="ControlFactory"/>.</param>
		public QuestionDialog(IControlFactory controlFactory)
			: base(controlFactory)
		{
			PropertyMessage = new Property<string>();
			PropertyMessage.PropertyChanged += OnTextChanged;
		}

		protected override Control CreateContent()
		{
			_messageTextBlock = _controlFactory.Create<TextBlock>();
			_messageTextBlock.TextWrapping = TextWrapping.Auto;

			var result = (DockPanel)base.CreateContent();
			result.Add(Dock.Fill, _messageTextBlock);

			return result;
		}

		private void OnTextChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			_messageTextBlock.Text = Message;
		}
	}
}
