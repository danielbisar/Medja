using System;
using System.Collections.Generic;
using Medja.Controls.Container;
using Medja.Properties;

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
            {
                InvokeOnHideView();
                Content = actualControl;
                InvokeOnShowView();
            }
        }
        
        private void OnCurrentViewChanged(object sender, PropertyChangedEventArgs e)
        {
            InvokeOnHideView();
            
            Content = _enumToViewMap[CurrentView];
            
            InvokeOnShowView();
        }

        private void InvokeOnHideView()
        {
            if(Content != null && Content is INavigationView navigationView)
                navigationView.OnHideView();
        }

        private void InvokeOnShowView()
        {
            if(Content != null && Content is INavigationView shownNavigationView)
                shownNavigationView.OnShowView();
        }
    }
}
