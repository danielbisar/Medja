using System;
using Medja.Primitives;

namespace Medja.Controls
{
	public class QuestionDialog : Dialog
	{
		private readonly ControlFactory _controlFactory;
		private readonly TextBlock _messageTextBlock;

		public readonly Property<string> PropertyMessage;
		public string Message
		{
			get { return PropertyMessage.Get(); }
			set { PropertyMessage.Set(value); }
		}

		public readonly Property<bool> PropertyIsConfirmed;
		public bool IsConfirmed
		{
			get { return PropertyIsConfirmed.Get(); }
			set { PropertyIsConfirmed.Set(value); }
		}

		public QuestionDialog(ControlFactory controlFactory)
		{
			PropertyMessage = new Property<string>();
			PropertyMessage.PropertyChanged += OnTextChanged;
			PropertyIsConfirmed = new Property<bool>();

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

			var stackPanel = _controlFactory.Create<VerticalStackPanel>();
			stackPanel.Padding = new Thickness(10);
			stackPanel.Children.Add(_messageTextBlock);

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
			IsConfirmed = false;
			DialogParent.IsDialogVisible = false;
		}

		private void OnOkButtonClicked(object sender, EventArgs e)
		{
			IsConfirmed = true;
			DialogParent.IsDialogVisible = false;
		}
	}
}
