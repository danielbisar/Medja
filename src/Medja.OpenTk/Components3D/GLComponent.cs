using System;

namespace Medja.OpenTk.Components3D
{
    public class GLComponent : IDisposable
    {
        protected GLComponent()
        {
        }

        public virtual void Render()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}