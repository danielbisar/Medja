using System.Collections.Generic;
using System.Text;
using Medja.Properties;

namespace Menu
{
    public class MenuItem
    {
        private readonly List<MenuItem> _children;

        public readonly Property<Command> CommandProperty;

        public readonly Property<bool> HasChildrenProperty;

        public readonly Property<string> TitleProperty;

        public MenuItem()
            : this(string.Empty)
        {
        }

        public MenuItem(string title, MenuItem parent = null)
        {
            HasChildrenProperty = new Property<bool>();
            _children = new List<MenuItem>();
            CommandProperty = new Property<Command>();
            Parent = parent;
            TitleProperty = new Property<string>();
            TitleProperty.SetSilent(title);
        }

        public IReadOnlyList<MenuItem> Children
        {
            get { return _children; }
        }

        public bool HasChildren
        {
            get => HasChildrenProperty.Get();
            set => HasChildrenProperty.Set(value);
        }

        public Command Command
        {
            get => CommandProperty.Get();
            set => CommandProperty.Set(value);
        }

        public string Title
        {
            get => TitleProperty.Get();
            set => TitleProperty.Set(value);
        }

        public MenuItem Parent { get; }

        /// <summary>
        /// Adds a child item.
        /// </summary>
        /// <param name="title">The title of the child item.</param>
        /// <returns>The new child item.</returns>
        public MenuItem Add(string title)
        {
            var result = new MenuItem(title, this);

            _children.Add(result);
            HasChildren = true;

            return result;
        }

        public MenuItem Add(Command command)
        {
            var result = Add(command.Title);

            result.Command = command;

            return result;
        }

        /// <summary>
        /// Gets the path for the menu item.
        /// </summary>
        /// <param name="separator">The path separator - default is '.'.</param>
        /// <returns>The path to this item.</returns>
        public string GetPath(string separator = ".")
        {
            var sb = new StringBuilder();
            var items = new Stack<MenuItem>();
            var item = this;

            while (item != null)
            {
                items.Push(item);
                item = item.Parent;
            }

            while (items.Count > 0)
            {
                sb.Append(items.Pop().Title);

                if (items.Count > 0)
                    sb.Append(separator);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return GetType().FullName + ": " + GetPath();
        }
    }
}