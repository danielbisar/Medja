namespace Medja.Controls
{
	public class ProgressBar : Control
	{
		public readonly Property<float> PropertyMaxValue;
		public float MaxValue
		{
			get { return PropertyMaxValue.Get(); }
			set { PropertyMaxValue.Set(value); }
		}

		public readonly Property<float> PropertyValue;
		public float Value
		{
			get { return PropertyValue.Get(); }
			set { PropertyValue.Set(value); }
		}

		public float Percentage
		{
			get { return Value == 0 || MaxValue == 0 ? 0 : Value / MaxValue; }
		}

		public ProgressBar()
		{
			PropertyMaxValue = new Property<float>();
			PropertyMaxValue.UnnotifiedSet(100);

			PropertyValue = new Property<float>();
		}
	}
}
