using System;
using Medja.Controls;

namespace Medja.Utils
{
    public static class MedjaDebug
    {
        public static void PrintControlTreeAfterArrange(Control control)
        {
            control.Arranged += (s, e) => Console.WriteLine(new ControlTreeStringBuilder((Control) s).GetTree());
        }
    }
}