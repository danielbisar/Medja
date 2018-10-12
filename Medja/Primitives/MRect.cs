namespace Medja.Primitives
{
	/// <summary>
	/// M for MedjaObject
	/// </summary>
	public class MRect : Rect
	{
		public Property<float> PropertyX { get; }
		public Property<float> PropertyY { get; }
		public Property<float> PropertyWidth { get; }
		public Property<float> PropertyHeight { get; }

		public override float X
		{
			get { return PropertyX.Get(); }
			set { PropertyX.Set(value); }
		}

		public override float Y
		{
			get { return PropertyY.Get(); }
			set { PropertyY.Set(value); }
		}

		public override float Width
		{
			get { return PropertyWidth.Get(); }
			set { PropertyWidth.Set(value); }
		}

		public override float Height
		{
			get { return PropertyHeight.Get(); }
			set { PropertyHeight.Set(value); }
		}

		public MRect()
		{
			PropertyX = new Property<float>();
			PropertyY = new Property<float>();
			PropertyWidth = new Property<float>();
			PropertyHeight = new Property<float>();
		}
	}
}
