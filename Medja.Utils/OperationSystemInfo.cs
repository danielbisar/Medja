using System;

namespace Medja.Utils
{
    public class OperationSystemInfo
    {
        public static OperationSystem GetCurrentOperationSystem()
        {
            var platformId = (int)Environment.OSVersion.Platform;

            if (platformId == 4 || platformId == 6 || platformId == 128)
                return OperationSystem.Linux;
		
            return OperationSystem.Windows;
        }
    }
}