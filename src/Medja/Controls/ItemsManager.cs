using System;
using System.Collections.Generic;
using System.Linq;
using Medja.Input;
using Medja.Properties;
using Medja.Theming;
using Medja.Utils;
using Medja.Utils.Collections.Generic;

namespace Medja.Controls
{
    public class ItemsManager<TItem>
     where TItem: class
    {
        private readonly Dictionary<MenuItem, TItem> _menuToItemMap;
        private readonly Dictionary<TItem, MenuItem> _itemToMenuMap;
        private readonly IControlFactory _controlFactory;

        public List<TItem> Items { get; }

        public Action<TItem, MenuItem> InitMenuItemFromItem { get; set; }
        
        public readonly Property<bool> PropertyIsSelectable;
        /// <summary>
        /// If true the control supports selection via click of a button.
        /// </summary>
        public bool IsSelectable
        {
            get { return PropertyIsSelectable.Get(); }
            set { PropertyIsSelectable.Set(value); }
        }
        
        public readonly Property<TItem> PropertySelectedItem;
        /// <summary>
        /// Gets or sets the selected item. You should also set <see cref="IsSelectable"/> to true.
        /// </summary>
        public TItem SelectedItem
        {
            get { return PropertySelectedItem.Get(); }
            set { PropertySelectedItem.Set(value); }
        }
        
        /// <summary>
        /// An event that is fired whenever any of the item buttons is clicked.
        /// </summary>
        public event EventHandler<ItemEventArgs<TItem>> ItemClicked;

        public event EventHandler<ItemEventArgs<TItem>> ItemAdded;
        public event EventHandler<ItemEventArgs<TItem>> ItemRemoved;
        
        public ItemsManager(IControlFactory controlFactory)
        {
            PropertyIsSelectable = new Property<bool>();
            PropertySelectedItem = new Property<TItem>();
            PropertySelectedItem.PropertyChanged += OnSelectedItemChanged;
            
            _controlFactory = controlFactory ?? throw new ArgumentNullException(nameof(controlFactory));;
            _menuToItemMap = new Dictionary<MenuItem, TItem>(new ReferenceEqualityComparer<MenuItem>());
            _itemToMenuMap = new Dictionary<TItem, MenuItem>(EqualityUtils.GetItemEqualityComparer<TItem>());
            
            Items = new List<TItem>();
            
            InitMenuItemFromItem = (item, button) =>
            {
                button.Title = item.ToString();
            };

            ItemAdded += (s, e) => { };
            ItemRemoved += (s, e) => { };
        }
       
        protected virtual void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            var oldSelectedItem = (TItem)e.OldValue;
            var newSelectedItem = (TItem)e.NewValue;

            if (oldSelectedItem != null)
            {
                // we need to use try, because the button might not be visible
                if(_itemToMenuMap.TryGetValue(oldSelectedItem, out var button))
                    button.IsSelected = false;
            }

            if (newSelectedItem != null)
            {
                // we need to use try, because the button might not be visible
                // if user sets it manually
                if(_itemToMenuMap.TryGetValue(newSelectedItem, out var button))
                    button.IsSelected = true;
            }
        }
        
        public void AddItem(TItem item)
        {
            Items.Add(item);

            var menuItem = CreateMenuItemForItem(item);
            _menuToItemMap.Add(menuItem, item);
            _itemToMenuMap.Add(item, menuItem);
            
            ItemAdded?.Invoke(this, new ItemEventArgs<TItem>(item, menuItem));
        }
        
        public void AddRange(IEnumerable<TItem> items)
        {
            foreach(var item in items)
                AddItem(item);
        }
        
        private MenuItem CreateMenuItemForItem(TItem item)
        {
            var result = _controlFactory.Create<MenuItem>();
            InitMenuItemFromItem(item, result);
            result.InputState.Clicked += OnMenuItemClicked;

            if (IsSelectable && ReferenceEquals(item, SelectedItem))
                result.IsSelected = true;

            return result;
        }
        
        private void OnMenuItemClicked(object sender, EventArgs e)
        {
            var inputState = sender as InputState;
            var menuItem = (MenuItem) inputState.Control;
			
            NotifyMenuItemClicked(menuItem);

            if (IsSelectable)
                SelectedItem = _menuToItemMap[menuItem];
        }

        public bool RemoveItem(TItem item)
        {
            if (Items.Remove(item))
            {
                var menuItem = _itemToMenuMap[item];

                menuItem.InputState.Clicked -= OnMenuItemClicked;
                
                _itemToMenuMap.Remove(item);
                _menuToItemMap.Remove(menuItem);
                
                ItemRemoved(this, new ItemEventArgs<TItem>(item, menuItem));

                return true;
            }

            return false;
        }

        public void RemoveRange(IEnumerable<TItem> items)
        {
            foreach(var item in items)
                RemoveItem(item);
        }
        
        private void NotifyMenuItemClicked(MenuItem menuItem)
        {
            var item = GetItemFromMenuItem(menuItem);
            ItemClicked?.Invoke(this, new ItemEventArgs<TItem>(item, menuItem));
        }

        private TItem GetItemFromMenuItem(MenuItem menuItem)
        {
            return _menuToItemMap.GetOrDefault(menuItem);
        }

        public TItem GetItemFromControl(MenuItem control)
        {
            if (_menuToItemMap.TryGetValue(control, out var item))
                return item;

            return null;
        }

        public MenuItem GetControlFromItem(TItem item)
        {
            if (_itemToMenuMap.TryGetValue(item, out var control))
                return control;

            return null;
        }

        public void Clear()
        {
            foreach (var item in Items.ToList())
                RemoveItem(item);
        }

        public void UpdateItem(TItem item)
        {
            InitMenuItemFromItem(item, _itemToMenuMap[item]);
        }
    }
}