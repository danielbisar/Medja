using System;
using System.Collections;
using System.Collections.Specialized;

namespace Medja.Properties
{
    public interface IMedjaObservableCollection : INotifyCollectionChanged, IList
    {
        event EventHandler BeforeClear;
    }
}