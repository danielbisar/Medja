using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Medja.Properties.ChangeTracking;

namespace Medja.Properties
{
    /// <summary>
    /// Tracks changes on properties and allow revert.
    /// </summary>
    public class PropertyRegistry : IDisposable
    {
        private readonly RegisteredPropertyMap _properties;
        
        private bool _ignoreChanges;
        
        private readonly List<PropertyChange> _changes;
        public IReadOnlyList<PropertyChange> Changes
        {
            get { return _changes; }
        }
        
        public readonly Property<bool> PropertyIsChanged;
        /// <summary>
        /// Gets if any property changed since the last reset. Does not check for sub objects. Use
        /// <see cref="GetChangesTreeSnapshot"/> if you want to receive also the changes made to sub objects.
        /// </summary>
        public bool IsChanged
        {
            get { return PropertyIsChanged.Get(); }
            private set { PropertyIsChanged.Set(value); }
        }

        public PropertyRegistry()
        {
            _changes = new List<PropertyChange>();
            _ignoreChanges = false;
            _properties = new RegisteredPropertyMap();
            
            PropertyIsChanged = new Property<bool>();
        }

        /// <summary>
        /// Adds a property to the registry.
        /// </summary>
        /// <param name="name">The properties name.</param>
        /// <param name="property">The property.</param>
        public void Add(string name, IProperty property)
        {
            var registeredProperty = new RegisteredProperty(name, property);
            _properties.Add(registeredProperty);
            property.PropertyChanged += OnPropertyChanged;

            registeredProperty.TryAddCollectionListener(AddCollectionChange);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChanges)
                return;

            IsChanged = true;
            var name = _properties.GetName((IProperty) sender);

            _changes.Add(new ValuePropertyChange(name, e.OldValue, e.NewValue));
        }

        private void AddCollectionChange(CollectionPropertyChange change)
        {
            if (_ignoreChanges)
                return;
                
            _changes.Add(change);
            IsChanged = true;
        }
        
        public void CommitChanges()
        {
            IsChanged = false;
            _changes.Clear();
        }

        /// <summary>
        /// Like commit changes but does the same for each <see cref="IUndoable"/> property value recursive.
        /// </summary>
        public void CommitChangesTree()
        {
            CommitChanges();

            foreach (var property in _properties.GetProperties())
            {
                var value = property.GetValue();
                CommitChangesTree(value);
            }
        }

        private void CommitChangesTree(object value)
        {
            if (value is IUndoable undoable)
                undoable.PropertyRegistry.CommitChangesTree();
            else if (value is IMedjaObservableCollection list)
            {
                foreach (var item in list)
                    CommitChangesTree(item);
            }
        }

        public void ApplySnapshot(IReadOnlyList<PropertyChange> changes)
        {
            foreach (var change in changes)
            {
                var registeredProperty = _properties.GetByName(change.Name);
                var property = registeredProperty.Property;
                
                ApplyChange(property, change);
            }
        }

        private void ApplyChange(IProperty property, PropertyChange change)
        {
            if (change is ValuePropertyChange vpc)
                property.SetValue(vpc.NewValue);
            else if (change is CollectionPropertyChange cpc)
            {
                var value = property.GetValue() as IMedjaObservableCollection;

                if (value == null)
                    throw new InvalidOperationException("Cannot apply collection property change");

                ApplyCollectionPropertyChange(value, cpc);
            }
            else if (change is SubPropertyChanges spc)
            {
                var registry = (property.GetValue() as IUndoable).PropertyRegistry;

                foreach (var subChange in spc.Changes)
                    ApplyChange(registry._properties.GetByName(subChange.Name).Property, subChange);
            }
            else if (change is SubListItemChanges slic)
            {
                var list = property.GetValue() as IMedjaObservableCollection;
                var registry = (list[slic.Index] as IUndoable).PropertyRegistry;
                
                foreach(var subChange in slic.Changes)
                    ApplyChange(registry._properties.GetByName(subChange.Name).Property, subChange);
            }
        }

