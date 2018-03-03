using System;
using System.Collections.Generic;
using System.Text;
using Medja.Controls;
using Medja.Primitives;

namespace Medja.Theming
{
    public class DefaultTheme : ControlFactory
    {
        protected override Button CreateButton()
        {
            var result = base.CreateButton();
            result.Background = new Color(0.01f, 0.3f, 0.5f);

            return result;
        }
    }
}
