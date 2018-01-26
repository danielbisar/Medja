using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.OpenTk.Eval
{
    public class Menu
    {
        public string Id { get; }
        public List<MenuEntry> Items { get; }
        
        public Menu(string id)
        {
            Id = id;
            Items = new List<MenuEntry>();
        }
    }
}
