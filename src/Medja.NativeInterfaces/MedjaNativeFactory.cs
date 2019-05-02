using System;
using System.Diagnostics;

namespace Medja.NativeInterfaces
{
    public static class MedjaNativeFactory
    {
        public static IMedjaNative Create()
        {
            try
            {
                var result = new MedjaNative();
                result.CheckIfLibIsAvailable();

                return result;
            }
            catch (DllNotFoundException dllNotFoundException)
            {
                Trace.TraceWarning("Medja.NativeInterfaces: Could not find DLL or module " 
                                   + dllNotFoundException.Message + Environment.NewLine 
                                   + "Medja.NativeInterfaces: Switch to fake implementation.");

                return new FakeMedjaNative();
            }
        }
    }
}