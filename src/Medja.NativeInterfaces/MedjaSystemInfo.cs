namespace Medja.NativeInterfaces
{
    public class MedjaSystemInfo
    {
        public MedjaScreen[] Screens { get; }

        internal MedjaSystemInfo(MedjaScreen[] screens)
        {
            Screens = screens;
        }
    }
}
