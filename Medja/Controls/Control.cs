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

		public MPosition Position { get; }

		public Property<Color> BackgroundProperty { get; }
		public Color Background
		{
			get { return BackgroundProperty.Get(); }
			set { BackgroundProperty.Set(value); }
		}

		public Property<bool> IsEnabledProperty { get; }
		public bool IsEnabled
		{
			get { return IsEnabledProperty.Get(); }
			set { IsEnabledProperty.Set(value); }
		}

		public IControlRenderer Renderer { get; set; }

		public Property<Visibility> VisibilityProperty { get; }
		public Visibility Visibility
		{
			get { return VisibilityProperty.Get(); }
			set { VisibilityProperty.Set(value); }
		}

		public Property<bool> IsVisibleProperty { get; }
		public bool IsVisible
		{
			get { return IsVisibleProperty.Get(); }
		}

		public Property<object> DataContextProperty;
		public object DataContext
		{
			get { return DataContextProperty.Get(); }
			set { DataContextProperty.Set(value); }
		}

		public Control()
		{
			AnimationManager = new AnimationManager();
			AttachedProperties = new Dictionary<int, object>();
			InputState = new InputState(this);
			Position = new MPosition();
			BackgroundProperty = new Property<Color>();
			IsEnabledProperty = new Property<bool>();
			IsEnabledProperty.UnnotifiedSet(true);

			VisibilityProperty = new Property<Visibility>();
			IsVisibleProperty = new Property<bool>();
			IsVisibleProperty.UnnotifiedSet(true);
			VisibilityProperty.PropertyChanged += OnVisibilityChanged;

			DataContextProperty = new Property<object>();
		}

		private void OnVisibilityChanged(IProperty property)
		{
			IsVisibleProperty.Set(Visibility == Visibility.Visible);
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
