using System;
using Medja.Theming;

namespace Medja.Controls
{
	public class DialogButtonsControl : ContentControl
	{
		private readonly ControlFactory _factory;
		public DialogButtons Buttons { get; set; }

		public Button Button1 { get; protected set; }
		public Button Button2 { get; protected set; }

		private float _buttonWidth;
		public float ButtonWidth
		{
			get { return _buttonWidth; }
			set { _buttonWidth = value; }
		}

		private float _buttonHeight;
		public float ButtonHeight
		{
			get { return _buttonHeight; }
			set { _buttonHeight = value; }
		}

		public DialogButtonsControl(ControlFactory factory)
		{
			_factory = factory;
			_buttonWidth = 100;
			_buttonHeight = 60;
		}

		public virtual void CreateContent()
		{
			Position.Height = _buttonHeight;
			CreateButtons();

			var buttonDockPanel = _factory.Create<DockPanel>();
			buttonDockPanel.Position.Height = _buttonHeight;
			buttonDockPanel.Add(Dock.Left, Button1);

			if (Buttons != DialogButtons.Ok)
				buttonDockPanel.Add(Dock.Right, Button2);
			
			Content = buttonDockPanel;
		}

		private void CreateButtons()
		{
			switch (Buttons)
			{
				case DialogButtons.Ok:
					CreateOkButton();
					break;
				case DialogButtons.OkCancel:
					CreateOkCancelButtons();
					break;
				case DialogButtons.YesNo:
					CreateYesNoButtons();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(Buttons));
			}
		}

		private void CreateOkButton()
		{
			Button1 = CreateButton(Globalization.OK);
		}

		private void CreateOkCancelButtons()
		{
			Button1 = CreateButton(Globalization.OK);
			//Button1.SetAttachedProperty(Dialog.IsDefaultAttachedId, true);
			Button2 = CreateButton(Globalization.Cancel);
		}

		private void CreateYesNoButtons()
		{
			Button1 = CreateButton(Globalization.Yes);
			//Button1.SetAttachedProperty(Dialog.IsDefaultAttachedId, true);
			Button2 = CreateButton(Globalization.No);
		}

		private Button CreateButton(string text)
		{
			var result = _factory.Create<Button>();
			result.Text = text;
			result.Position.Width = _buttonWidth;

			return result;
		}
	}
}
