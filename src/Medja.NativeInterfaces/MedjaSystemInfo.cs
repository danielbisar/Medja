using System.Collections.Generic;

namespace Medja.NativeInterfaces
{
    public class MedjaSystemInfo
    {
        public static MedjaSystemInfo Default { get; }

        static MedjaSystemInfo()
        {
            var info = new MedjaNative.medja_system_info();
            var screens = new List<MedjaScreen>();

            if(MedjaNative.medja_get_system_info(ref info) == 0)
            {
                for(int i = 0; i < info.screen_count; i++)
                    screens.Add(new MedjaScreen(info.screens[i].x_scale, info.screens[i].y_scale));
            }

            Default = new MedjaSystemInfo(screens);
        }

        public IReadOnlyList<MedjaScreen> Screens { get; }

        internal MedjaSystemInfo(List<MedjaScreen> screens)
        {
            Screens = screens;
        }
    }
}
