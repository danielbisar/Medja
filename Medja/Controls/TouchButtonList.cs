using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
	public class TouchButtonList<TItem> : ContentControl 
			where TItem : class
	{
		private readonly PagedListView<TItem> _visibleItems;
		private readonly VerticalStackPanel _itemsStackPanel;
		private readonly ControlFactory _controlFactory;
		private readonly Button _buttonUp;
		private readonly Button _buttonDown;
		private readonly ItemsManager<TItem> _itemsManager;

		/// <summary>
		/// The list of items. Do not use to add children. 
		/// Use AddItem of this class instead.
		/// </summary>
		/// <value>The items.</value>
		public IReadOnlyList<TItem> Items
		{
			get { return _itemsManager.Items; }
		}

		/// <summary>
		/// Gets or sets the method that initializes the button from an item 
		/// instance. By default this just sets button.Text = item.ToString();
		/// </summary>
		/// <value>The initialize button from item.</value>
		public Action<TItem, Button> InitButtonFromItem
		{
			get { return _itemsManager.InitButtonFromItem; }
			set { _itemsManager.InitButtonFromItem = value; }
		}

		/// <summary>
		/// Gets or sets the page size. This control displays PageSize amount
		/// of items and allows next/previous navigation between the pages.
		/// </summary>
		/// <value>The size of the page.</value>
		public int PageSize
		{
			get { return _visibleItems.PageSize; }
			set { _visibleItems.PageSize = value; }
		}

		/// <summary>
		/// An event that is fired whenever any of the item buttons is clicked.
		/// </summary>
		public event EventHandler<TouchButtonClickedEventArgs> ButtonClicked
		{
			add { _itemsManager.ButtonClicked += value; }
			remove { _itemsManager.ButtonClicked -= value; }
		}
		
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

		public TouchButtonList(ControlFactory controlFactory)
		{
			PropertyIsSelectable = new Property<bool>();
			PropertySelectedItem = new Property<TItem>();
			
			_itemsManager = new ItemsManager<TItem>(controlFactory);
			_itemsManager.PropertyIsSelectable.BindTo(PropertyIsSelectable);
			_itemsManager.PropertySelectedItem.BindTo(PropertySelectedItem);
			PropertySelectedItem.BindTo(_itemsManager.PropertySelectedItem);
			
			_controlFactory = controlFactory ?? throw new ArgumentNullException(nameof(controlFactory));
			_itemsStackPanel = _controlFactory.Create<VerticalStackPanel>();
			_buttonUp = _controlFactory.Create<Button>();
			_buttonDown = _controlFactory.Create<Button>();
			_visibleItems = new PagedListView<TItem>(_itemsManager.Items);
			
			CreateContent();
		}

		private void CreateContent()
		{
			var mainDockPanel = _controlFactory.Create<DockPanel>();

			_buttonUp.Text = "^";
			_buttonUp.Position.Height = 50;
			_buttonUp.InputState.MouseClicked += (s, e) => ScrollUp();

			_buttonDown.Text = "ˇ";
			_buttonDown.Position.Height = 50;
			_buttonDown.InputState.MouseClicked += (s, e) => ScrollDown();

			var scrollButtonDockPanel = _controlFactory.Create<DockPanel>();
			scrollButtonDockPanel.Add(Dock.Top, _buttonUp);
			scrollButtonDockPanel.Add(Dock.Bottom, _buttonDown);
			scrollButtonDockPanel.Position.Width = 70;
			scrollButtonDockPanel.Padding.Left = 10;

			mainDockPanel.Add(Dock.Right, scrollButtonDockPanel);
			mainDockPanel.Add(Dock.Fill, _itemsStackPanel);
			
			Content = mainDockPanel;
		}

		public void AddItem(TItem item)
		{
			_itemsManager.AddItem(item);
			UpdateItemsAndScrollingState();
		}

		public void RemoveItem(TItem item)
		{
			if (_itemsManager.RemoveItem(item))
			{
				if (ReferenceEquals(item, SelectedItem))
					SelectedItem = default(TItem);
				
				UpdateItemsAndScrollingState();
			}
		}

		public void ScrollUp()
		{
			_visibleItems.MovePrevious();
			UpdateItemsAndScrollingState();
		}

		public void ScrollDown()
		{
			_visibleItems.MoveNext();
			UpdateItemsAndScrollingState();
		}

		public void ScrollIntoView(TItem item)
		{
			_visibleItems.MakeActivePageWith(item);
			UpdateItemsAndScrollingState();
		}

		public void UpdateItemsAndScrollingState()
		{
			UpdateItems();
			UpdateButtons();
		}
		
		private void UpdateButtons()
		{
			_buttonUp.IsEnabled = _visibleItems.CanMovePrevious();
			_buttonDown.IsEnabled = _visibleItems.CanMoveNext();
		}

		private void UpdateItems()
		{
			var children = _itemsStackPanel.Children;
			children.Clear();
			
			foreach(var item in _visibleItems)
				children.Add(_itemsManager.GetControlFromItem(item));

			IsLayoutUpdated = false;
		}

		public void UpdateItem(TItem item)
		{
			_itemsManager.UpdateItem(item);
		}

		public void Clear()
		{
			_itemsManager.Clear();
		}
	}
}
