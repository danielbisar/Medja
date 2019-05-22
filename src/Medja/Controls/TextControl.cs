using System;
using Medja.Primitives;
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
	public abstract class TextControl : Control
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

		protected TextControl()
		{
			PropertyText = new Property<string>();
			PropertyTextAlignment = new Property<TextAlignment>();
			PropertyTextWrapping = new Property<TextWrapping>();
			
			PropertyText.PropertyChanged += (s, e) => InvalidateLines();
			PropertyTextWrapping.PropertyChanged += (s, e) => InvalidateLines();
			PropertyIsLayoutUpdated.PropertyChanged += (s, e) =>
			{
				if(!IsLayoutUpdated)
					InvalidateLines();
			};
			
			Padding = new Thickness();
			TextClippingArea = new MRect();

			Font = new Font();
			_linesNeedUpdate = true;
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
	}
}
