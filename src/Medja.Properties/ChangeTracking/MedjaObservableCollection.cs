using System;
using System.Collections.ObjectModel;

namespace Medja.Properties.ChangeTracking
{
    public class MedjaObservableCollection<T> : ObservableCollection<T>, IMedjaObservableCollection
    {
        public event EventHandler BeforeClear;

        protected override void ClearItems()
        {
            BeforeClear?.Invoke(this, EventArgs.Empty);
            base.ClearItems();
        }
    }
}
