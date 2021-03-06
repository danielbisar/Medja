using System;
using Medja.Controls.Panels;
using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;

namespace Medja.Controls.Dialogs
{
	public class SimpleMessageDialog : Dialog
	{
		private readonly IControlFactory _controlFactory;
		private readonly TextBlock _messageTextBlock;

		public readonly Property<string> PropertyText;
		public string Text
		{
			get { return PropertyText.Get(); }
			set { PropertyText.Set(value); }
		}

		public SimpleMessageDialog(IControlFactory controlFactory)
		{
			PropertyText = new Property<string>();
			PropertyText.PropertyChanged += OnTextChanged;

			_controlFactory = controlFactory;
			_messageTextBlock = _controlFactory.Create<TextBlock>();

			Padding.SetAll(5);
			Content = CreateContent();
		}

		private void OnTextChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			_messageTextBlock.Text = Text;
		}

		private Control CreateContent()
		{
			_messageTextBlock.TextWrapping = TextWrapping.Auto;
			_messageTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
			_messageTextBlock.VerticalAlignment = VerticalAlignment.Stretch;

			var buttons = _controlFactory.Create<DialogButtonsControl>();
			buttons.Buttons = DialogButtons.Ok;
			buttons.CreateContent();
			buttons.Button1.InputState.Clicked += OnOkButtonClicked;

			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Bottom, buttons);
			dockPanel.Add(Dock.Fill, _messageTextBlock);

			return dockPanel;
		}

		private void OnOkButtonClicked(object sender, EventArgs e)
		{
			DialogService.Hide(this);
		}
	}
}
