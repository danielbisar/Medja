﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Medja.Primitives;
using System.Linq;

namespace Medja.Controls
{
	public abstract class Panel : Control
	{		
		private readonly Dictionary<Property<bool>, PropertyValueStorage<bool>> _isEnabledValues;

		public ObservableCollection<Control> Children { get; }
		public Thickness Padding { get; set; }

		protected Panel()
		{
			_isEnabledValues = new Dictionary<Property<bool>, PropertyValueStorage<bool>>();
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

		private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var isClippingEmpty = ClippingArea.IsEmpty;
			
			foreach (var item in e.NewItems.Cast<Control>())
			{
				if (!isClippingEmpty)
					ForwardClippingArea(item);

				if (!IsLayoutUpdated)
					item.IsLayoutUpdated = false;

				item.IsEnabled = IsEnabled;
				
				ForwardIsEnabled(item);
			}
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
			if (!_isEnabledValues.TryGetValue(child.PropertyIsEnabled, out var oldValue))
			{
				oldValue = new PropertyValueStorage<bool>(child.PropertyIsEnabled);
				_isEnabledValues.Add(child.PropertyIsEnabled, oldValue);
			}

			if (!IsEnabled)
				child.IsEnabled = false;
			else
				oldValue.Restore();
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
