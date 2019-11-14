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

        // usage of print methods allows easier exchange of output
        // without fully abstracting it now
        private static void PrintMenu(Menu menu)
        {
            PrintMenuItems("", menu.Items);
        }

        private static void Print(string v)
        {
            Console.Write(v);
        }

        private static void PrintSur(string v1, string v2, string v3)
        {
            Console.Write(v1 + v2 + v3);
        }

        // INN = If Not Null
        private static void PrintINN(string value)
        {
            if (value != null)
                Console.Write(value);
        }

        private static void PrintSurINN(string v1, string v2)
        {
            if (v1 != null && v2 != null)
                PrintSur(v2, v1, v2);
        }

        private static void PrintSurINN(string v1, string v2, string v3)
        {
            if (v1 != null && v2 != null && v3 != null)
                PrintSur(v1, v2, v3);
        }

        private static void PrintLn()
        {
            Console.WriteLine();
        }

        private static void PrintLn(string value)
        {
            Console.WriteLine(value);
        }

        private static void PrintMenuItems(string indention, IEnumerable<MenuItem> items)
        {
            foreach (var item in items)
            {
                Print(indention + "{ ");
                PrintSur("\"", item.Title, "\"");

                if (item.Command != null)
                {
                    PrintSurINN(item.Command.KeyboardShortcut, ", ");
                    PrintSurINN("\"", item.Command.Description, "\"");
                }

                if (item.HasChildren)
                {
                    PrintLn();
                    PrintMenuItems(indention + "    ", item.Children);
                    PrintLn(indention + "}");
                }
                else
                    PrintLn(" }");
            }
        }
    }
}