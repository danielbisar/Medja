using System.Collections.Generic;
using OpenTK;

namespace Medja.OpenTk.Utils
{
    public static class ListExtensions
    {
        public static void Add(this ICollection<Vector3> list, float x, float y, float z)
        {
            list.Add(new Vector3(x,y,z));
        }

        public static void Add(this ICollection<float> list, Vector3 v)
        {
            list.Add(v.X);
            list.Add(v.Y);
            list.Add(v.Z);
        }
    }
}