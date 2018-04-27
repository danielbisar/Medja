using System.Collections.Generic;
using Medja.Controls.Animation;
using Medja.Primitives;

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
            //Debug.WriteLine("Control.UpdateLayout of " + GetType().FullName);
            UpdateAnimations();

            var result = Measure(new Size(Position.Width, Position.Height));
            Arrange(result);
        }

        internal virtual Size Measure(Size availableSize)
        {
            //Debug.WriteLine("Control.Measure of " + GetType().FullName);
            return availableSize;
        }

        internal virtual void Arrange(Size availableSize)
        {
            //Debug.WriteLine("Control.Arrange of " + GetType().FullName);
            // TODO update position info
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

        internal virtual void UpdateAnimations()
        {
            AnimationManager.ApplyAnimations();
        }
    }
}
