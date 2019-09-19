namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Creates the vertices needed by GLLabel for a 2D plane.
    /// </summary>
    /// <remarks>
    /// Note: OpenGL does not allow multiple uv (texture) coordinates per vertex
    ///  
    /// Vertices are created like this (... is just for clearer visualisation; actually there is no space in between)
    /// 
    ///  0__3   4__7   
    ///  |  |...|  |...
    ///  |__|...|__|...
    ///  1  2   5  6
    /// 
    /// This allows indices to be (so that we have always counter clock wise order)
    /// 
    /// 0 1 2 | 0 2 3  = first letter plane
    /// 4 5 6 | 4 6 7  = seconds letter (each initial index + 4)
    /// ...
    /// 
    /// Labels vertex 0 is at x = 0, y = 0, z = 0
    /// </remarks>
    public class GLLabelVertexFactory
    {
        private readonly CharTexture _texture;
        private readonly string _text;
        private readonly int _letterHeight;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        private readonly float[] _textureCoordinates;
        private int _currentVertexArrayPos;
        private int _vertexCount;
        
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="text">The text to create planes for.</param>
        /// <param name="letterHeight">The height of a letter. Note: the value is negated; the upper vertex is at y = 0,
        /// the lower at y = -letterHeight.</param>
        public GLLabelVertexFactory(CharTexture texture, string text, int letterHeight)
        {
            _texture = texture;
            _text = text;
            _letterHeight = -letterHeight;
            
            const int valuesPerVertex = 3;
            const int verticesPerTriangle = 3;
            const int trianglesPerPlane = 2;
            const int textureCoordsPerVertex = 2;

            _vertexCount = _text.Length * 4;
            _vertices = new float[valuesPerVertex * _vertexCount];
            _indices = new uint[verticesPerTriangle * trianglesPerPlane * _text.Length];
            _textureCoordinates = new float[textureCoordsPerVertex * _vertexCount];
            
            _currentVertexArrayPos = 0;
        }
        
        public float[] CreateVertices()
        {
            float x = 0;
            float letterWidth = 1;

            for (int i = 0; i < _text.Length; i++)
            {
                AddLetterVertices(x, letterWidth);
                
                // TODO for now we assume monospace
                x += letterWidth;
            }

            return _vertices;
        }

        /// <summary>
        /// Add two (up and down) vertices for the a 4 vertices letter plane at position x
        /// </summary>
        /// <param name="x">The x coordinate to add the two points.</param>
        /// <param name="letterWidth">The width of the current letter in 3D units.</param>
        private void AddLetterVertices(float x, float letterWidth)
        {
            // vertex 0
            _vertices[_currentVertexArrayPos++] = x; // x
            _vertices[_currentVertexArrayPos++] = 0; // y
            _vertices[_currentVertexArrayPos++] = 0; // z

            // vertex 1
            _vertices[_currentVertexArrayPos++] = x;
            _vertices[_currentVertexArrayPos++] = _letterHeight;
            _vertices[_currentVertexArrayPos++] = 0;

            // vertex 3
            _vertices[_currentVertexArrayPos++] = x + letterWidth;
            _vertices[_currentVertexArrayPos++] = _letterHeight;
            _vertices[_currentVertexArrayPos++] = 0;

            // vertex 4
            _vertices[_currentVertexArrayPos++] = x + letterWidth;
            _vertices[_currentVertexArrayPos++] = 0;
            _vertices[_currentVertexArrayPos++] = 0;
        }

        public uint[] CreateIndices()
        {
            int indexPos = 0;
            
            for (uint i = 0; i < _text.Length; i++)
            {
                // see class remarks section for why the indices are like this
                _indices[indexPos++] = 0 + (i * 4);
                _indices[indexPos++] = 1 + (i * 4);
                _indices[indexPos++] = 2 + (i * 4);

                _indices[indexPos++] = 0 + (i * 4);
                _indices[indexPos++] = 2 + (i * 4);
                _indices[indexPos++] = 3 + (i * 4);
            }
            
            return _indices;
        }

        public float[] CreateTextureCoordinates()
        {
            int texCoordPos = 0;

            for (int i = 0; i < _text.Length; i++)
            {
                var coord = _texture.GetCoordinates(_text[i]);
                
                _textureCoordinates[texCoordPos++] = coord.TopLeft.U;
                _textureCoordinates[texCoordPos++] = coord.TopLeft.V;

                _textureCoordinates[texCoordPos++] = coord.BottomLeft.U;
                _textureCoordinates[texCoordPos++] = coord.BottomLeft.V;

                _textureCoordinates[texCoordPos++] = coord.BottomRight.U;
                _textureCoordinates[texCoordPos++] = coord.BottomRight.V;

                _textureCoordinates[texCoordPos++] = coord.TopRight.U;
                _textureCoordinates[texCoordPos++] = coord.TopRight.V;
            }
            
            return _textureCoordinates;
        }
    }
}