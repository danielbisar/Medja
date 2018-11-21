using System;
using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
    /// <summary>
    /// Allows switching of views via an enum value.
    /// </summary>
    /// <typeparam name="TEnum">The Enum type.</typeparam>
    public class ViewNavigator<TEnum> : ContentControl
        where TEnum: struct, Enum
    {
        private readonly Dictionary<TEnum, Control> _enumToViewMap;

        public readonly Property<TEnum> PropertyCurrentView;
        /// <summary>
        /// The current view.
        /// </summary>
        public TEnum CurrentView
        {
            get { return PropertyCurrentView.Get(); }
            set { PropertyCurrentView.Set(value); }
        }
        
        public ViewNavigator()
        {
            _enumToViewMap = new Dictionary<TEnum, Control>();
            
            PropertyCurrentView = new Property<TEnum>();
            PropertyCurrentView.PropertyChanged += OnPropertyChanged;
        }

        public void SetView(TEnum value, Control control)
        {
            var actualControl = control ?? new Control();
            _enumToViewMap[value] = actualControl;

            if (Equals(CurrentView, value))
                Content = actualControl;
        }
        
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Content = _enumToViewMap[CurrentView];
        }
    }
}