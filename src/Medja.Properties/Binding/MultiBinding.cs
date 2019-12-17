using System;
using System.Collections.Generic;

namespace Medja.Properties.Binding
{
    public class MultiBinding<TTarget> : IBinding
    {
        private readonly Property<TTarget> _targetProperty;
        
        private readonly List<IProperty> _sourceProperties;
        /// <summary>
        /// The source properties. Use <see cref="AddSource"/> to add more.
        /// </summary>
        public IReadOnlyList<IProperty> SourceProperties
        {
            get => _sourceProperties;
        }
        
        public Func<IReadOnlyList<IProperty>, TTarget> Converter { get; }
        
        public MultiBinding(Property<TTarget> targetProperty, Func<IReadOnlyList<IProperty>, TTarget> converter)
        {
            _sourceProperties = new List<IProperty>();
            _targetProperty = targetProperty ?? throw new ArgumentNullException(nameof(targetProperty));
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        /// <summary>
        /// Adds a new source property (listens for changes).
        /// </summary>
        /// <param name="property">The property.</param>
        public void AddSource(IProperty property)
        {
            _sourceProperties.Add(property);
            property.PropertyChanged += OnSourcePropertyChanged;
        }

        /// <summary>
        /// Removes a source property. 
        /// </summary>
        /// <param name="property">The property to remove.</param>
        /// <returns>returns true if the property was removed, else false.</returns>
        public bool RemoveSource(IProperty property)
        {
            var removed = _sourceProperties.Remove(property);

            if (removed)
                property.PropertyChanged -= OnSourcePropertyChanged;

            return removed;
        }
        
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            _targetProperty.Set(Converter(_sourceProperties));
        }

        public void Dispose()
        {
            foreach (var property in _sourceProperties)
                property.PropertyChanged -= OnSourcePropertyChanged;
            
            _sourceProperties.Clear();
        }
    }
}