using System;
using Medja.Controls;
using Medja.Properties;

namespace Medja.Utils
{
    /// <summary>
    /// Allows selection of only one control from a list of controls.
    /// </summary>
    /// <typeparam name="TControl">The controls type.</typeparam>
    /// <remarks>
    /// Selection is controlled via a property of the control. Most of this will be named PropertyIsSelected or
    /// something similar.
    /// </remarks>
    public class ExclusiveOrSelector<TControl>
        where TControl : Control
    {
        private readonly Func<TControl, Property<bool>> _getIsSelectedProperty;
        private TControl _selectedControl;

        public ExclusiveOrSelector(Func<TControl, Property<bool>> getIsSelectedProperty)
        {
            _getIsSelectedProperty =
                getIsSelectedProperty ?? throw new ArgumentNullException(nameof(getIsSelectedProperty));
        }

        /// <summary>
        /// Adds a control to the list.
        /// </summary>
        /// <param name="control"></param>
        public void Add(TControl control)
        {
            var property = _getIsSelectedProperty(control);

            if (property == null)
                throw new NullReferenceException("Property is null");

            property.PropertyChanged += (s, e) => OnPropertyChanged(control, e);
        }

        private void OnPropertyChanged(TControl control, PropertyChangedEventArgs e)
        {
            var newValue = (bool) e.NewValue;

            if (!newValue) 
                return;
            if (ReferenceEquals(control, _selectedControl)) 
                return;
            
            if (_selectedControl != null)
                _getIsSelectedProperty(_selectedControl).Set(false);

            _selectedControl = control;
            _getIsSelectedProperty(_selectedControl).Set(true);
        }
    }
}