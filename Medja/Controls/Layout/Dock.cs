﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Controls
{
    public enum Dock
    {
        Top,
        Left,
        Right,
        Bottom,

        /// <summary>
        /// Fills the left-over area. This can be used only for the last control added to the DockPanel.
        /// </summary>
        Fill
    }
}
