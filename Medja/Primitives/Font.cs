namespace Medja.Primitives
{
	public class Font
	{
		public readonly Property<string> PropertyName;
		public string Name
		{
			get { return PropertyName.Get(); }
			set { PropertyName.Set(value); }
		}

		public readonly Property<float> PropertySize;
		public float Size
		{
			get { return PropertySize.Get(); }
			set { PropertySize.Set(value); }
		}

		public readonly Property<FontStyle> PropertyStyle;
		public FontStyle Style
		{
			get { return PropertyStyle.Get(); }
			set { PropertyStyle.Set(value); }
		}

		public Font()
		{
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
