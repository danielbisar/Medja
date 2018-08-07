using System;
using System.Collections.Generic;
using System.Linq;

namespace Medja.Controls
{
	public class TouchButtonList<TItem> : ContentControl
	{
		private readonly Dictionary<Button, TItem> _buttonToItemMap;
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

		public event EventHandler<TouchButtonClickedEventArgs> ButtonClicked;

		public TouchButtonList(ControlFactory controlFactory)
		{
			_controlFactory = controlFactory ?? throw new ArgumentNullException(nameof(controlFactory));
			_itemsStackPanel = _controlFactory.Create<VerticalStackPanel>();
			_buttonUp = _controlFactory.Create<Button>();
			_buttonDown = _controlFactory.Create<Button>();
			_buttonToItemMap = new Dictionary<Button, TItem>();

			Items = new List<TItem>();
			_visibleItems = new PagedListView<TItem>(Items);

			InitializeButtonFromItem = (item, button) =>
			{
				button.Text = item.ToString();
			};

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
			scrollButtonDockPanel.Position.Width = 60;
			scrollButtonDockPanel.Padding.Left = 10;

			mainDockPanel.Add(Dock.Right, scrollButtonDockPanel);
			mainDockPanel.Add(Dock.Fill, _itemsStackPanel);

			Content = mainDockPanel;
		}

		public void AddItem(TItem item)
		{
			Items.Add(item);
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

		private void UpdateItemsAndScrollingState()
		{
			UpdateItems();
			UpdateButtons();
		}

		private void UpdateItems()
		{
			var children = _itemsStackPanel.Children;

			foreach (var button in children.Cast<Button>())
				button.InputState.MouseClicked -= OnButtonClicked;

			children.Clear();
			_buttonToItemMap.Clear();

			foreach (var item in _visibleItems)
			{
				var button = CreateButtonForItem(item);
				children.Add(button);
				_buttonToItemMap.Add(button, item);
			}
		}

		private Button CreateButtonForItem(TItem item)
		{
			var result = _controlFactory.Create<Button>();
			InitializeButtonFromItem(item, result);
			result.InputState.MouseClicked += OnButtonClicked;

			return result;
		}

		private void OnButtonClicked(object sender, EventArgs e)
		{
			var inputState = sender as InputState;
			NotifyButtonClicked((Button)inputState.Control);
		}

		private void UpdateButtons()
		{
			_buttonUp.IsEnabled = _visibleItems.CanMovePrevious();
			_buttonDown.IsEnabled = _visibleItems.CanMoveNext();
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
