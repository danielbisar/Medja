using System;
using System.Collections.Generic;
using Medja.Input;
using Medja.Primitives;
using Medja.Theming;
using Medja.Utils;

namespace Medja.Controls
{
	public class TabControl : ContentControl
	{
		private readonly IControlFactory _controlFactory;
		private readonly List<TabItem> _tabs;
		
		private HorizontalStackPanel _tabHeaderPanel;
		public HorizontalStackPanel TabHeaderPanel
		{
			get { return _tabHeaderPanel; }
		}

		private ContentControl _tabContentControl;
		private IDisposable _headerHeightBinding;

		public readonly Property<float> PropertyTabHeaderHeight;

		public float TabHeaderHeight
		{
			get { return PropertyTabHeaderHeight.Get(); }
			set { PropertyTabHeaderHeight.Set(value); }
		}

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

		public TabControl(IControlFactory controlFactory)
		{
			_controlFactory = controlFactory;
			_tabs = new List<TabItem>();
			Tabs = _tabs.AsReadOnly();

			PropertyTabHeaderHeight = new Property<float>();
			PropertyTabHeaderHeight.UnnotifiedSet(30);
			PropertySelectedTab = new Property<TabItem>();
			PropertySelectedTab.PropertyChanged += OnSelectedTabChanged;
			PropertySelectedTab.AffectsLayout(this);

			Padding.PropertyBottom.PropertyChanged += OnPaddingChanged;
			Padding.PropertyTop.PropertyChanged += OnPaddingChanged;
			Padding.PropertyLeft.PropertyChanged += OnPaddingChanged;
			Padding.PropertyRight.PropertyChanged += OnPaddingChanged;

			Content = CreateContent();
		}

		private void OnPaddingChanged(object sender, PropertyChangedEventArgs e)
		{
			ForwardPaddingToContent();
		}

		private void ForwardPaddingToContent()
		{
			_tabContentControl.Padding.SetFrom(Padding);
		}

		protected Control CreateContent()
		{
			_tabHeaderPanel = _controlFactory.Create<HorizontalStackPanel>();
			_headerHeightBinding = _tabHeaderPanel.Position.PropertyHeight.BindTo(PropertyTabHeaderHeight);
			_tabContentControl = _controlFactory.Create<ContentControl>();
			
			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Top, _tabHeaderPanel);
			dockPanel.Add(Dock.Fill, _tabContentControl);

			return dockPanel;
		}

		protected virtual void OnSelectedTabChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			if (SelectedTab?.Content == null)
				_tabContentControl.Content = null;
			else
			{
				_tabContentControl.Content = SelectedTab.Content;
				SelectedTab.IsSelected = true;
			}
		}

		public void AddTab(string header, Control content)
		{
			var tabItem = _controlFactory.Create<TabItem>();
			tabItem.Header = header;
			tabItem.Content = content;
			
			AddTab(tabItem);
		}

		public virtual void AddTab(TabItem tabItem)
		{
			if(tabItem == null)
				throw new ArgumentNullException(nameof(tabItem));
			
			_tabs.Add(tabItem);
			_tabHeaderPanel.Add(tabItem);
			tabItem.InputState.Clicked += OnTabClicked;

			if (_tabs.Count > 0 && !_tabs.Contains(SelectedTab))
				SelectedTab = _tabs[0];

			IsLayoutUpdated = false;
		}
		
		public override void Arrange(Size availableSize)
		{
			base.Arrange(availableSize);

			var area = new Rect(Position.X, Position.Y, availableSize.Width, availableSize.Height);
			area.Subtract(Margin);
			
			ContentArranger.Position(area);
			ContentArranger.Stretch(area);
		}

		private void OnTabClicked(object sender, EventArgs e)
		{
			var inputState = (InputState) sender;
			SelectedTab = (TabItem) inputState.Control;
		}

		public virtual void RemoveTab(TabItem tabItem)
		{
			_tabs.Remove(tabItem);
			_tabHeaderPanel.Remove(tabItem);
			
			tabItem.InputState.Clicked -= OnTabClicked;

			if (SelectedTab == tabItem)
				SelectedTab = _tabs.Count > 0 ? _tabs[0] : null;

			IsLayoutUpdated = false;
		}

		public virtual void Clear()
		{
			_tabs.Clear();
			SelectedTab = null;
		}

		protected override void Dispose(bool disposing)
		{
			_headerHeightBinding.Dispose();
			base.Dispose(disposing);
		}
	}
}
