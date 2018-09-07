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

		public Thickness Padding { get; set; }

		public ContentControl()
		{
			PropertyContent = new Property<Control>();
			Padding = new Thickness();
			PropertyIsEnabled.PropertyChanged += OnIsEnabledChanged;
		}

		private void OnIsEnabledChanged(IProperty property)
		{
			if (Content != null)
				Content.IsEnabled = IsEnabled;
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
				var pos = Content.Position;

				pos.X = Position.X + Padding.Left;
				pos.Y = Position.Y + Padding.Top;
				ArrangeContent();
			}
		}

		protected virtual void ArrangeContent()
		{
			if (Content != null)
			{
				var pos = Content.Position;

				pos.Width = Position.Width - Padding.LeftAndRight;

				var availableHeight = Position.Height - Padding.TopAndBottom;

				pos.Height = Content.VerticalAlignment == VerticalAlignment.Top
					|| Content.VerticalAlignment == VerticalAlignment.Bottom
					? pos.Height : availableHeight;

				Content.Arrange(new Size(pos.Width, pos.Height));
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
