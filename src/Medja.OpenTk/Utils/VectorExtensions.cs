using System.Collections.Generic;
using OpenTK;

namespace Medja.OpenTk.Utils
{
    public static class VectorExtensions
    {
        public static IEnumerable<float> Iterate(this Vector3 v)
        {
            yield return v.X;
            yield return v.Y;
            yield return v.Z;
        }
    }
}