using System;
using System.Collections.Generic;
using System.Text;

namespace Medja
{
    public static class MedjaMath
    {
        public static bool AboutEquals(this double value1, double value2)
        {
            double epsilon = Math.Max(Math.Abs(value1), Math.Abs(value2)) * 1E-15;
            return Math.Abs(value1 - value2) <= epsilon;
        }
    }
}
