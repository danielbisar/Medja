using System;
using System.Collections.Generic;
using Medja.Controls;
using Medja.Properties;
using Medja.Utils.Collections.Generic;

namespace Medja
{
    public static class LayoutExtensions
    {
        private static readonly int MarkLayoutDirtyHelpersId = AttachedPropertyIdFactory.NewId();
        
        /// <summary>
        /// Tells the control that the layout needs to be updated every time given property changes.
        /// </summary>
        /// <param name="property">The property that affects the layout.</param>
        /// <param name="control">The control that is affected by the layout. This should be the control
        /// containing the property.</param>
        public static void AffectsLayoutOf(this IProperty property, Control control)
        {
            var result = new MarkLayoutDirtyOnPropertyChanged(control, property);

            var markDirtyHelpersProperty = (Property<Dictionary<IProperty, IDisposable>>)control.AttachedProperties
                .GetOrAdd(MarkLayoutDirtyHelpersId, CreateLayoutDirtyHelpersProperty);
            var markDirtyHelpers = markDirtyHelpersProperty.Get();
            
            markDirtyHelpers.Add(property, result);
        }

        private static Property<Dictionary<IProperty, IDisposable>> CreateLayoutDirtyHelpersProperty(int attachedPropertyId)
        {
            var result = new Property<Dictionary<IProperty, IDisposable>>();
            result.SetSilent(new Dictionary<IProperty, IDisposable>());

            return result;
        }

        public static void RemoveAffectsLayoutOf(this IProperty property, Control control)
        {
            var markDirtyHelpers = control.AttachedProperties.Get<Dictionary<IProperty, IDisposable>>(MarkLayoutDirtyHelpersId);
            var helper = markDirtyHelpers[property];

            helper.Dispose();
            markDirtyHelpers.Remove(property);
        }
    }
}