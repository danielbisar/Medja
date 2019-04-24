using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Medja.NativeInterfaces
{
    internal class MedjaNative
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct medja_screen
        {
            public float x_scale;
            public float y_scale;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct medja_system_info
        {
            public int screen_count;
            
            [MarshalAs(UnmanagedType.ByValArray)]
            public medja_screen[] screens;
        }

        [DllImport("medja.so")]
        internal static extern int medja_get_system_info(ref medja_system_info system_info);
    }
}