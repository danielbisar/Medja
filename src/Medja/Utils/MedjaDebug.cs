using System;
using System.IO;
using Medja.Controls;

namespace Medja.Utils
{
    public static class MedjaDebug
    {
        /// <summary>
        /// After each arrange call prints out the control tree.
        /// </summary>
        /// <param name="control"></param>
        public static void DebugLayout(this Control control, TextWriter target = null)
        {
            if(target == null)
                target = Console.Out;
            
            control.Arranged += (s, e) => target.WriteLine(new ControlTreeStringBuilder((Control) s).GetTree());
        }
    }
}