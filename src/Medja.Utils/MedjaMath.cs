namespace Medja.Utils
{
    public static class MedjaMath
    {
        public const double PIby180 = System.Math.PI / 180;
        
        /// <summary>
        /// Compares two double values and returns if they are almost equals.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        /// <remarks>This method is useful due to the fact that double values are not always calculated exactly.
        /// </remarks>
        public static bool AboutEquals(this double value1, double value2)
        {
            var epsilon = System.Math.Max(System.Math.Abs(value1), System.Math.Abs(value2)) * 1E-15;
            return System.Math.Abs(value1 - value2) <= epsilon;
        }

        /// <summary>
        /// Compares two float values and returns if they are almost equals.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        /// <remarks>This method is useful due to the fact that float values are not always calculated exactly.
        /// </remarks>
        public static bool AboutEquals(this float value1, float value2)
        {
            var epsilon = System.Math.Max(System.Math.Abs(value1), System.Math.Abs(value2)) * 1E-15;
            return System.Math.Abs(value1 - value2) <= epsilon;
        }
        
        // TODO check alternative implementations / performance and cross-platform must be answered - see https://stackoverflow.com/questions/3874627/floating-point-comparison-functions-for-c-sharp
        /*
         * public static unsafe int FloatToInt32Bits( float f )
           {
           return *( (int*)&f );
           }
           
           public static bool AlmostEqual2sComplement( float a, float b, int maxDeltaBits )
           {
           int aInt = FloatToInt32Bits( a );
           if ( aInt < 0 )
           aInt = Int32.MinValue - aInt;
           
           int bInt = FloatToInt32Bits( b );
           if ( bInt < 0 )
           bInt = Int32.MinValue - bInt;
           
           int intDiff = Math.Abs( aInt - bInt );
           return intDiff <= ( 1 << maxDeltaBits );
           }
         */
        
        /// <summary>
        /// Allows calculation of modulo with negative x values.
        /// x mod m works for both positive and negative x (unlike x % m)
        /// </summary>
        /// <returns>The modulo.</returns>
        /// <param name="x">Left hand value for modulo.</param>
        /// <param name="m">Right hand value for modulo.</param>
        public static int ExtendedModulo(this int x, int m)
        {
            return (x % m + m) % m;
        }

        
        /// <summary>
        /// Calculates the radians from a degree angle.
        /// </summary>
        /// <param name="angle">Angle in degrees.</param>
        /// <returns>The radian equivalent.</returns>
        public static double Radians(this double angle)
        {
            return angle * PIby180;
        }

        /// <summary>
        /// Gets if the value is a power of two.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if the value is power of two. False if not.</returns>
        public static bool IsPowerOfTwo(this uint value)
        {
            return value != 0 && (value & ~value + 1) == value;
        }

        /// <summary>
        /// Gets the next higher power of two value.
        /// </summary>
        /// <param name="value">Any value > 0.</param>
        /// <returns></returns>
        /// <remarks>Source is from https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2Float released
        /// unto the public domain.</remarks>
        public static uint GetNextPowerOfTwo(this uint value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value++;
            
            return value;
        }
    }
}
