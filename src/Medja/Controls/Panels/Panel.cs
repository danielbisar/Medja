using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Medja.Primitives;
using System.Linq;
using Medja.Properties;

namespace Medja.Controls
{
	public class Panel : Control
	{		
		public ObservableCollection<Control> Children { get; }
		public Thickness Padding { get; set; }

		public readonly Property<bool> PropertyDisposeChildren;

		/// <summary>
		/// Gets or sets if a child control should be disposed in <see cref="Dispose"/> and on remove from the panel.
		/// Default = true.
		/// </summary>
		public bool DisposeChildren
		{
			get { return PropertyDisposeChildren.Get(); }
			set { PropertyDisposeChildren.Set(value); }
		}

		/// <summary>
		/// Do use subclasses instead. This is just for testing purposes.
		/// </summary>
		public Panel()
		{
			Children = new ObservableCollection<Control>();
			Children.CollectionChanged += OnChildrenCollectionChanged;
			
			Padding = new Thickness();
			PropertyDisposeChildren = new Property<bool>();
			PropertyDisposeChildren.SetSilent(true);
			
			PropertyIsEnabled.PropertyChanged += OnIsEnabledChanged;
			PropertyIsLayoutUpdated.PropertyChanged += OnIsLayoutUpdatedChanged;
			
			ClippingArea.PropertyHeight.PropertyChanged += OnClippingAreaChanged;
			ClippingArea.PropertyWidth.PropertyChanged += OnClippingAreaChanged;
			ClippingArea.PropertyX.PropertyChanged += OnClippingAreaChanged;
			ClippingArea.PropertyY.PropertyChanged += OnClippingAreaChanged;
		}

		protected virtual void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				// todo? - see local clear
				foreach(var item in Children)
					OnItemAdded(item);
			}
			else
			{
				if (e.NewItems != null)
				{
					foreach (var item in e.NewItems.Cast<Control>())
						OnItemAdded(item);
				}

				if (e.OldItems != null)
				{
					foreach (var item in e.OldItems.Cast<Control>())
						OnItemRemoved(item);
				}
			}
			
			IsLayoutUpdated = false;
		}

		protected virtual void OnItemAdded(Control child)
		{
			if(!ClippingArea.IsEmpty)
				ForwardClippingArea(child);

			ForwardIsEnabled(child);
			child.Parent = this;
		}

		protected virtual void OnItemRemoved(Control child)
		{
			child.Parent = null;
			
			if(DisposeChildren)
				child.Dispose();
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
		
		public void Clear()
		{
			// do not use Children.Clear(), because then you would only get a reset event and Old and NewItems of will be empty
			while(Children.Count > 0)
				Children.RemoveAt(Children.Count - 1);
		}

		protected override void Dispose(bool disposing)
		{
			Clear(); // will cause dispose of children if this object is configured like this
			base.Dispose(disposing);
		}
	}
}
