using System.Collections.Generic;
using Medja.Primitives;

namespace Medja.Controls
{
	/// <summary>
	/// A control that allows dialogs and message boxes to be presented.
	/// </summary>
	public class DialogParentControl : ContentControl
	{
		public readonly Property<Dialog> PropertyDialogControl;
		public Dialog DialogControl
		{
			get { return PropertyDialogControl.Get(); }
			set { PropertyDialogControl.Set(value); }
		}

		public readonly Property<bool> PropertyIsDialogVisible;
		public bool IsDialogVisible
		{
			get { return PropertyIsDialogVisible.Get(); }
			set { PropertyIsDialogVisible.Set(value); }
		}

		public Thickness DialogPadding { get; set; }

		public DialogParentControl()
		{
			PropertyDialogControl = new Property<Dialog>();
			PropertyDialogControl.PropertyChanged += OnDialogControlChanged;
			PropertyDialogControl.AffectsLayout(this);
			
			PropertyIsDialogVisible = new Property<bool>();
			PropertyIsDialogVisible.PropertyChanged += OnIsDialogVisibleChanged;
			DialogPadding = new Thickness(100);
		}

		private void OnIsDialogVisibleChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			// TODO this is just a quick solution to simulate modality
			if (Content != null)
				Content.IsEnabled = !IsDialogVisible;
		}

		private void OnDialogControlChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			var dialog = eventArgs.OldValue as Dialog;
			
			if (dialog != null)
				dialog.DialogParent = null;

			dialog = eventArgs.NewValue as Dialog;

			if (dialog != null)
				dialog.DialogParent = this;
		}

		public override void Arrange(Size availableSize)
		{
			base.Arrange(availableSize);
			
			if (IsDialogVisible && DialogControl != null)
			{
				var pos = DialogControl.Position;

				pos.X = Position.X + DialogPadding.Left;
				pos.Y = Position.Y + DialogPadding.Top;
				pos.Width = Position.Width - DialogPadding.LeftAndRight;
				pos.Height = Position.Height - DialogPadding.TopAndBottom;

				DialogControl.Arrange(new Size(pos.Width, pos.Height));
			}
		}

		public override IEnumerable<Control> GetChildren()
		{
			var children = base.GetChildren();

			foreach (var child in children)
				yield return child;

			if (DialogControl != null && IsDialogVisible)
				yield return DialogControl;
		}
	}
}
