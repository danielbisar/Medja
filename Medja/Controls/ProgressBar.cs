namespace Medja.Controls
{
	public class ProgressBar : Control
	{
		public Property<float> MaxValueProperty;
		public float MaxValue
		{
			get { return MaxValueProperty.Get(); }
			set { MaxValueProperty.Set(value); }
		}

		public Property<float> ValueProperty;
		public float Value
		{
			get { return ValueProperty.Get(); }
			set { ValueProperty.Set(value); }
		}

		public float Percentage
		{
			get { return Value == 0 ? 0 : Value / MaxValue; }
		}

		public ProgressBar()
		{
			MaxValueProperty = new Property<float>();
			MaxValueProperty.UnnotifiedSet(100);

			ValueProperty = new Property<float>();
		}
	}
}
