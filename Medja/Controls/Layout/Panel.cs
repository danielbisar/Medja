using System.Collections.Generic;
using Medja.Controls;
using Medja.Primitives;

namespace Medja.Controls
{
	public abstract class Panel : Control
	{
		public List<Control> Children { get; }

		public Thickness Margin { get; set; }
		public Thickness Padding { get; set; }

		protected Panel()
		{
			Children = new List<Control>();
			Margin = new Thickness();
			Padding = new Thickness();
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
