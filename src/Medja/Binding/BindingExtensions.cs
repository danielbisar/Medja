using Medja.Controls;
using Medja.Properties;

namespace Medja
{
    public static class BindingExtensions
    {
        /// <summary>
        /// Tells the control that the layout needs to be updated every time given property changes.
        /// </summary>
        /// <param name="property">The property that affects the layout.</param>
        /// <param name="control">The control that is affected by the layout. This should be the control
        /// containing the property.</param>
        /// <typeparam name="T">The properties value type.</typeparam>
        public static MarkLayoutDirtyOnPropertyChanged<T> AffectsLayout<T>(this Property<T> property, Control control)
        {
            return new MarkLayoutDirtyOnPropertyChanged<T>(control, property);
        }
    }
}