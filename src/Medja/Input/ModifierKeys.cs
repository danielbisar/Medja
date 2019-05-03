using System;

namespace Medja.Input
{
    [Flags]
    public enum ModifierKeys
    {
        None = 0,
        
        LeftShift = 1,
        RightShift = 1 << 1,

        LeftCtrl = 1 << 2,
        RightCtrl = 1 << 3,
        
        LeftAlt = 1 << 4,
        RightAlt = 1 << 4,

        LeftSuper = 1 << 5,
        RightSuper = 1 << 6
    }
}