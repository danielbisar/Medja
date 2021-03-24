using System;
using Medja.Controls.Panels;
using Medja.Primitives;
using Medja.Properties;
using Medja.Theming;

namespace Medja.Controls.Dialogs
{
    /// <summary>
    /// Dialog that allows a boolean result with two buttons at the bottom.
    /// </summary>
    public class ConfirmableDialog : Dialog
    {
        protected readonly IControlFactory _controlFactory;

        public readonly Property<bool> PropertyIsConfirmed;
        /// <summary>
        /// Gets or sets if the dialog was confirmed.
        /// </summary>
        public bool IsConfirmed
        {
            get { return PropertyIsConfirmed.Get(); }
            set { PropertyIsConfirmed.Set(value); }
        }

        /// <summary>
        /// Creates a new instance. Use <see cref="ControlFactory"/> to create an instance.
        /// </summary>
        /// <param name="controlFactory"><see cref="ControlFactory"/> for the sub controls.</param>
        public ConfirmableDialog(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;

            PropertyIsConfirmed = new Property<bool>();
            Padding.SetAll(5);

            Content = CreateContent();
        }

        protected virtual Control CreateContent()
        {
            var buttons = _controlFactory.Create<DialogButtonsControl>();
            buttons.Buttons = DialogButtons.OkCancel;
            buttons.CreateContent();
            buttons.HorizontalAlignment = HorizontalAlignment.Stretch;
            buttons.Button1.InputState.Clicked += OnConfirmButtonClicked;
            buttons.Button2.InputState.Clicked += OnCancelButtonClicked;

            var dockPanel = _controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Bottom, buttons);
            dockPanel.VerticalAlignment = VerticalAlignment.Stretch;
            dockPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            return dockPanel;
        }

        /// <summary>
        /// Sets IsConfirmed and closes the dialog.
        /// </summary>
        public void Confirm()
        {
            IsConfirmed = true;
            Hide();
        }

        /// <summary>
        /// Sets IsConfirmed and closes the dialog.
        /// </summary>
        public void Dismiss()
        {
            IsConfirmed = false;
            Hide();
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Dismiss();
        }

        private void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            Confirm();
        }
    }
}
