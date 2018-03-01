using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
    public class ContentControl : Control
    {
        public readonly Property<Control> PropertyContent;
        public Control Content
        {
            get { return PropertyContent.Get(); }
            set { PropertyContent.Set(value); }
        }

        public ContentControl()
        {
            PropertyContent = new Property<Control>();
        }

        public override void UpdateLayout()
        {
            base.UpdateLayout();

            if (Content == null)
                return;

            var result = Content.Measure(new Size(Position.Width, Position.Height));
            Content.Arrange(result);
        }

        internal override Size Measure(Size availableSize)
        {
            if (Content == null)
                return base.Measure(availableSize);

            return Content.Measure(availableSize);
        }

        internal override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);

            if (Content == null)
                return;

            Content.Arrange(availableSize);
        }

        public override IEnumerable<Control> GetAllControls()
        {
            yield return this;

            foreach (var control in Content.GetAllControls())
                yield return control;
        }
    }
}
