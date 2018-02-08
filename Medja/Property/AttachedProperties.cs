using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Property
{
    public static class AttachedProperties
    {
        private static int _i = -1;

        public static int GetNextId()
        {
            return _i++;
        }
    }
}
