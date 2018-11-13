using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Medja.Primitives;
using System.Linq;

namespace Medja.Controls
{
	public abstract class Panel : Control
	{		
		public ObservableCollection<Control> Children { get; }
		public Thickness Padding { get; set; }

		protected Panel()
		{
			Children = new ObservableCollection<Control>();
			Children.CollectionChanged += OnChildrenCollectionChanged;
			
			Padding = new Thickness();
			PropertyIsEnabled.PropertyChanged += OnIsEnabledChanged;
			PropertyIsLayoutUpdated.PropertyChanged += OnIsLayoutUpdatedChanged;
			
			ClippingArea.PropertyHeight.PropertyChanged += OnClippingAreaChanged;
			ClippingArea.PropertyWidth.PropertyChanged += OnClippingAreaChanged;
			ClippingArea.PropertyX.PropertyChanged += OnClippingAreaChanged;
			ClippingArea.PropertyY.PropertyChanged += OnClippingAreaChanged;
		}

		protected virtual void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var item in e.NewItems.Cast<Control>())
				{
					OnItemAdded(item);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				foreach(var item in Children)
					OnItemAdded(item);
			}

			IsLayoutUpdated = false;
		}

		protected virtual void OnItemAdded(Control child)
		{
			if(!ClippingArea.IsEmpty)
				ForwardClippingArea(child);

			ForwardIsEnabled(child);
		}

		private void ForwardClippingArea(Control child)
		{
			child.ClippingArea.Width = ClippingArea.Width;
			child.ClippingArea.Height = ClippingArea.Height;
			child.ClippingArea.X = ClippingArea.X;
			child.ClippingArea.Y = ClippingArea.Y;
		}

		private void OnClippingAreaChanged(object sender, PropertyChangedEventArgs e)
		{
			foreach (var child in Children)
			{
				ForwardClippingArea(child);
			}
		}

		private void OnIsLayoutUpdatedChanged(object sender, PropertyChangedEventArgs e)
		{
			foreach (var child in Children)
				child.IsLayoutUpdated = (bool)e.NewValue;
		}

		private void OnIsEnabledChanged(object sender, PropertyChangedEventArgs eventArgs)
		{
			foreach (var child in Children)
			{
				if (child == null) 
					continue;

				ForwardIsEnabled(child);
			}
		}

		private void ForwardIsEnabled(Control child)
		{
			if (IsEnabled)
				child.PropertyIsEnabled.ClearOverwrittenValue();
			else
				child.PropertyIsEnabled.OverwriteSet(false);
		}

		public override IEnumerable<Control> GetChildren()
		{
			return Children;
		}

		public override void UpdateAnimations()
		{
			base.UpdateAnimations();

			foreach (var child in Children)
				child.UpdateAnimations();
		}
	}
}
