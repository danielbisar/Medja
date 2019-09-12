using System;
using Medja.Controls;
using Medja.Primitives;
using Medja.Properties;

namespace Medja.Theming
{
    public abstract class ControlRendererBase<TContext, TControl> : IControlRenderer
        where TContext : class
        where TControl : Control
    {
        public bool IsInitialized { get; private set; }
        
        protected TControl _control;
        
        protected ControlRendererBase(TControl control)
        {
            _control = control ?? throw new ArgumentNullException(nameof(control));
            _control.Position.PropertyX.PropertyChanged += OnPositionChanged;
            _control.Position.PropertyY.PropertyChanged += OnPositionChanged;
            _control.Position.PropertyWidth.PropertyChanged += OnPositionChanged;
            _control.Position.PropertyHeight.PropertyChanged += OnPositionChanged;
        }

        private void OnPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            Resize(_control.Position);
        }

        public virtual void Initialize()
        {
            IsInitialized = true;
        }

        public virtual void Resize(MRect position)
        {
        }
        
        protected abstract void Render(TContext context);

        public void Render(object context)
        {
            Render(context as TContext);
        }

        protected virtual void Dispose(bool disposing)
        {
            _control = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
