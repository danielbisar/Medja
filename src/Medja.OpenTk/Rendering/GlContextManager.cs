using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform;

namespace Medja.OpenTk
{
    /// <summary>
    /// Manages multiple OpenGL contexts. (f.e. handle resizes etc, setup, etc)
    /// </summary>
    public class GlContextManager : IDisposable
    {
        private readonly IWindowInfo _windowInfo;
        private readonly List<GlContext> _contexts;
        public IReadOnlyList<GlContext> Contexts { get {return _contexts;}}
        
        private int _width;
        private int _height;

        public GlContextManager(IWindowInfo windowInfo)
        {
            _windowInfo = windowInfo;
            _contexts = new List<GlContext>();
        }

        public GlContext Create()
        {
            return Add(new GraphicsContext(GraphicsMode.Default, _windowInfo, 4, 2, GraphicsContextFlags.ForwardCompatible));
        }

        public GlContext Add(IGraphicsContext context)
        {
            var result = new GlContext(context);
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

        /*public void Resize(int width, int height)
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
        }*/

        public void Render()
        {
            var needsUpdate = false;
            
            foreach (var context in _contexts)
            {
                context.MakeCurrent(_windowInfo);
                needsUpdate |= context.Actions.OnRender.Invoke();
                GL.Flush();
            }
            
            if(needsUpdate)
                _contexts[_contexts.Count - 1].SwapBuffers();
        }

        public void Execute(int context, Action action)
        {
            _contexts[context].MakeCurrent(_windowInfo);
            action();
        }

        public void Dispose()
        {
            foreach(var context in _contexts)
                context?.Dispose();
            
            _contexts.Clear();
            _contexts.TrimExcess();
        }

        public void MakeCurrent(GlContext context)
        {
            context.MakeCurrent(_windowInfo);
        }
    }
}