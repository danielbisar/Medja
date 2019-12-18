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

        public readonly Property<TControl> PropertySelectedControl;
        /// <summary>
        /// Gets or sets the selected control.
        /// </summary>
        public TControl SelectedControl
        {
            get => PropertySelectedControl.Get();
            set => PropertySelectedControl.Set(value);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="getIsSelectedProperty">Returns the correct property from the control.</param>
        public ExclusiveOrSelector(Func<TControl, Property<bool>> getIsSelectedProperty)
        {
            PropertySelectedControl = new Property<TControl>();
            PropertySelectedControl.PropertyChanged += OnSelectedControlChanged;
            
            _getIsSelectedProperty =
                getIsSelectedProperty ?? throw new ArgumentNullException(nameof(getIsSelectedProperty));
        }

        private void OnSelectedControlChanged(object sender, PropertyChangedEventArgs e)
        {
            var oldControl = (TControl) e.OldValue;
            var newControl = (TControl) e.NewValue;

            Deselect(oldControl);
            Select(newControl);
        }

        private void Deselect(TControl control)
        {
            if (control != null)
                _getIsSelectedProperty(control).Set(false);
        }

        private void Select(TControl control)
        {
            if (control != null)
                _getIsSelectedProperty(control).Set(true);

            // does not call the changed handler again if we just
            // set it to the same value, but so this method
            // could be use anywhere else
            SelectedControl = control;
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

            property.PropertyChanged += (s, e) => OnIsSelectedChanged(control, e);
        }

        private void OnIsSelectedChanged(TControl control, PropertyChangedEventArgs e)
        {
            var newValue = (bool) e.NewValue;

            if (!newValue) 
                return;
            
            if (ReferenceEquals(control, SelectedControl)) 
                return;
            
            if (SelectedControl != null)
                _getIsSelectedProperty(SelectedControl).Set(false);

            SelectedControl = control;
        }
    }
}