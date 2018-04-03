using System;

namespace Medja.Controls
{
    public class MedjaWindow : ContentControl
    {
        public Property<string> TitleProperty { get; }
        public string Title
        {
            get { return TitleProperty.Get(); }
            set { TitleProperty.Set(value); }
        }

        public bool IsClosed { get; set; }

        public event EventHandler Closed;

        public MedjaWindow()
        {
            TitleProperty = new Property<string>();
        }

        public virtual void Close()
        {
            IsClosed = true;
            NotifyClosed();
        }

        private void NotifyClosed()
        {
            if(Closed != null)
                Closed(this, EventArgs.Empty);
        }
    }
}
