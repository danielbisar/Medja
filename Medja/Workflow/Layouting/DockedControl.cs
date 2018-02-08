using System;
using System.Collections.Generic;
using System.Text;
using Medja.Controls;

namespace Medja.Layouting
{
    public class DockedControl
    {
        public Control Control { get; set; }
        public Dock Dock { get; set; }

        public float MinWidth { get; set; }
        public float MinHeight { get; set; }
    }
}
