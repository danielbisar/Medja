using System;
using Medja.Controls;
using Medja.Primitives;

namespace Medja.Theming
{
    /// <summary>
    /// Interface for ControlFactories.
    /// </summary>
    public interface IControlFactory
    {
        Font DefaultFont { get; }
        
        TControl Create<TControl>() where TControl : Control;
        TControl Create<TControl>(Action<TControl> applyCustomStyle) where TControl : Control;
        
        bool HasControl<TControl>() where TControl : Control;
        bool HasControl(Type type);

        void ComboBoxMenuItemStyle(MenuItem menuItem);
    }
}