namespace Medja.Utils
{
    public static class ByteSize
    {
        /// <summary>
        /// Gets a byte size in a human readable format with the abbreaviation that makes the most sense (KB, MB, ... up to EB).
        /// Based on https://www.somacon.com/p576.php
        /// Found at: https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetHumanReadable(long size)
        {
            // Get absolute value
            long absoluteI = (size < 0 ? -size : size);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absoluteI >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (size >> 50);
            }
            else if (absoluteI >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (size >> 40);
            }
            else if (absoluteI >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (size >> 30);
            }
            else if (absoluteI >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (size >> 20);
            }
            else if (absoluteI >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (size >> 10);
            }
            else if (absoluteI >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = size;
            }
            else
            {
                return size.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.## ") + suffix;
        }
    }
}