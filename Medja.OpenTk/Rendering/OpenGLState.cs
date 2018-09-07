using System;
using OpenTK.Graphics.OpenGL;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// This class is specific for SkiaSharp and how it modifies the state of opengl
    /// </summary>
    public class OpenGLState
    {
        public static void KeepState(Action action)
        {
            var state = new OpenGLState();
            state.Save();
            action();
            state.Restore();
        }

        private bool _isSaved;

        private bool _isDepthBufferWriteEnabled;
        private bool _isDepthTestEnabled;
        private bool _isProgramPointSizeEnabled;
        private int _progamId;  

        public void Save()
        {
            if(_isSaved)
                throw new InvalidOperationException("First call restore, old state already saved.");

            _isSaved = true;

            GL.PushClientAttrib(ClientAttribMask.ClientAllAttribBits);
            // todo how to get if depth buffer write is enabled? just assume it is
            // same for program (opentk seems not to allow this via getinteger)
            _isDepthBufferWriteEnabled = true;
            _isDepthTestEnabled = GL.IsEnabled(EnableCap.DepthTest);
            _progamId = 0;
            _isProgramPointSizeEnabled = GL.IsEnabled(EnableCap.ProgramPointSize);            
        }

        public void Restore()
        {
            if(!_isSaved)
                throw new InvalidOperationException("First call save.");

            GL.PopClientAttrib();
            GL.DepthMask(_isDepthBufferWriteEnabled);
            EnableOrDisable(EnableCap.DepthTest, _isDepthTestEnabled);
            GL.UseProgram(_progamId);
            EnableOrDisable(EnableCap.ProgramPointSize, _isProgramPointSizeEnabled);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            _isSaved = false;
        }

        private void EnableOrDisable(EnableCap cap, bool value)
        {
            if(value)
                GL.Enable(cap);
            else
                GL.Disable(cap);
        }
    }
}