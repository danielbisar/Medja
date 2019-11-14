using System;
using System.Collections.Generic;

namespace Menu
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var menu = new Menu();
            var file = menu.Add("File");
            
            file.Add(new Command("Open", "Ctrl+o", "Open a file."));
            file.Add("Save");
            file.Add("Exit");
            
            menu.Add("Session");
            menu.Add("Spaced title");
            
            PrintMenu(menu);
        }

        private static void PrintMenu(Menu menu)
        {
            PrintMenuItems("", menu.Items);
        }

        private static void PrintMenuItems(string indention, IEnumerable<MenuItem> items)
        {
            foreach (var item in items)
            {
                Console.Write(indention + "{ ");
                Console.Write(item.Title);

                if (item.HasChildren)
                {
                    Console.WriteLine();
                    PrintMenuItems(indention + "    ", item.Children);
                    Console.WriteLine(indention + "}");
                }
                else
                    Console.WriteLine(" }");
            }
        }
    }
}