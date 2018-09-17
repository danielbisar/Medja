namespace Medja.Controls
{
    public class ConfirmableDialog : Dialog
    {
        public readonly Property<bool> PropertyIsConfirmed;
        public bool IsConfirmed
        {
            get { return PropertyIsConfirmed.Get(); }
            set { PropertyIsConfirmed.Set(value); }
        }

        public ConfirmableDialog()
        {
            PropertyIsConfirmed = new Property<bool>();
        }

        public void Confirm()
        {
            IsConfirmed = true;
            Hide();
        }

        public void Dismiss()
        {
            IsConfirmed = false;
            Hide();
        }
    }
}