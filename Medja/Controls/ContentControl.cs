﻿using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
	public class ContentControl : Control
	{
		public readonly Property<Control> PropertyContent;
		public Control Content
		{
			get { return PropertyContent.Get(); }
			set { PropertyContent.Set(value); }
		}

		public ContentControl()
		{
			PropertyContent = new Property<Control>();
		}

		public override Size Measure(Size availableSize)
		{
			if (Content == null)
				return base.Measure(availableSize);

			return Content.Measure(availableSize);
		}

		public override void Arrange(Size availableSize)
		{
			base.Arrange(availableSize);

			if (Content != null)
			{
				Content.Position.X = Position.X;
				Content.Position.Y = Position.Y;
				Content.Arrange(availableSize);
			}
		}

		public override IEnumerable<Control> GetAllControls()
		{
			yield return this;

			if (Content != null)
			{
				foreach (var control in Content.GetAllControls())
					yield return control;
			}
		}

		public override void UpdateAnimations()
		{
			base.UpdateAnimations();

			if (Content != null)
				Content.UpdateAnimations();
		}
	}
}
