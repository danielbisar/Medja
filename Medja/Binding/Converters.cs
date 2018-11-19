namespace Medja
{
    public static class Converters
    {
        public static int ToInt(string value)
        {
            if (int.TryParse(value, out var intVal))
                return intVal;

            return 0;
        }

        public static uint ToUint(string value)
        {
            if (uint.TryParse(value, out var uintVal))
                return uintVal;

            return 0;
        }
    }
}