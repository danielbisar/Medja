using System;
using Medja.Controls;
using Medja.Properties;
using Medja.Utils.Collections.Generic;

namespace Medja.Binding
{
    public static class ComboBoxEnumBinding
    {
        /// <summary>
        /// Creates a binding to an Enum.
        /// </summary>
        /// <param name="comboBox">The ComboBox to bind to.</param>
        /// <param name="property">The property to sync with the selected item.</param>
        /// <param name="toString">Translation function for the enum values.</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>The binding. Should be Disposed if you don't need it anymore.</returns>
        public static ComboBoxEnumBinding<T> BindEnum<T>(
            this ComboBox comboBox, 
            Property<T> property, 
            Func<T, string> toString = null) 
            where T : Enum
        {
            var result = new ComboBoxEnumBinding<T>(comboBox, property);
            result.Translate(toString);
            result.CreateItems();
            result.Bind();
            
            return result;
        }
    }
    
    public class ComboBoxEnumBinding<T> : IDisposable
        where T : Enum
    {
        private readonly ComboBox _comboBox;
        private readonly Property<T> _property;
        private readonly Map<string, T> _map;

        public ComboBoxEnumBinding(ComboBox comboBox, Property<T> property)
        {
            _comboBox = comboBox;
            _property = property;
            _map = new Map<string, T>();
        }

        /// <summary>
        /// Creates the actual binding. You should call <see cref="CreateItems"/> before.
        /// </summary>
        public void Bind()
        {
            if(_map.Count == 0 || _comboBox.ItemsPanel.Children.Count == 0)
                throw new InvalidOperationException($"Call first CreateItems");
            
            _comboBox.PropertySelectedItem.PropertyChanged += OnSelectedItemChanged;
            _property.PropertyChanged += OnPropertyChanged;
            
            _comboBox.SelectItem(_map.GetKey(_property.Get()));
        }

        private void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            var menuItem = e.NewValue as MenuItem;
            var value = default(T);
            
            if(menuItem != null)
                value = _map.GetValue(menuItem.Title);
            
            _property.Set(value);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!(e.NewValue is T))
                return;
            
            var value = (T) e.NewValue;
            var text = _map.GetKey(value);
            
            _comboBox.SelectItem(text);
        }
        
        /// <summary>
        /// Create the items. Call <see cref="Translate"/> first if you want to localize the items.
        /// </summary>
        public void CreateItems()
        {
            if (_map.Count == 0)
                Translate();
            
            _comboBox.Clear();

            foreach (var kvp in _map.GetKeyAndValues())
                _comboBox.Add(kvp.Key);
        }

        /// <summary>
        /// Translates all enum values. Can be used for localization.
        /// </summary>
        /// <param name="toString">Returns the string that should be displayed in the <see cref="ComboBox"/>.</param>
        public void Translate(Func<T, string> toString = null)
        {
            if (toString == null)
                toString = p => p.ToString();

            _map.Clear();

            foreach (var value in Enum.GetValues(typeof(T)))
            {
                var tValue = (T) value;
                _map.Add(toString(tValue), tValue);
            }
        }

        /// <summary>
        /// Gets the title stored for the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The title.</returns>
        public string GetTitle(T value)
        {
            return _map.GetKey(value);
        }

        public void Dispose()
        {
            _comboBox.PropertySelectedItem.PropertyChanged -= OnSelectedItemChanged;
            _property.PropertyChanged -= OnPropertyChanged;
        }
    }
}