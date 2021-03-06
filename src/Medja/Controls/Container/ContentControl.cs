using System.Collections.Generic;
using Medja.Primitives;
using Medja.Properties;

namespace Medja.Controls.Container
{
    public class ContentControl : Control
    {
        private static void SetContentAlignment(Control content)
        {
            if (content.VerticalAlignment == VerticalAlignment.None)
                content.VerticalAlignment = VerticalAlignment.Stretch;

            if (content.HorizontalAlignment == HorizontalAlignment.None)
                content.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        protected readonly ContentArranger ContentArranger;

        public readonly Property<Control> PropertyContent;
        public Control Content
        {
            get { return PropertyContent.Get(); }
            set { PropertyContent.Set(value); }
        }

        public readonly Property<bool> PropertyAutoSetContentAlignment;
        /// <summary>
        /// Gets or sets whether Horizontal- and VerticalAlignment of the Control is set as Content is set to Stretch
        /// automatically if it was set to None. If false no the alignment will not change.
        /// </summary>
        public bool AutoSetContentAlignment
        {
            get { return PropertyAutoSetContentAlignment.Get(); }
            set { PropertyAutoSetContentAlignment.Set(value); }
        }

        public Thickness Padding { get; }

        public ContentControl()
        {
            PropertyContent = new Property<Control>();
            PropertyContent.PropertyChanged += OnContentChanged;
            PropertyContent.AffectsLayoutOf(this);

            PropertyAutoSetContentAlignment = new Property<bool>();
            PropertyAutoSetContentAlignment.SetSilent(true);

            Padding = new Thickness();
            PropertyIsEnabled.PropertyChanged += OnIsEnabledChanged;
            PropertyIsLayoutUpdated.PropertyChanged += OnIsLayoutUpdatedChanged;

            ContentArranger = new ContentArranger();
        }

        protected virtual void OnIsLayoutUpdatedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Content != null)
                Content.IsLayoutUpdated = (bool)e.NewValue;
        }

        protected virtual void OnContentChanged(object sender, PropertyChangedEventArgs e)
        {
            var content = e.OldValue as Control;

            if (content != null && content.Parent == this)
            {
                content.Parent = null;
                content.PropertyIsEnabled.ClearOverwrittenValue();
            }

            content = e.NewValue as Control;
            ContentArranger.Control = content;

            if (content != null)
            {
                content.Parent = this;

                if(AutoSetContentAlignment)
                    SetContentAlignment(content);
            }

            UpdateContentIsEnabled();
        }

        private void OnIsEnabledChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            UpdateContentIsEnabled();
        }

        private void UpdateContentIsEnabled()
        {
            if (Content != null)
            {
                if(IsEnabled)
                    Content.PropertyIsEnabled.ClearOverwrittenValue();
                else
                    Content.PropertyIsEnabled.OverwriteSet(false);
            }
        }

        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);

            var area = new Rect(Position.X, Position.Y, availableSize.Width, availableSize.Height);
            area.Subtract(Padding);

            ContentArranger.Position(area);
            ContentArranger.Stretch(area);
        }

        public override IEnumerable<Control> GetChildren()
        {
            if (Content != null)
                yield return Content;
        }

        protected override void Dispose(bool disposing)
        {
            Content?.Dispose();
            base.Dispose(disposing);
        }
    }
}
