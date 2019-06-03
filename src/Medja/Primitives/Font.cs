using System;
using Medja.Properties;

namespace Medja.Primitives
{
	/// <summary>
	/// The description of a font or typeface including style, size and color.
	/// </summary>
	public class Font
	{
		public readonly Property<string> PropertyName;
		/// <summary>
		/// The name of the font (family) - the typeface name.
		/// </summary>
		public string Name
		{
			get { return PropertyName.Get(); }
			set { PropertyName.Set(value); }
		}

		public readonly Property<float> PropertySize;
		/// <summary>
		/// The size of the font in points. (default = 16)
		/// </summary>
		public float Size
		{
			get { return PropertySize.Get(); }
			set { PropertySize.Set(value); }
		}

		public readonly Property<FontStyle> PropertyStyle;
		/// <summary>
		/// Gets or sets the font style.
		/// </summary>
		public FontStyle Style
		{
			get { return PropertyStyle.Get(); }
			set { PropertyStyle.Set(value); }
		}

		public readonly Property<Color> PropertyColor;
		/// <summary>
		/// Gets or sets the color of the font.
		/// </summary>
		/// <remarks>Even if color is usually not an attribute of a font or typeface we included the color here to
		/// make the usage of this object more easy. If no color is set, the color is black.</remarks>
		public Color Color
		{
			get { return PropertyColor.Get(); }
			set { PropertyColor.Set(value); }
		}
		
		public Func<string, float> GetWidth { get; set; }

		public Font()
		{
			PropertyColor = new Property<Color>();
			PropertyColor.UnnotifiedSet(Colors.Black);
			PropertyName = new Property<string>();
			PropertySize = new Property<float>();
			PropertySize.UnnotifiedSet(16);
			PropertyStyle = new Property<FontStyle>();
		}

		public override int GetHashCode()
		{
			// TODO implementation could impact performance because
			// of collisions; should only be relevant with a lot of
			// values. Test alternatives described here
			// https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx

			var hash = 0;

			if (Name != null)
				hash ^= Name.GetHashCode();

			hash ^= Size.GetHashCode() ^ Style.GetHashCode();

			return hash;
		}
	}
}
