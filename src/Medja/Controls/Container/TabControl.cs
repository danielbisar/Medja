using System;
using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
	public class TabControl : ContentControl
	{
		private readonly List<TabItem> _tabs;

		public float HeaderHeight { get; }

		/// <summary>
		/// Gets the tabs. Use AddTabs/RemoveTabs of TabControl to modify.
		/// </summary>
		/// <value>The tabs.</value>
		public IReadOnlyList<TabItem> Tabs { get; }

		public readonly Property<TabItem> PropertySelectedTab;
		public TabItem SelectedTab
		{
			get { return PropertySelectedTab.Get(); }
			set
			{
				var oldTab = PropertySelectedTab.Get();

				if (oldTab != value && oldTab != null)
					oldTab.IsSelected = false;

				PropertySelectedTab.Set(value);
			}
		}

		public TabControl()
		{
			HeaderHeight = 40;
			_tabs = new List<TabItem>();
			Tabs = _tabs.AsReadOnly();

			PropertySelectedTab = new Property<TabItem>();
			PropertySelectedTab.PropertyChanged += OnSelectedTabChanged;
			PropertySelectedTab.AffectsLayout(this);
			
			InputState.Clicked += OnClicked;
		}

		protected virtual void OnClicked(object sender, EventArgs e)
		{
			var pointerPos = InputState.PointerPosition;

			var tab = GetTabFromPoint(pointerPos);

			if (tab != null)
				SelectedTab = tab;
		}

		/// <summary>
		/// Gets the tab from point (if clicked on it's header).
		/// </summary>
		/// <returns>The tab from point.</returns>
		/// <param name="pointerPos">Pointer position.</param>
		protected virtual TabItem GetTabFromPoint(Point pointerPos)
		{
			if (Position.X <= pointerPos.X && pointerPos.X <= Position.X + Position.Width)
			{
				if (pointerPos.Y >= Position.Y
				    && pointerPos.Y <= Position.Y + HeaderHeight)
				{
					var tabWidth = Position.Width / _tabs.Count;
					var tabIndex = (int)(pointerPos.X / tabWidth);

					return _tabs[tabIndex];
				}
			}

			return null;
		}

		protected virtual void OnSelectedTabChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			if (SelectedTab == null || SelectedTab.Content == null)
				Content = null;
			else
			{
				Content = SelectedTab.Content;
				
				if (Content.VerticalAlignment == VerticalAlignment.None)
					Content.VerticalAlignment = VerticalAlignment.Stretch;
				
				if (Content.HorizontalAlignment == HorizontalAlignment.None)
					Content.HorizontalAlignment = HorizontalAlignment.Stretch;
				
				SelectedTab.IsSelected = true;
			}
		}

		public virtual void AddTab(TabItem tabItem)
		{
			if(tabItem == null)
				throw new ArgumentNullException(nameof(tabItem));
			
			_tabs.Add(tabItem);

			if (_tabs.Count > 0 && !_tabs.Contains(SelectedTab))
				SelectedTab = _tabs[0];
		}

		public virtual void RemoveTab(TabItem tabItem)
		{
			_tabs.Remove(tabItem);

			if (SelectedTab == tabItem)
				SelectedTab = _tabs.Count > 0 ? _tabs[0] : null;
		}

		public virtual void Clear()
		{
			_tabs.Clear();
			SelectedTab = null;
		}

		public override void Arrange(Size availableSize)
		{
			//base.Arrange(availableSize);
			
			var area = new Rect(Position.X, Position.Y, availableSize.Width, availableSize.Height);
			area.SubtractTop(HeaderHeight);
			area.Subtract(Padding);
			
			ContentArranger.Position(area);
			ContentArranger.Stretch(area);
		}
		
		protected override void Dispose(bool disposing)
		{
			foreach(var tab in _tabs)
				tab.Dispose();
			
			base.Dispose(disposing);
		}
	}
}
