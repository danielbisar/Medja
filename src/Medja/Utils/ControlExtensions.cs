using System;
using System.Collections.Generic;
using Medja.Binding;
using Medja.Primitives;

namespace Medja.Controls
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Positions the control relative to the bottom of the orientationControl.
        /// </summary>
        /// <param name="control">The control to position.</param>
        /// <param name="orientationControl">The control to use for orientation.</param>
        /// <returns>The disposable binding objects.</returns>
        public static IEnumerable<IDisposable> AutoPosRelativeToBottomOf(this Control control, Control orientationControl)
        {
            var position = control.Position;
            var orientationPosition = orientationControl.Position;
            
            var xBinding = position.PropertyX.BindTo(orientationPosition.PropertyX);
            
            var multiBinding = new MultiSourcesToTargetBinding<MRect, float>(position, position.PropertyY, 
                pos => orientationPosition.Y + orientationPosition.Height);
            multiBinding.AddSource(orientationPosition.PropertyY);
            multiBinding.AddSource(orientationPosition.PropertyHeight);
            
            yield return xBinding;
            yield return multiBinding;
        }
        
        /// <summary>
        /// Toggles if the control is visible or not.
        /// </summary>
        /// <param name="control">The control to update.</param>
        /// <param name="hiddenVisibility">The visibility value used to "hide" the control.</param>
        /// <returns>If the control is visible or not.</returns>
        public static bool ToggleVisibility(this Control control, Visibility hiddenVisibility = Visibility.Hidden)
        {
            control.Visibility = control.IsVisible 
                ? hiddenVisibility
                : Visibility.Visible;
            
            return control.IsVisible;
        }
        
        

        
    }
}