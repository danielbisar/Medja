using System;
using Medja.Controls;
using Medja.Controls.Menu;
using Medja.Properties;
using Medja.Properties.Binding;
using Medja.Utils.Collections.Generic;

namespace Medja
{
    public static class ComboBoxEnumBinding
    {
        /// <summary>
        /// Creates a binding to an Enum.
        /// </summary>
        /// <param name="comboBox">The ComboBox to bind to.</param>
        /// <param name="property">The property that should receive the value of <see cref="ComboBox.SelectedItem"/>
        /// </param>
        /// <param name="toString">Translation function for the enum values.</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>The binding. Should be Disposed if you don't need it anymore.</returns>
        /// <example>
        /// comboBox.BindEnum(targetProperty, p => { /* translate enum values */ });
        /// </example>
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
    
    /// <summary>
    /// Represents a binding of a ComboBox to an Enum. 
    /// </summary>
    /// <typeparam name="TEnum">The Enum type.</typeparam>
    /// <remarks>For easier usage use <see cref="ComboBoxEnumBinding.BindEnum{T}"/>. See examples section of
    /// <see cref="ComboBoxEnumBinding.BindEnum{T}"/>.</remarks>
    public class ComboBoxEnumBinding<TEnum> : IBinding
        where TEnum : Enum
    {
        private readonly ComboBox _comboBox;
        private readonly Property<TEnum> _property;
        private readonly Map<string, TEnum> _map;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox"/> that should be filled with the Enums values.</param>
        /// <param name="property">The property that should receive the value of <see cref="ComboBox.SelectedItem"/>
        /// </param>
        public ComboBoxEnumBinding(ComboBox comboBox, Property<TEnum> property)
        {
            _comboBox = comboBox;
            _property = property;
            _map = new Map<string, TEnum>();
        }

        /// <summary>
        /// Creates the actual binding. Call <see cref="CreateItems"/> first.
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
            var value = default(TEnum);
            
            if(menuItem != null)
                value = _map.GetValue(menuItem.Title);
            
            _property.Set(value);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!(e.NewValue is TEnum))
                return;
            
            var value = (TEnum) e.NewValue;
            UpdateComboBox(value);
        }

        private void UpdateComboBox(TEnum value)
        {
            var text = _map.GetKey(value);

            _comboBox.SelectItem(text);
        }

        /// <summary>
        /// Create the <see cref="ComboBox"> items. Call <see cref="Translate"/>
        /// first if you want to localize the items.
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
        /// <param name="toString">Returns the string that should be displayed in the <see cref="ComboBox"/> based
        /// on the given Enum value.</param>
        /// <example>
        /// enum MyEnum { Value1, Value2 }
        /// 
        /// var binding = new ComboBoxEnumBinding&lt;MyEnum&gt;(targetProperty);
        /// binding.Translate(p =>
        /// {
        ///     switch(p)
        ///     {
        ///         case MyEnum.Value1: return "Wert 1";
        ///         case MyEnum.Value2: return "Wert 2";
        ///     }
        ///
        ///     return p.ToString(); // fallback
        /// });
        /// </example>
        public void Translate(Func<TEnum, string> toString = null)
        {
            if (toString == null)
                toString = p => p.ToString();

            _map.Clear();

            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                var tValue = (TEnum) value;
                _map.Add(toString(tValue), tValue);
            }
        }

        /// <summary>
        /// Gets the string value stored for the given enum value.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The translated string value.</returns>
        public string GetString(TEnum value)
        {
            return _map.GetKey(value);
        }

        public void Update()
        {
            var value = _property.Get();
            UpdateComboBox(value);
        }

        /// <summary>
        /// Disposes the binding.
        /// </summary>
        public void Dispose()
        {
            _comboBox.PropertySelectedItem.PropertyChanged -= OnSelectedItemChanged;
            _property.PropertyChanged -= OnPropertyChanged;
        }
    }
}
