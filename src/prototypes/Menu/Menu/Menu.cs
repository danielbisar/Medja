using System.Collections.Generic;

namespace Menu
{
    public class Menu
    {
        private readonly List<MenuItem> _items;

        public Menu()
        {
            _items = new List<MenuItem>();
        }

        public IReadOnlyList<MenuItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Adds a new top level menu item.
        /// </summary>
        /// <param name="title">The title of the item.</param>
        /// <returns>The new item.</returns>
        public MenuItem Add(string title)
        {
            var result = new MenuItem(title);
            _items.Add(result);

            return result;
        }

        public MenuItem Add(Command command)
        {
            var result = Add(command.Title);

            result.Command = command;

            return result;
        }
    }
}