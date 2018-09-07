namespace Medja.Primitives
{
	/// <summary>
	/// M for MedjaObject
	/// </summary>
	public class MPosition : MObject
	{
		public Property<float> PropertyX { get; }
		public Property<float> PropertyY { get; }
		public Property<float> PropertyWidth { get; }
		public Property<float> PropertyHeight { get; }

		public float X
		{
			get { return PropertyX.Get(); }
			set { PropertyX.Set(value); }
		}

		public float Y
		{
			get { return PropertyY.Get(); }
			set { PropertyY.Set(value); }
		}

		public float Width
		{
			get { return PropertyWidth.Get(); }
			set { PropertyWidth.Set(value); }
		}

		public float Height
		{
			get { return PropertyHeight.Get(); }
			set { PropertyHeight.Set(value); }
		}

		public MPosition()
		{
			PropertyX = new Property<float>();
			PropertyY = new Property<float>();
			PropertyWidth = new Property<float>();
			PropertyHeight = new Property<float>();
		}

		public override string ToString()
		{
			return "X = " + X + ", Y = " + Y + ", Width = " + Width + ", Height = " + Height;
		}
	}
}
