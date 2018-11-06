using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
	public class TouchButtonList<TItem> : ContentControl
	{
		private readonly Dictionary<Button, TItem> _buttonToItemMap;
		private readonly Dictionary<TItem, Button> _itemToButtonMap;
		
		private readonly PagedListView<TItem> _visibleItems;
		private readonly VerticalStackPanel _itemsStackPanel;
		private readonly ControlFactory _controlFactory;
		private readonly Button _buttonUp;
		private readonly Button _buttonDown;

		/// <summary>
		/// The list of items. Do not use to add children. 
		/// Use AddItem of this class instead.
		/// </summary>
		/// <value>The items.</value>
		public List<TItem> Items { get; }

		/// <summary>
		/// Gets or sets the method that initializes the button from an item 
		/// instance. By default this just sets button.Text = item.ToString();
		/// </summary>
		/// <value>The initialize button from item.</value>
		public Action<TItem, Button> InitializeButtonFromItem { get; set; }

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
		public event EventHandler<TouchButtonClickedEventArgs> ButtonClicked;
		
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
			PropertySelectedItem.PropertyChanged += OnSelectedItemChanged;
			
			_controlFactory = controlFactory ?? throw new ArgumentNullException(nameof(controlFactory));
			_itemsStackPanel = _controlFactory.Create<VerticalStackPanel>();
			_buttonUp = _controlFactory.Create<Button>();
			_buttonDown = _controlFactory.Create<Button>();
			_buttonToItemMap = new Dictionary<Button, TItem>();
			_itemToButtonMap = new Dictionary<TItem, Button>();

			Items = new List<TItem>();
			_visibleItems = new PagedListView<TItem>(Items);

			InitializeButtonFromItem = (item, button) =>
			{
				button.Text = item.ToString();
			};

			CreateContent();
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
			scrollButtonDockPanel.Position.Width = 60;
			scrollButtonDockPanel.Padding.Left = 10;

			mainDockPanel.Add(Dock.Right, scrollButtonDockPanel);
			mainDockPanel.Add(Dock.Fill, _itemsStackPanel);
			mainDockPanel.VerticalAlignment = VerticalAlignment.Stretch;
			mainDockPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			
			Content = mainDockPanel;
		}

		public void AddItem(TItem item)
		{
			Items.Add(item);
			UpdateItemsAndScrollingState();
		}

		public void RemoveItem(TItem item)
		{
			if (Items.Remove(item))
				UpdateItemsAndScrollingState();
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

			foreach (var button in children.Cast<Button>())
				button.InputState.MouseClicked -= OnButtonClicked;

			children.Clear();
			_buttonToItemMap.Clear();
			_itemToButtonMap.Clear();

			foreach (var item in _visibleItems)
			{
				var button = CreateButtonForItem(item);
				children.Add(button);
				
				_buttonToItemMap.Add(button, item);
				_itemToButtonMap.Add(item, button);
			}

			IsLayoutUpdated = false;
		}

		private Button CreateButtonForItem(TItem item)
		{
			var result = _controlFactory.Create<Button>();
			InitializeButtonFromItem(item, result);
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

		private void NotifyButtonClicked(Button button)
		{
			var item = GetItemFromButton(button);
			ButtonClicked?.Invoke(this, new TouchButtonClickedEventArgs(button, item));
		}

		private TItem GetItemFromButton(Button button)
		{
			return _buttonToItemMap.GetOrDefault(button);
		}
	}
}
