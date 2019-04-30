using System.Runtime.InteropServices;

namespace Medja.NativeInterfaces
{
    [StructLayout(LayoutKind.Sequential)]
    struct medja_system_info
    {
        public int screen_count;
            
        [MarshalAs(UnmanagedType.ByValArray)]
        public medja_screen[] screens;
    }
}