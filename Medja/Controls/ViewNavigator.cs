using System;
using System.Collections.Generic;

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
            PropertyCurrentView.PropertyChanged += OnCurrentViewChanged;
        }

        public void SetView(TEnum value, Control control)
        {
            var actualControl = control ?? new Control();
            _enumToViewMap[value] = actualControl;

            if (Equals(CurrentView, value))
                Content = actualControl;
        }
        
        private void OnCurrentViewChanged(object sender, PropertyChangedEventArgs e)
        {
            if(Content != null && Content is INavigationView navigationView)
                navigationView.OnHideView();
            
            Content = _enumToViewMap[CurrentView];
            
            if(Content != null && Content is INavigationView shownNavigationView)
                shownNavigationView.OnShowView();
        }
    }
}