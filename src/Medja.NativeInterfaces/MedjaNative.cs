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

            var res = CommonExternFunctions.medja_float()[0];
            
            Console.WriteLine("Medja_float: "+ res.x_scale + ", " + res.y_scale);
        }

        public MedjaSystemInfo GetSystemInfo()
        {
            var info = new medja_system_info();

            if(CommonExternFunctions.medja_get_system_info(ref info) == 0)
            {
                Console.WriteLine("info.sceen_count: " + info.screen_count);
                
                var screens = new MedjaScreen[info.screen_count];
                
                Console.WriteLine("Screens.count: " + screens.Length);
                Console.WriteLine("Info.screen count: " + info.screen_count);

                for (int i = 0; i < info.screen_count; i++)
                {
                    Console.WriteLine("I="+ i);
                    var info_screen = info.screens[i];

                    Console.WriteLine("AFTER_INFO_SCREEN");
                    Console.WriteLine(info_screen.x_scale);
                    Console.WriteLine(info_screen.y_scale);
                    
                    screens[i] = new MedjaScreen(info_screen.x_scale, info_screen.y_scale);
                }
                

                return new MedjaSystemInfo(screens);
            }
            
            throw new Exception($"Error calling {nameof(CommonExternFunctions.medja_get_system_info)}.");
        }
    }
}
