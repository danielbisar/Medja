using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Medja.OpenTk.Eval
{
    public class MenuEntry
    {
        public string Text { get; }

        public ICommand Command { get; }

        public MenuEntry(string text, ICommand command = null)
        {
            Text = text;
            Command = command;
        }        
    }
}
