namespace Medja.Utils.Text
{
    public static class Digits
    {
        public static int GetDigitCount(uint n)
        {
            if (n == 0 || n < 10)
                return 1;

            return (int) System.Math.Log10(n) + 1;
        }
    }
}