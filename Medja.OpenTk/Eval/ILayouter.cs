using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.OpenTk.Eval
{
    interface ILayouter
    {
        Layout GetLayout(PositionInfo positionInfo);
    }
}
