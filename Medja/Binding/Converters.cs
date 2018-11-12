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
    }
}