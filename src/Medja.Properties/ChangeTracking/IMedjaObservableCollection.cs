using System;
using System.Collections;
using System.Collections.Specialized;

namespace Medja.Properties.ChangeTracking
{
    public interface IMedjaObservableCollection : INotifyCollectionChanged, IList
    {
        event EventHandler BeforeClear;
    }
}
