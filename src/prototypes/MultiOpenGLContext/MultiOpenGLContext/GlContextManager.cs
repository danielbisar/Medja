using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform;

namespace MultiOpenGLContext
{
    /// <summary>
    /// Manages multiple OpenGL contexts. (f.e. handle resizes etc, setup, etc)
    /// </summary>
    public class GlContextManager : IDisposable
    {
        private readonly IWindowInfo _windowInfo;
        
        private readonly Context _windowContext;

        public Context WindowContext
        {
            get { return _windowContext; }
        }
        
        private readonly List<Context> _contexts;
        private int _width;
        private int _height;

        public GlContextManager(IWindowInfo windowInfo, IGraphicsContext windowContext)
        {
            _windowInfo = windowInfo ?? throw new ArgumentNullException(nameof(windowInfo));
            _contexts = new List<Context>();

            _windowContext = Add(windowContext);
        }

        public Context CreateWindowContext()
        {
            return Add(new GraphicsContext(GraphicsMode.Default, _windowInfo, 4, 2, GraphicsContextFlags.ForwardCompatible));
        }

        private Context Add(IGraphicsContext context)
        {
            var result = new Context(context);
            _contexts.Add(result);
            
            return result;
        }

        public void Init()
        {
            foreach (var context in _contexts)
            {
                context.MakeCurrent(_windowInfo);
                context.Actions.OnInit?.Invoke();
            }
        }

        public void Resize(int width, int height)
        {
            _width = width;
            _height = height;
            
            OnResize();
        }

        private void OnResize()
        {
            foreach (var context in _contexts)
            {
                context.MakeCurrent(_windowInfo);
                context.Actions.OnResize?.Invoke(_width, _height);
            }
        }

        public void Render()
        {
            foreach (var context in _contexts)
            {
                context.MakeCurrent(_windowInfo);
                context.Actions.OnRender?.Invoke();
                //GL.Flush();
            }
            
            /*foreach(var context in _contexts)
                context.SwapBuffers();*/
            
            _contexts[_contexts.Count - 1].SwapBuffers();
        }

        public void Dispose()
        {
            foreach(var context in _contexts)
                context?.Dispose();
            
            _contexts.Clear();
            _contexts.TrimExcess();
        }
    }

    public class ContextActions
    {
        public Action OnInit;
        public Action<int, int> OnResize;
        public Action OnRender;
    }

    public class Context : IDisposable
    {
        public readonly IGraphicsContext _context;
        public readonly ContextActions Actions;

        public Context(IGraphicsContext context)
        {
            Actions = new ContextActions();
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void MakeCurrent(IWindowInfo windowInfo)
        {
            _context.MakeCurrent(windowInfo);
        }

        public void SwapBuffers()
        {
            _context.SwapBuffers();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}