using System;
using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
    public class MedjaWindow : ContentControl
    {
        public Property<string> PropertyTitle { get; }

        public string Title
        {
            get { return PropertyTitle.Get(); }
            set { PropertyTitle.Set(value); }
        }

        public event EventHandler Closed;

        public MedjaWindow()
        {
            PropertyTitle = new Property<string>();
        }

        public virtual void Close()
        {
            NotifyClosed();
        }

        private void NotifyClosed()
        {
            if(Closed != null)
                Closed(this, EventArgs.Empty);
        }
    }
}
