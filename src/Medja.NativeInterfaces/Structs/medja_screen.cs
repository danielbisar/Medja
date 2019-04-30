using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace Medja.NativeInterfaces
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct medja_screen
    {
        public float x_scale;
        public float y_scale;
    }
}