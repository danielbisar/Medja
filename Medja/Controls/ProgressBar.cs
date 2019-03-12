using Medja.Primitives;

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
		
		public readonly Property<Color> PropertyForeground;
		public Color Foreground
		{
			get { return PropertyForeground.Get(); }
			set { PropertyForeground.Set(value); }
		}

		public ProgressBar()
		{
			PropertyMaxValue = new Property<float>();
			PropertyMaxValue.UnnotifiedSet(100);

			PropertyValue = new Property<float>();
			PropertyValue.PropertyChanged += OnValueChanged;
			
			PropertyForeground = new Property<Color>();
		}

		private void OnValueChanged(object sender, PropertyChangedEventArgs e)
		{
			var value = (float) e.NewValue;
			
			if (value > MaxValue)
				Value = MaxValue;
			else if (value < 0)
				Value = 0;
		}
	}
}
