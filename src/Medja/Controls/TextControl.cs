using System;
using System.Linq;
using Medja.Primitives;
using Medja.Properties;
using Medja.Utils;

namespace Medja.Controls
{
    /// <summary>
    /// A base class for controls that mainly display text.
    /// </summary>
    /// <remarks>
    /// Hint for implementing a renderer for any TextControl: use GetLines to receive the wrapped lines.
    /// The Font.GetWidth method must be set beforehand.
    /// </remarks>
    public class TextControl : Control
    {
        private string[] _lines;
        private bool _linesNeedUpdate;
        
        public readonly Property<string> PropertyText;
        public string Text
        {
            get { return PropertyText.Get(); }
            set { PropertyText.Set(value); }
        }

        public readonly Property<TextWrapping> PropertyTextWrapping;
        public TextWrapping TextWrapping
        {
            get { return PropertyTextWrapping.Get(); }
            set { PropertyTextWrapping.Set(value); }
        }

        public readonly Property<TextAlignment> PropertyTextAlignment;
        public TextAlignment TextAlignment
        {
            get { return PropertyTextAlignment.Get(); }
            set { PropertyTextAlignment.Set(value);}
        }

        public Font Font { get; }
        public Thickness Padding { get; }
        
        public MRect TextClippingArea { get; }

        /// <summary>
        /// Raised when the layout of lines was recalculated.
        /// </summary>
        public event EventHandler TextMeasured;
        
        public TextControl()
        {
            PropertyText = new Property<string>();
            PropertyTextAlignment = new Property<TextAlignment>();
            PropertyTextWrapping = new Property<TextWrapping>();
            
            PropertyText.PropertyChanged += OnTextChanged;
            PropertyTextWrapping.PropertyChanged += OnTextWrappingChanged;
            PropertyIsLayoutUpdated.PropertyChanged += OnIsLayoutUpdatedChanged;
            
            Padding = new Thickness();
            TextClippingArea = new MRect();

            Font = new Font();
            _linesNeedUpdate = true;
        }

        protected virtual void OnIsLayoutUpdatedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!IsLayoutUpdated)
                InvalidateLines();
        }

        protected virtual void OnTextWrappingChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateLines();
        }

        protected virtual void OnTextChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateLines();
        }


        private void InvalidateLines()
        {
            _linesNeedUpdate = true;
        }

        /// <summary>
        /// Gets the text as lines, how they should be rendered.
        /// </summary>
        /// <returns>The text lines.</returns>
        /// <remarks>Note for Rendering layer implementers: Be sure to set Font.GetWidth before you call this method.</remarks>
        public string[] GetLines()
        {
            if(!IsLayoutUpdated)
                throw new InvalidOperationException("First the layout needs to be updated.");
            
            if (_linesNeedUpdate)
            {
                if (Font.GetWidth == null)
                    throw new InvalidOperationException("The used font does not have a GetWidth function set.");
                
                var wrapper = new TextWrapper();
                wrapper.GetWidth = Font.GetWidth;
                wrapper.TextWrapping = TextWrapping;

                _lines = wrapper.Wrap(Text, Position.Width - Padding.LeftAndRight);
                _linesNeedUpdate = false;

                TextMeasured?.Invoke(this, EventArgs.Empty);
            }

            return _lines;
        }

        public override void Arrange(Size availableSize)
        {
            base.Arrange(availableSize);
            
            TextClippingArea.X = Position.X + Padding.Left;
            TextClippingArea.Y = Position.Y + Padding.Top;
            TextClippingArea.Width = Position.Width - Padding.LeftAndRight;
            TextClippingArea.Height = Position.Height - Padding.TopAndBottom;
        }

        /// <summary>
        /// Gets the width of the longest line (rendered).
        /// </summary>
        /// <returns>The width of the longest line or 0 if Text is null or empty or -1 if Font.GetWidth is not set.</returns>
        public float GetLongestLineWidth()
        {
            if (string.IsNullOrEmpty(Text))
                return 0;

            if (Font.GetWidth == null)
                return -1;

            var lines = Text.Split('\n');
            return lines.Max(Font.GetWidth);
        }

        /// <summary>
        /// Sets the height of the control to it's content.
        /// </summary>
        /// <returns>The new height, 0 if Text = null or empty, -1 if Font.LineHeight is &lt;= 0 or IsLayoutUpdated is false.</returns>
        public void SetHeightToContent()
        {
            if (!IsLayoutUpdated)
                throw new InvalidOperationException("First the layout needs to be updated.");
            
            if(Font.LineHeight == null)
                throw new InvalidOperationException("The renderer should set " + nameof(Font) + "." + nameof(Font.LineHeight) + ".");

            var lineHeight = Font.LineHeight.Value;
            
            if (string.IsNullOrEmpty(Text))
                Position.Height = lineHeight;
            else
            {
                var lines = GetLines();
                var height = lines.Length * lineHeight;
                Position.Height = height + Padding.TopAndBottom;
            }
        }

        public override string ToString()
        {
            return GetType().FullName + " '" + Text + "'";
        }
    }
}
