using System;
using System.Collections.Specialized;
using System.Linq;
using Medja.Properties.ChangeTracking;

namespace Medja.Properties
{
    public class PropertyCollectionChangedHandler
    {
        public string PropertyName { get; }
        public Action<CollectionPropertyChange> OnChange { get; }

        public PropertyCollectionChangedHandler(string propertyName, Action<CollectionPropertyChange> onChange)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            OnChange = onChange ?? throw new ArgumentNullException(nameof(onChange));
        }

        public void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var index = e.NewStartingIndex;
                var newItems = e.NewItems;

                foreach (var item in newItems)
                    OnChange(new CollectionPropertyChange(PropertyName, NotifyCollectionChangedAction.Add, item, index++));
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var index = e.OldStartingIndex;
                var oldItems = e.OldItems;
                
                foreach (var item in oldItems)
                    OnChange(new CollectionPropertyChange(PropertyName, NotifyCollectionChangedAction.Remove, item, index++));
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // does not contain any item information so useless for
                // creating changes 
                return;
            }
        }

        public void HandleBeforeClear(object sender, EventArgs e)
        {
            var items = ((IMedjaObservableCollection) sender).Cast<object>().ToArray();
            OnChange(new CollectionPropertyChange(PropertyName, NotifyCollectionChangedAction.Reset, items));
        }
    }
}