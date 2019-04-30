using System;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Medja.NativeInterfaces
{
    internal class MedjaNative : IMedjaNative
    {
        public void CheckIfLibIsAvailable()
        {
            // main purpose: make the runtime try to load the native lib
            CommonExternFunctions.medja_native_check();
        }

        public MedjaSystemInfo GetSystemInfo()
        {
            var info = new medja_system_info();

            if(CommonExternFunctions.medja_get_system_info(ref info) == 0)
            {
                var screens = new MedjaScreen[info.screen_count];
                
                for(int i = 0; i < info.screen_count; i++)
                    screens[i] = new MedjaScreen(info.screens[i].x_scale, info.screens[i].y_scale);

                return new MedjaSystemInfo(screens);
            }
            
            throw new Exception($"Error calling {nameof(CommonExternFunctions.medja_get_system_info)}.");
        }
    }
}
