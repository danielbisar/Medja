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
		public Dictionary<int, object> AttachedProperties { get; set; }
		public InputState InputState { get; }

		public MPosition Position { get; }

		public Property<Color> BackgroundProperty { get; }
		public Color Background
		{
			get { return BackgroundProperty.Get(); }
			set { BackgroundProperty.Set(value); }
		}

		public IControlRenderer Renderer { get; set; }

		public Control()
		{
			AnimationManager = new AnimationManager();
			InputState = new InputState();
			Position = new MPosition();
			BackgroundProperty = new Property<Color>();
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
		internal virtual Size Measure(Size availableSize)
		{
			return availableSize;
		}

		/// <summary>
		/// Arrange places the sub-controls as needed. Default does nothing.
		/// </summary>
		/// <param name="availableSize">Available size.</param>
		internal virtual void Arrange(Size availableSize)
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
