using System;
using System.Collections.Generic;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
    public class ComboBox<TItem> : ComboBoxBase
        where TItem: class
    {
        private readonly ItemsManager<TItem> _itemsManager;

        public readonly Property<TItem> PropertySelectedItem;
        public TItem SelectedItem
        {
            get { return PropertySelectedItem.Get(); }
            set { PropertySelectedItem.Set(value); }
        }

        public IReadOnlyList<TItem> Items
        {
            get { return _itemsManager.Items; }
        }
        
        public ComboBox(ControlFactory controlFactory)
            : base(controlFactory)
        {
            PropertySelectedItem = new Property<TItem>();
            PropertySelectedItem.PropertyChanged += OnSelectedItemChanged;
            
            _itemsManager = new ItemsManager<TItem>(controlFactory);
            _itemsManager.IsSelectable = true;
            _itemsManager.PropertySelectedItem.PropertyChanged += OnItemsManagerSelectedItemChanged;
            _itemsManager.ButtonClicked += OnButtonClicked;
            _itemsManager.ItemAdded += OnItemAdded;
            _itemsManager.ItemRemoved += OnItemRemoved;
        }
        
        private void OnItemAdded(object sender, ItemEventArgs<TItem> e)
        {
            ItemsPanel.Children.Add(e.Control);
            UpdateItemsPanel();
        }
        
        private void OnItemRemoved(object sender, ItemEventArgs<TItem> e)
        {
            ItemsPanel.Children.Remove(e.Control);
            UpdateItemsPanel();
        }

        private void OnButtonClicked(object sender, TouchButtonClickedEventArgs e)
        {
            IsDropDownOpen = false;
        }

        public void AddItem(TItem item)
        {
            _itemsManager.AddItem(item);
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            _itemsManager.AddRange(items);
        }

        public bool RemoveItem(TItem item)
        {
            return _itemsManager.RemoveItem(item);
        }

        public void RemoveItems(IEnumerable<TItem> items)
        {
            _itemsManager.RemoveRange(items);
        }

        private void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = e.NewValue as TItem;

            _itemsManager.SelectedItem = item;
            
            if (item == null)
            {
                SelectedItemTextBlock.Text = "";
            }
            else
                SelectedItemTextBlock.Text = _itemsManager.GetControlFromItem(item)?.Text ?? "";
            
        }

        private void OnItemsManagerSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedItem = e.NewValue as TItem;
        }
    }
}