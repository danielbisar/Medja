using Medja.Primitives;
using Medja.Properties;

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

		public readonly Property<float> PropertyPercentage;
		public float Percentage
		{
			get { return PropertyPercentage.Get(); }
		}
		
		public readonly Property<Color> PropertyProgressColor;
		public Color ProgressColor
		{
			get { return PropertyProgressColor.Get(); }
			set { PropertyProgressColor.Set(value); }
		}

		public ProgressBar()
		{
			PropertyMaxValue = new Property<float>();
			PropertyMaxValue.SetSilent(100);

			PropertyValue = new Property<float>();
			PropertyValue.PropertyChanged += OnValueChanged;
			PropertyPercentage = new Property<float>();
			
			PropertyProgressColor = new Property<Color>();
		}

		private void OnValueChanged(object sender, PropertyChangedEventArgs e)
		{
			var value = (float) e.NewValue;
			
			if (value > MaxValue)
				Value = MaxValue;
			else if (value < 0)
				Value = 0;
			
			PropertyPercentage.Set(Value == 0 || MaxValue == 0 ? 0 : Value / MaxValue);
		}
	}
}
