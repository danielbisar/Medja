using System;
using System.Collections.Generic;
using Medja.Input;
using Medja.Primitives;
using Medja.Properties;
using Medja.Properties.Binding;
using Medja.Theming;

namespace Medja.Controls
{
	public class TabControl : ContentControl
	{
		private readonly IControlFactory _controlFactory;
		private readonly List<TabItem> _tabs;
		private readonly ContentControl _tabContentControl;
		private IDisposable _headerWidthBinding;
		private IDisposable _headerHeightBinding;
		private IDisposable _headerSpacingBinding;
		
		private Panel _tabHeaderPanel;
		public Panel TabHeaderPanel
		{
			get { return _tabHeaderPanel; }
		}

		public readonly Property<float> PropertyTabHeaderHeight;
		public float TabHeaderHeight
		{
			get { return PropertyTabHeaderHeight.Get(); }
			set { PropertyTabHeaderHeight.Set(value); }
		}

		public readonly Property<float> PropertyTabHeaderMaxWidth;
        /// <summary>
        /// Gets or sets the maximum width of a tab header. 
        /// </summary>
		public float TabHeaderMaxWidth
		{
			get { return PropertyTabHeaderMaxWidth.Get(); }
			set { PropertyTabHeaderMaxWidth.Set(value); }
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

		public readonly Property<TabHeaderPosition> PropertyTabHeaderPosition;
		/// <summary>
		/// Gets or sets where the headers should be placed.
		/// </summary>
		public TabHeaderPosition TabHeaderPosition
		{
			get { return PropertyTabHeaderPosition.Get(); }
			set { PropertyTabHeaderPosition.Set(value); }
		}

		public readonly Property<float> PropertyTabHeaderSpacing;
		public float TabHeaderSpacing
		{
			get { return PropertyTabHeaderSpacing.Get(); }
			set { PropertyTabHeaderSpacing.Set(value); }
		}

        /// <summary>
        /// Gets the padding used for the header area.
        /// </summary>
        public Thickness HeaderAreaPadding { get; }
		
		public TabControl(IControlFactory controlFactory)
		{
			_controlFactory = controlFactory;
			_tabs = new List<TabItem>();
			Tabs = _tabs.AsReadOnly();
            HeaderAreaPadding = new Thickness();

			PropertyTabHeaderHeight = new Property<float>();
			PropertyTabHeaderHeight.SetSilent(30);
			PropertyTabHeaderMaxWidth = new Property<float>();
			PropertyTabHeaderMaxWidth.Set(200);
			PropertyTabHeaderSpacing = new Property<float>();
			PropertyTabHeaderSpacing.Set(5);
			PropertyTabHeaderPosition = new Property<TabHeaderPosition>();
			PropertyTabHeaderPosition.PropertyChanged += OnTabHeaderPositionChanged;
			PropertySelectedTab = new Property<TabItem>();
			PropertySelectedTab.PropertyChanged += OnSelectedTabChanged;
			PropertySelectedTab.AffectsLayoutOf(this);

            // HeaderAreaPadding.PropertyBottom.AffectsLayoutOf(this);
            // HeaderAreaPadding.PropertyLeft.AffectsLayoutOf(this);
            // HeaderAreaPadding.PropertyRight.AffectsLayoutOf(this);
            // HeaderAreaPadding.PropertyTop.AffectsLayoutOf(this);
            
			Padding.PropertyBottom.PropertyChanged += OnPaddingChanged;
			Padding.PropertyTop.PropertyChanged += OnPaddingChanged;
			Padding.PropertyLeft.PropertyChanged += OnPaddingChanged;
			Padding.PropertyRight.PropertyChanged += OnPaddingChanged;
            
            _tabContentControl = _controlFactory.Create<ContentControl>();
			UpdateContent();
		}

		private void OnPaddingChanged(object sender, PropertyChangedEventArgs e)
		{
			ForwardPaddingToContent();
		}

		private void ForwardPaddingToContent()
		{
			_tabContentControl.Padding.SetFrom(Padding);
		}

		private void UpdateContent()
		{
			if (_tabHeaderPanel != null)
			{
				_tabHeaderPanel.DisposeChildren = false;
				_tabHeaderPanel.Dispose();
				_headerWidthBinding.Dispose();
				_headerHeightBinding.Dispose();
				_headerSpacingBinding.Dispose();
			}

			if (Content != null)
			{
				((Panel) Content).DisposeChildren = false;
				Content.Dispose();
			}

			switch (TabHeaderPosition)
			{
				case TabHeaderPosition.Top:
					Content = CreateHeadersTop();
					break;
				case TabHeaderPosition.Left:
					Content = CreateHeadersLeft();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private Control CreateHeadersTop()
		{
			var uniformWidthPanel = _controlFactory.Create<UniformWidthPanel>();
			_tabHeaderPanel = uniformWidthPanel;
            
			_headerHeightBinding = _tabHeaderPanel.Position.PropertyHeight.UpdateFrom(PropertyTabHeaderHeight);
			_headerWidthBinding = uniformWidthPanel.PropertyMaxChildWidth.UpdateFrom(PropertyTabHeaderMaxWidth);
			_headerSpacingBinding = uniformWidthPanel.PropertySpaceBetweenChildren.UpdateFrom(PropertyTabHeaderSpacing);
			
            
            
			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Top, _tabHeaderPanel);
			dockPanel.Add(Dock.Fill, _tabContentControl);

			return dockPanel;
		}
		
		private Control CreateHeadersLeft()
		{
			var stackPanel = _controlFactory.Create<VerticalStackPanel>();
			_tabHeaderPanel = stackPanel;
			_headerWidthBinding = _tabHeaderPanel.Position.PropertyWidth.UpdateFrom(PropertyTabHeaderMaxWidth);
			_headerHeightBinding = stackPanel.PropertyChildrenHeight.UpdateFrom(PropertyTabHeaderHeight, f => f);
			_headerSpacingBinding = stackPanel.PropertySpaceBetweenChildren.UpdateFrom(PropertyTabHeaderSpacing);
			
			var dockPanel = _controlFactory.Create<DockPanel>();
			dockPanel.Add(Dock.Left, _tabHeaderPanel);
			dockPanel.Add(Dock.Fill, _tabContentControl);

			return dockPanel;
		}
		
		private void OnTabHeaderPositionChanged(object sender, PropertyChangedEventArgs e)
		{
			UpdateContent();
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

		public TabItem AddTab(string header, Control content)
		{
			var tabItem = _controlFactory.Create<TabItem>();
			tabItem.Header = header;
			tabItem.Content = content;
			
			AddTab(tabItem);
            
            return tabItem;
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
			_headerWidthBinding.Dispose();
			base.Dispose(disposing);
		}
	}
}
