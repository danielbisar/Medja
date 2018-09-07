using System.Collections.Generic;
using Medja.Primitives;
using System.Linq;

namespace Medja.Controls
{
	public abstract class Panel : Control
	{
		public List<Control> Children { get; }
		public Thickness Padding { get; set; }

		protected Panel()
		{
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
				
				if (!child.PropertyIsEnabled.HasDefaultValue) 
					child.PropertyIsEnabled.SetDefault(child.IsEnabled);

				if (!isEnabled)
					child.IsEnabled = false;
				else
					child.PropertyIsEnabled.ResetAndClearWithDefault();
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
