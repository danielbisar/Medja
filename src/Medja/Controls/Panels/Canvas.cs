using Medja.Primitives;
using Medja.Properties;

namespace Medja.Controls
{
    /// <summary>
    /// A Panel that allows its children to be positioned freely.
    /// Use attached property X and Y available in this class.
    /// </summary>
    public class Canvas : Panel
    {
        // the attached X and Y is the initial position provided by the control
        public static readonly int AttachedXId = AttachedPropertyIdFactory.NewId();
        public static readonly int AttachedYId = AttachedPropertyIdFactory.NewId();
        
        public static void SetX(Control child, float value)
        {
            child.AttachedProperties.Set(AttachedXId, value);
        }

        public static void SetY(Control child, float value)
        {
            child.AttachedProperties.Set(AttachedYId, value);
        }

        protected override void OnItemAdded(Control child)
        {
            base.OnItemAdded(child);

            var attachedProperties = child.AttachedProperties;
            var propertyX = attachedProperties.GetOrAddProperty<float>(AttachedXId);
            var propertyY = attachedProperties.GetOrAddProperty<float>(AttachedYId);

            propertyX.AffectsLayoutOf(this);
            propertyY.AffectsLayoutOf(this);
        }

        protected override void OnItemRemoved(Control child)
        {
            var attachedProperties = child.AttachedProperties;
            var propertyX = attachedProperties[AttachedXId];
            var propertyY = attachedProperties[AttachedYId];

            propertyX.RemoveAffectsLayoutOf(this);
            propertyY.RemoveAffectsLayoutOf(this);

            base.OnItemRemoved(child);
        }
        
        public override void Arrange(Size availableSize)
        {
            var x = Position.X;
            var y = Position.Y;

            foreach (var child in Children)
            {
                var attachedProperties = child.AttachedProperties;

                var childX = attachedProperties.Get<float>(AttachedXId);
                var childY = attachedProperties.Get<float>(AttachedYId);
                
                child.Position.X = x + childX + child.Margin.Left;
                child.Position.Y = y + childY + child.Margin.Top;

                child.Arrange(new Size(child.Position.Width, child.Position.Height));
            }

            base.Arrange(availableSize);
        }
    }
}
