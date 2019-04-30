using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace Medja.NativeInterfaces
{
    /// <summary>
    /// External methods that are the same for all operation systems.
    /// </summary>
    static class CommonExternFunctions
    {
        [DllImport("medja")]
        internal static extern void medja_native_check();
        
        [DllImport("medja")]
        internal static extern int medja_get_system_info(ref medja_system_info system_info);
    }
}