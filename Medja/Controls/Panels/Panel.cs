using System.Collections.Generic;
using Medja.Primitives;
using System.Linq;

namespace Medja.Controls
{
	public abstract class Panel : Control
	{		
		private readonly Dictionary<Property<bool>, PropertyValueStorage<bool>> _isEnabledValues;

		public List<Control> Children { get; }
		public Thickness Padding { get; set; }

		protected Panel()
		{
			_isEnabledValues = new Dictionary<Property<bool>, PropertyValueStorage<bool>>();
			Children = new List<Control>();
			Padding = new Thickness();
			PropertyIsEnabled.PropertyChanged += OnIsEnabledChanged;
		}

		private void OnIsEnabledChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			var isEnabled = IsEnabled;

			foreach (var child in Children)
			{
				if (child == null) 
					continue;

				if (!_isEnabledValues.TryGetValue(child.PropertyIsEnabled, out var oldValue))
				{
					oldValue = new PropertyValueStorage<bool>(child.PropertyIsEnabled);
					_isEnabledValues.Add(child.PropertyIsEnabled, oldValue);
				}

				if (!isEnabled)
					child.IsEnabled = false;
				else
					oldValue.Restore();
			}
		}

		public override IEnumerable<Control> GetAllControls()
		{
			yield return this;

			foreach (var control in Children)
			{
				// GetAllControls also returns self
				foreach (var subControl in control.GetAllControls())
					yield return subControl;
			}
		}

		public override void UpdateAnimations()
		{
			base.UpdateAnimations();

			foreach (var child in Children)
				child.UpdateAnimations();
		}
	}
}
