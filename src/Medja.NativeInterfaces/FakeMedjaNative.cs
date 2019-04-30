namespace Medja.NativeInterfaces
{
    /// <summary>
    /// Implements just enough so we still might be able to run the application.
    /// </summary>
    class FakeMedjaNative : IMedjaNative
    {
        public void CheckIfLibIsAvailable()
        {
            // just does nothing
        }

        public MedjaSystemInfo GetSystemInfo()
        {
            var screens = new MedjaScreen[1];
            screens[0] = new MedjaScreen(1, 1);
            
            return new MedjaSystemInfo(screens);
        }
    }
}