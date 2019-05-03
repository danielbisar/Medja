using System;
using System.Collections.Generic;
using Medja.Controls.Animation;
using Medja.Input;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
	/// <summary>
	/// Any control should inherit from this class.
	/// </summary>
	public class Control
	{
		public AnimationManager AnimationManager { get; }
		public Dictionary<int, object> AttachedProperties { get; }
		public InputState InputState { get; }

		/// <summary>
		/// Gets the position this item should be drawn at.
		/// </summary>
		/// <value>The position.</value>
		public MRect Position { get; }
		
		/// <summary>
		/// Defines the outer border, where no rendering occurs.
		/// </summary>
		public Thickness Margin { get; }

		public readonly Property<Color> PropertyBackground;
		public Color Background
		{
			get { return PropertyBackground.Get(); }
			set { PropertyBackground.Set(value); }
		}

		public readonly OverwritableProperty<bool> PropertyIsEnabled;
		public bool IsEnabled
		{
			get { return PropertyIsEnabled.Get(); }
			set { PropertyIsEnabled.Set(value); }
		}

		public readonly Property<bool> PropertyIsFocused;
		public bool IsFocused
		{
			get { return PropertyIsFocused.Get(); }
			internal set { PropertyIsFocused.Set(value); }
		}

		public IControlRenderer Renderer { get; set; }

		public readonly Property<Visibility> PropertyVisibility;
		public Visibility Visibility
		{
			get { return PropertyVisibility.Get(); }
			set { PropertyVisibility.Set(value); }
		}

		public readonly Property<bool> PropertyIsVisible;
		public bool IsVisible
		{
			get { return PropertyIsVisible.Get(); }
		}

		public readonly Property<Control> PropertyParent;
		public Control Parent
		{
			get { return PropertyParent.Get(); }
			set { PropertyParent.Set(value); }
		}

		public readonly Property<VerticalAlignment> PropertyVerticalAlignment;
		public VerticalAlignment VerticalAlignment
		{
			get { return PropertyVerticalAlignment.Get(); }
			set { PropertyVerticalAlignment.Set(value); }
		}

		public readonly Property<HorizontalAlignment> PropertyHorizontalAlignment;
		public HorizontalAlignment HorizontalAlignment
		{
			get { return PropertyHorizontalAlignment.Get(); }
			set { PropertyHorizontalAlignment.Set(value); }
		}
		
		public readonly Property<bool> PropertyIsLayoutUpdated;
		public bool IsLayoutUpdated
		{
			get { return PropertyIsLayoutUpdated.Get(); }
			set { PropertyIsLayoutUpdated.Set(value); }
		}

		public readonly Property<bool> PropertyIsTopMost;
		/// <summary>
		/// If set to true this control will be rendered above others (later).
		/// </summary>
		public bool IsTopMost
		{
			get { return PropertyIsTopMost.Get(); }
			set { PropertyIsTopMost.Set(value); }
		}
		
		public MRect ClippingArea { get; }

		public Control()
		{
			AnimationManager = new AnimationManager();
			AttachedProperties = new Dictionary<int, object>();
			InputState = new InputState(this);
			Position = new MRect();
			Position.PropertyHeight.PropertyChanged += OnPositionChanged;
			Position.PropertyWidth.PropertyChanged += OnPositionChanged;
			Position.PropertyX.PropertyChanged += OnPositionChanged;
			Position.PropertyY.PropertyChanged += OnPositionChanged;
			Margin = new Thickness();
			
			PropertyBackground = new Property<Color>();
			PropertyIsEnabled = new OverwritableProperty<bool>();
			PropertyIsEnabled.UnnotifiedSet(true);
			PropertyIsFocused = new Property<bool>();

			PropertyVisibility = new Property<Visibility>();
			PropertyIsVisible = new Property<bool>();
			PropertyIsVisible.UnnotifiedSet(true);
			PropertyVisibility.PropertyChanged += OnVisibilityChanged;

			PropertyParent = new Property<Control>();
			PropertyVerticalAlignment = new Property<VerticalAlignment>();
			PropertyHorizontalAlignment = new Property<HorizontalAlignment>();
			PropertyIsLayoutUpdated = new Property<bool>();
			
			PropertyIsTopMost = new Property<bool>();
			
			ClippingArea = new MRect();
		}

		protected virtual void OnPositionChanged(object sender, PropertyChangedEventArgs e)
		{
			IsLayoutUpdated = false;
		}

		private void OnVisibilityChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			PropertyIsVisible.Set(Visibility == Visibility.Visible);
		}

		public virtual void UpdateLayout()
		{
			UpdateAnimations();
			Arrange(new Size(Position.Width, Position.Height));
			
			IsLayoutUpdated = true;
		}

		/// <summary>
		/// Arrange places the sub-controls as needed. Default does nothing.
		/// </summary>
		/// <param name="availableSize">Available size.</param>
		public virtual void Arrange(Size availableSize)
		{
		}

		public void SetAttachedProperty(int id, object value)
		{
			AttachedProperties[id] = value;
		}

		public object GetAttachedProperty(int id)
		{
			return AttachedProperties.TryGetValue(id, out var result) ? result : null;
		}

		public T GetAttachedProperty<T>(int id)
		{
			return (T)GetAttachedProperty(id);
		}

		public void RemoveAttachedProperty(int id)
		{
			AttachedProperties.Remove(id);
		}

		public virtual IEnumerable<Control> GetChildren()
		{
			return new Control[0];
		}

		/// <summary>
		/// Updates the state of the animations. Should not be called manually.
		/// </summary>
		public virtual void UpdateAnimations()
		{
			AnimationManager.ApplyAnimations();
		}
	}
}
