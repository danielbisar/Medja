using Medja.Primitives;

namespace Medja.Controls
{
	public class TextControl : Control
	{
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

		public readonly Property<Color> PropertyForeground;
		public Color Foreground
		{
			get { return PropertyForeground.Get(); }
			set { PropertyForeground.Set(value); }
		}

		public Font Font { get; }

		public TextControl()
		{
			PropertyText = new Property<string>();
			PropertyTextWrapping = new Property<TextWrapping>();
			PropertyForeground = new Property<Color>();
			PropertyForeground.UnnotifiedSet(Colors.Black);

			Font = new Font();
		}
	}
}
