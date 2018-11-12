using System;
using System.Collections.Generic;
using System.Linq;
using Medja.Theming;

namespace Medja.Controls
{
    public class ItemsManager<TItem>
     where TItem: class
    {
        private readonly Dictionary<Button, TItem> _buttonToItemMap;
        private readonly Dictionary<TItem, Button> _itemToButtonMap;
        private readonly ControlFactory _controlFactory;

        public List<TItem> Items { get; }

        public Action<TItem, Button> InitButtonFromItem { get; set; }
        
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
        public event EventHandler<TouchButtonClickedEventArgs> ButtonClicked;

        public event EventHandler<ItemEventArgs<TItem>> ItemAdded;
        public event EventHandler<ItemEventArgs<TItem>> ItemRemoved;
        
        public ItemsManager(ControlFactory controlFactory)
        {
            PropertyIsSelectable = new Property<bool>();
            PropertySelectedItem = new Property<TItem>();
            PropertySelectedItem.PropertyChanged += OnSelectedItemChanged;
            
            _controlFactory = controlFactory ?? throw new ArgumentNullException(nameof(controlFactory));;
            _buttonToItemMap = new Dictionary<Button, TItem>(new ReferenceEqualityComparer<Button>());
            _itemToButtonMap = new Dictionary<TItem, Button>(new ReferenceEqualityComparer<TItem>());
            
            Items = new List<TItem>();
            
            InitButtonFromItem = (item, button) =>
            {
                button.Text = item.ToString();
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
                if(_itemToButtonMap.TryGetValue(oldSelectedItem, out var button))
                    button.IsSelected = false;
            }

            if (newSelectedItem != null)
            {
                // we need to use try, because the button might not be visible
                // if user sets it manually
                if(_itemToButtonMap.TryGetValue(newSelectedItem, out var button))
                    button.IsSelected = true;
            }
        }
        
        public void AddItem(TItem item)
        {
            Items.Add(item);

            var button = CreateButtonForItem(item);
            _buttonToItemMap.Add(button, item);
            _itemToButtonMap.Add(item, button);
            
            ItemAdded?.Invoke(this, new ItemEventArgs<TItem>(item, button));
        }
        
        public void AddRange(IEnumerable<TItem> items)
        {
            foreach(var item in items)
                AddItem(item);
        }
        
        private Button CreateButtonForItem(TItem item)
        {
            var result = _controlFactory.Create<Button>();
            InitButtonFromItem(item, result);
            result.InputState.MouseClicked += OnButtonClicked;

            if (IsSelectable && ReferenceEquals(item, SelectedItem))
                result.IsSelected = true;

            return result;
        }
        
        private void OnButtonClicked(object sender, EventArgs e)
        {
            var inputState = sender as InputState;
            var button = (Button) inputState.Control;
			
            NotifyButtonClicked(button);

            if (IsSelectable)
                SelectedItem = _buttonToItemMap[button];
        }

        public bool RemoveItem(TItem item)
        {
            if (Items.Remove(item))
            {
                var button = _itemToButtonMap[item];

                button.InputState.MouseClicked -= OnButtonClicked;
                
                _itemToButtonMap.Remove(item);
                _buttonToItemMap.Remove(button);
                
                ItemRemoved?.Invoke(this, new ItemEventArgs<TItem>(item, button));

                return true;
            }

            return false;
        }

        public void RemoveRange(IEnumerable<TItem> items)
        {
            foreach(var item in items)
                RemoveItem(item);
        }
        
        private void NotifyButtonClicked(Button button)
        {
            var item = GetItemFromButton(button);
            ButtonClicked?.Invoke(this, new TouchButtonClickedEventArgs(button, item));
        }

        private TItem GetItemFromButton(Button button)
        {
            return _buttonToItemMap.GetOrDefault(button);
        }

        public TItem GetItemFromControl(Button control)
        {
            if (_buttonToItemMap.TryGetValue(control, out var item))
                return item;

            return null;
        }

        public Button GetControlFromItem(TItem item)
        {
            if (_itemToButtonMap.TryGetValue(item, out var control))
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
            InitButtonFromItem(item, _itemToButtonMap[item]);
        }
    }
}