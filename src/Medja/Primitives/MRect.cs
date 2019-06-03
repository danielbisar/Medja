using Medja.Properties;

namespace Medja.Primitives
{
	/// <summary>
	/// M for MedjaObject
	/// </summary>
	public class MRect : Rect
	{
		public readonly Property<float> PropertyX;
		public readonly Property<float> PropertyY;
		public readonly Property<float> PropertyWidth;
		public readonly Property<float> PropertyHeight;

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

		public bool IsEmpty
		{
			get { return X == 0 && Y == 0 && Width == 0 && Height == 0; }
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
