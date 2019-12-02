using System;
using System.Collections.Generic;
using Medja.Primitives;
using Medja.Properties;

namespace Medja.Controls
{
    /// <summary>
    /// A container that layouts the Content and just shows the popup (does not move it).
    /// </summary>
    public class PopupContainer : ContentControl
    {
        [NonSerialized] 
        public readonly Property<Popup> PropertyPopup;
        /// <summary>
        /// Gets or sets the <see cref="Popup"/>.
        /// </summary>
        /// <remarks>Visibility of the Popup is controlled via its <see cref="Popup.Visibility"/> property.</remarks>
        public Popup Popup
        {
            get { return PropertyPopup.Get(); }
            set { PropertyPopup.Set(value); }
        }
        
        public PopupContainer()
        {
            PropertyPopup = new Property<Popup>();
            PropertyPopup.AffectsLayout(this);
            PropertyPopup.PropertyChanged += OnPopupChanged;
        }

        protected virtual void OnPopupChanged(object sender, PropertyChangedEventArgs e)
        {
            var oldPopup = (Popup) e.OldValue;

            if (oldPopup != null)
                oldPopup.Parent = null;

            var newPopup = (Popup) e.NewValue;

            if (newPopup != null)
            {
                newPopup.Parent = this;
                
                if (newPopup.IsVisible)
                    newPopup.Visibility = Visibility.Collapsed;
            }
        }

        public override IEnumerable<Control> GetChildren()
        {
            foreach(var child in base.GetChildren())
                yield return child;
            
            if(Popup != null && Popup.IsVisible)
                yield return Popup;
        }

        protected override void Dispose(bool disposing)
        {
            Popup?.Dispose();
            base.Dispose(disposing);
        }
    }
}