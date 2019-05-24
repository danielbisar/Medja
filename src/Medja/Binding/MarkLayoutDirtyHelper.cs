using System;
using Medja.Controls;

namespace Medja
{
    public class MarkLayoutDirtyHelper<T> : IDisposable
    {
        private readonly Control _control;
        private readonly Property<T> _property;
        private bool _isDisposed;

        public MarkLayoutDirtyHelper(Control control, Property<T> property)
        {
            _control = control;
            _property = property;
            _property.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs eventargs)
        {
            _control.IsLayoutUpdated = false;
        }

        public void Dispose()
        {
            if (_isDisposed) 
                return;
            
            _isDisposed = true;
            _property.PropertyChanged -= OnPropertyChanged;
        }
    }
}