        private void ApplyCollectionPropertyChange(IMedjaObservableCollection value, CollectionPropertyChange cpc)
        {
            switch (cpc.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    value.Insert(cpc.Index, cpc.Item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    value.RemoveAt(cpc.Index);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    value.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cpc.Action));
            }
        }

        /// <summary>
        /// Gets the last relevant changes (aggregates multiple assignments and returns changes from sub objects)
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<PropertyChange> GetChangesTreeSnapshot()
        {
            var aggregatesChanges = new List<PropertyChange>();

            foreach (var change in _changes)
            {
                if (change is ValuePropertyChange)
                {
                    var index = aggregatesChanges.FindIndex(p => p.Name == change.Name);

                    if (index == -1)
                    {
                        aggregatesChanges.Add(change);
                    }
                    else
                    {
                        aggregatesChanges[index] = change;
                    }
                }
                else if (change is CollectionPropertyChange)
                {
                    aggregatesChanges.Add(change);
                }
            }
            
            var result = new List<PropertyChange>();

            foreach (var change in aggregatesChanges)
            {
                result.Add(change);
            }

            // all properties not yet changed since registered but their objects contain changes
            foreach (var registeredProperty in _properties.GetRegisteredProperties())
            {
                result.AddRange(GetSubChanges(registeredProperty.Name));
            }

            return result;
        }

        private IEnumerable<PropertyChange> GetSubChanges(string propertyName)
        {
            var value = _properties.GetByName(propertyName).Property.GetValue();
            return GetSubChanges(propertyName, value);
        }

        private PropertyChange GetSubChanges(PropertyChange change)
        {
            IReadOnlyList<PropertyChange> objChanges = null;

            if (change is ValuePropertyChange vpc)
            {
                if (vpc.NewValue is IUndoable undoable)
                    objChanges = undoable.PropertyRegistry.GetChangesTreeSnapshot();
            }
            else if (change is CollectionPropertyChange cpc)
            {
                if(cpc.Action == NotifyCollectionChangedAction.Add && cpc.Item is IUndoable undoable)
                    objChanges = undoable.PropertyRegistry.GetChangesTreeSnapshot();
            }
            
            return objChanges != null ? new SubPropertyChanges(change.Name, objChanges) : null;
        }

        private IEnumerable<PropertyChange> GetSubChanges(string name, object value)
        {
            if (value == null)
                yield break;

            if (value is IUndoable undoable)
            {
                var snapshot = undoable.PropertyRegistry.GetChangesTreeSnapshot();

                if (snapshot.Count > 0)
                    yield return new SubPropertyChanges(name, snapshot);
            }
            else if (value is IMedjaObservableCollection collection)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    var item = collection[i];
                    
                    if (item is IUndoable itemUndoable)
                    {
                        var snapshot = itemUndoable.PropertyRegistry.GetChangesTreeSnapshot();
                        
                        if(snapshot.Count > 0)
                            yield return new SubListItemChanges(name, i, snapshot);
                    }
                }
            }
        }

        /// <summary>
        /// Undo the last registered change.
        /// </summary>
        public void UndoLastChange()
        {
            if (_changes.Count == 0)
                return;

            var lastChange = _changes[_changes.Count - 1];
            var property = _properties.GetByName(lastChange.Name).Property;
            
            _changes.RemoveAt(_changes.Count - 1);
            IsChanged = _changes.Count > 0;
            
            _ignoreChanges = true;
            
            if(lastChange is ValuePropertyChange valChange)
                property.SetValue(valChange.OldValue);
            else if (lastChange is CollectionPropertyChange colChange)
            {
                var collection = (IMedjaObservableCollection) property.GetValue();
                
                if(colChange.Action == NotifyCollectionChangedAction.Add)
                    collection.RemoveAt(colChange.Index);
                else if(colChange.Action == NotifyCollectionChangedAction.Remove)
                    collection.Insert(colChange.Index, colChange.Item);
                else if (colChange.Action == NotifyCollectionChangedAction.Reset)
                {
                    foreach (var item in colChange.Items)
                        collection.Add(item);
                }
                else
                    throw new NotImplementedException("Handling for undoing " + colChange.Action + " is not yet implemented.");
            }
            
            _ignoreChanges = false;
        }

        /// <summary>
        /// Undo all registered changes.
        /// </summary>
        public void UndoChanges()
        {
            while(_changes.Count > 0)
                UndoLastChange();
        }

        public void Dispose()
        {
            foreach(var property in _properties.GetProperties())
                property.PropertyChanged -= OnPropertyChanged;
            
            _properties.Dispose();
            _changes.Clear();
        }
    }
}