using OpenTK;
using OpenTK.Graphics;

namespace MultiOpenGLContext
{
    public struct Vertex
    {
        public const int Size = 4*sizeof(float) + 4*sizeof(float);

        private readonly Vector4 _position;
        private readonly Color4 _color;

        public Vertex(Vector4 position, Color4 color)
        {
            _position = position;
            _color = color;
        }
    }
}