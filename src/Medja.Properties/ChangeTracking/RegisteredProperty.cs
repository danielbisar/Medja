using System;
using Medja.Properties.ChangeTracking.Changes;

namespace Medja.Properties.ChangeTracking
{
    /// <summary>
    /// Is used to describe a property used by <see cref="PropertyRegistry"/>.
    /// </summary>
    internal class RegisteredProperty : IDisposable
    {
        private PropertyCollectionChangedHandler _collectionChangedHandler;

        public string Name { get; }
        public IProperty Property { get; }

        public RegisteredProperty(string name, IProperty property)
        {
            Name = name;
            Property = property;
        }

        public void TryAddCollectionListener(Action<CollectionPropertyChange> addCollectionChange)
        {
            if(addCollectionChange == null)
                throw new ArgumentNullException(nameof(addCollectionChange));

            var valueType = Property.GetValueType();

            if (!typeof(IMedjaObservableCollection).IsAssignableFrom(valueType))
                return;

            _collectionChangedHandler = new PropertyCollectionChangedHandler(Name, addCollectionChange);

            var value = Property.GetValue() as IMedjaObservableCollection;

            if (value != null)
            {
                value.BeforeClear += _collectionChangedHandler.HandleBeforeClear;
                value.CollectionChanged += _collectionChangedHandler.HandleCollectionChanged;
            }

            Property.PropertyChanged += UpdateCollectionHandler;
        }

        private void UpdateCollectionHandler(object sender, PropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as IMedjaObservableCollection;
            var newValue = e.NewValue as IMedjaObservableCollection;

            if (oldValue != null)
            {
                oldValue.BeforeClear -= _collectionChangedHandler.HandleBeforeClear;
                oldValue.CollectionChanged -= _collectionChangedHandler.HandleCollectionChanged;
            }

            if (newValue != null)
            {
                newValue.BeforeClear += _collectionChangedHandler.HandleBeforeClear;
                newValue.CollectionChanged += _collectionChangedHandler.HandleCollectionChanged;
            }
        }

        public void Dispose()
        {
            if (_collectionChangedHandler != null)
            {
                var value = (IMedjaObservableCollection) Property.GetValue();
                value.BeforeClear -= _collectionChangedHandler.HandleBeforeClear;
                value.CollectionChanged -= _collectionChangedHandler.HandleCollectionChanged;
                _collectionChangedHandler = null;

                Property.PropertyChanged -= UpdateCollectionHandler;
            }
        }
    }
}
