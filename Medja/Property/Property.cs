﻿using System.Collections.Generic;

namespace Medja
{
	// TODO implement readonly version
	public class Property<T> : IProperty
	{
		// creating a static variable inside this class makes creation 3X as slow as currently
		private readonly EqualityComparer<T> _comparer;
		private T _value;
		private T _defaultValue;

		public bool HasDefaultValue { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public Property()
		{
			// see EqualityComparerCache header for info why
			_comparer = EqualityComparerCache<T>.Comparer;
		}
		
		public void Set(T value)
		{
			if (_comparer.Equals(_value, value))
				return;

			var oldValue = _value;
			_value = value;
			NotifyPropertyChanged(oldValue, value);
		}

		public void SetDefault(T value)
		{
			HasDefaultValue = true;
			_defaultValue = value;
		}

		public void ResetAndClearWithDefault()
		{
			Set(_defaultValue);
			HasDefaultValue = false;
		}

		public void UnnotifiedSet(T value)
		{
			_value = value;
		}

		public T Get()
		{
			return _value;
		}

		/// <summary>
		/// Manually call the property changed event, even though the property has not been changed.
		/// </summary>
		protected void NotifyPropertyChanged(object oldValue, object newValue)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(oldValue, newValue));
		}

		public void NotifyPropertyChanged()
		{
			NotifyPropertyChanged(_value, _value);
		}
	}
}
