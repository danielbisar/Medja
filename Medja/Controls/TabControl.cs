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

		public Property<TabItem> PropertySelectedTab;
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
			HeaderHeight = 50;
			_tabs = new List<TabItem>();
			Tabs = _tabs.AsReadOnly();

			PropertySelectedTab = new Property<TabItem>();
			PropertySelectedTab.PropertyChanged += OnSelectedTabChanged;
			InputState.MouseClicked += OnMouseClicked;
		}

		protected virtual void OnMouseClicked(object sender, EventArgs e)
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
				if (Position.Y <= pointerPos.Y && pointerPos.Y <= Position.Y + HeaderHeight)
				{
					var tabWidth = Position.Width / _tabs.Count;
					var tabIndex = (int)(pointerPos.X / tabWidth);

					return _tabs[tabIndex];
				}
			}

			return null;
		}

		protected virtual void OnSelectedTabChanged(IProperty property)
		{
			if (SelectedTab == null)
				Content = null;
			else
			{
				Content = SelectedTab.Content;
				SelectedTab.IsSelected = true;
			}
		}

		public virtual void AddTab(TabItem tabItem)
		{
			_tabs.Add(tabItem);

			if (_tabs.Count == 1)
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
			base.Arrange(availableSize);

			if (Content != null)
			{
				var pos = Content.Position;

				pos.X = Position.X + Padding.Left;
				pos.Y = Position.Y + Padding.Top + HeaderHeight;
				ArrangeContent();
			}
		}

		protected override void ArrangeContent()
		{
			if (Content != null)
			{
				var pos = Content.Position;

				pos.Width = Position.Width - Padding.LeftAndRight;

				var availableHeight = Position.Height - Padding.TopAndBottom - HeaderHeight;

				pos.Height = Content.VerticalAlignment == VerticalAlignment.Top
					|| Content.VerticalAlignment == VerticalAlignment.Bottom
					? pos.Height : availableHeight;

				Content.Arrange(new Size(pos.Width, pos.Height));
			}
		}
	}
}
