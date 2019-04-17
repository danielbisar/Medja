using System.Diagnostics;
using System.Reflection;

namespace Medja.Utils.Reflection
{
    /// <summary>
    /// Extensions for <see cref="Assembly"/>.
    /// </summary>
    public static class AssemblyExtensions
    {
        public static bool IsAssemblyOptimized(this Assembly assembly)
        {
            var debuggableAttribute = assembly.GetCustomAttribute<DebuggableAttribute>();
            return debuggableAttribute != null && debuggableAttribute.IsJITOptimizerDisabled == false;
        }
    }
}