using OpenTK;
using OpenTK.Graphics;

namespace MultiOpenGLContext
{
    public struct Vertex
    {
        public const int Size = 4*sizeof(float) + 4*sizeof(float) + 2*sizeof(float);

        private readonly Vector4 _position;
        private readonly Color4 _color;
        private readonly Vector2 _texCoords;
        
        public Vertex(Vector4 position, Color4 color)
        {
            _position = position;
            _color = color;
            _texCoords = Vector2.Zero;
        }

        public Vertex(Vector4 position, Vector2 texCoords)
        {
            _position = position;
            _color = Color4.Black;
            _texCoords = texCoords;
        }
    }
}