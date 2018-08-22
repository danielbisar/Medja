using System.Collections.Generic;
using Medja.Controls.Animation;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.Controls
{
	/// <summary>
	/// Any control should inherit from this class.
	/// </summary>
	public class Control : MObject
	{
		public AnimationManager AnimationManager { get; }
		public Dictionary<int, object> AttachedProperties { get; }
		public InputState InputState { get; }

		/// <summary>
		/// Gets the position this item should be drawn at.
		/// </summary>
		/// <value>The position.</value>
		public MPosition Position { get; }

		public Property<Color> PropertyBackground { get; }
		public Color Background
		{
			get { return PropertyBackground.Get(); }
			set { PropertyBackground.Set(value); }
		}

		public Property<bool> PropertyIsEnabled { get; }
		public bool IsEnabled
		{
			get { return PropertyIsEnabled.Get(); }
			set { PropertyIsEnabled.Set(value); }
		}

		public Property<bool> PropertyIsFocused;
		public bool IsFocused
		{
			get { return PropertyIsFocused.Get(); }
			internal set { PropertyIsFocused.Set(value); }
		}

		public IControlRenderer Renderer { get; set; }

		public Property<Visibility> PropertyVisibility { get; }
		public Visibility Visibility
		{
			get { return PropertyVisibility.Get(); }
			set { PropertyVisibility.Set(value); }
		}

		public Property<bool> PropertyIsVisible { get; }
		public bool IsVisible
		{
			get { return PropertyIsVisible.Get(); }
		}

		public Property<object> DataContextProperty;
		public object DataContext
		{
			get { return DataContextProperty.Get(); }
			set { DataContextProperty.Set(value); }
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

		public Control()
		{
			AnimationManager = new AnimationManager();
			AttachedProperties = new Dictionary<int, object>();
			InputState = new InputState(this);
			Position = new MPosition();
			PropertyBackground = new Property<Color>();
			PropertyIsEnabled = new Property<bool>();
			PropertyIsEnabled.UnnotifiedSet(true);
			PropertyIsFocused = new Property<bool>();

			PropertyVisibility = new Property<Visibility>();
			PropertyIsVisible = new Property<bool>();
			PropertyIsVisible.UnnotifiedSet(true);
			PropertyVisibility.PropertyChanged += OnVisibilityChanged;

			DataContextProperty = new Property<object>();
			PropertyVerticalAlignment = new Property<VerticalAlignment>();
			PropertyHorizontalAlignment = new Property<HorizontalAlignment>();
		}

		private void OnVisibilityChanged(IProperty property)
		{
			PropertyIsVisible.Set(Visibility == Visibility.Visible);
		}

		public virtual void UpdateLayout()
		{
			UpdateAnimations();

			var result = Measure(new Size(Position.Width, Position.Height));
			Arrange(result);
		}

		/// <summary>
		/// Measure how much space the current control needs. Default just returns available size.
		/// </summary>
		/// <returns>The measure.</returns>
		/// <param name="availableSize">Available size.</param>
		public virtual Size Measure(Size availableSize)
		{
			return availableSize;
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
			if (AttachedProperties.TryGetValue(id, out var result))
				return result;

			return null;
		}

		public T GetAttachedProperty<T>(int id)
		{
			return (T)GetAttachedProperty(id);
		}

		public void RemoveAttachedProperty(int id)
		{
			AttachedProperties.Remove(id);
		}

		public virtual IEnumerable<Control> GetAllControls()
		{
			yield return this;
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
