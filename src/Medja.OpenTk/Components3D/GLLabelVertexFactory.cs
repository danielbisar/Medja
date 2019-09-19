namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Creates the vertices needed by GLLabel for a 2D plane.
    /// </summary>
    /// <remarks>
    /// vertices are created like this             
    /// 0_2_4
    /// |_|_|
    /// 1 3 5
    /// 
    /// This allows indices to be (so that we have always counter clock wise order)
    /// 
    /// 0 1 3 | 0 3 2  = first letter plane
    /// 2 3 5 | 2 5 4  = seconds letter (each initial index + 2)
    /// ...
    /// 
    /// Labels vertex 0 is at x = 0, y = 0, z = 0
    /// </remarks>
    public class GLLabelVertexFactory
    {
        private readonly string _text;
        private readonly int _letterHeight;
        private readonly float[] _vertices;
        private readonly uint[] _indices;
        private int _currentVertexArrayPos;
        
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="text">The text to create planes for.</param>
        /// <param name="letterHeight">The height of a letter. Note: the value is negated; the upper vertex is at y = 0,
        /// the lower at y = -letterHeight.</param>
        public GLLabelVertexFactory(string text, int letterHeight)
        {
            _text = text;
            _letterHeight = -letterHeight;
            
            // 3 float values per vertex, 2 per letter + 2 as starting pos (vertex 0 and 1)
            _vertices = new float[3 * ((_text.Length*2) + 2)];
            // 3 vertices per triangle, 2 triangles per plane => 6 * letter count
            _indices = new uint[6 * _text.Length];
            
            _currentVertexArrayPos = 0;
        }
        
        public float[] CreateVertices()
        {
            float x = 0;
            float letterWidth = 1;

            // add vertex 0 and 1
            AddLetterVertices(x);

            for (int i = 0; i < _text.Length; i++)
            {
                // TODO for now we assume monospace
                x += letterWidth;
                AddLetterVertices(x);
            }

            return _vertices;
        }

        public uint[] CreateIndices()
        {
            int indexPos = 0;
            
            for (uint i = 0; i < _text.Length; i++)
            {
                // see class remarks section for why the indices are like this
                _indices[indexPos++] = 0 + (i * 2);
                _indices[indexPos++] = 1 + (i * 2);
                _indices[indexPos++] = 3 + (i * 2);

                _indices[indexPos++] = 0 + (i * 2);
                _indices[indexPos++] = 3 + (i * 2);
                _indices[indexPos++] = 2 + (i * 2);
            }
            
            return _indices;
        }

        /// <summary>
        /// Add two (up and down) vertices for the a 4 vertices letter plane at position x
        /// </summary>
        /// <param name="x">The x coordinate to add the two points.</param>
        private void AddLetterVertices(float x)
        {
            // vertex 0
            _vertices[_currentVertexArrayPos++] = x; // x
            _vertices[_currentVertexArrayPos++] = 0; // y
            _vertices[_currentVertexArrayPos++] = 0; // z

            // vertex 1
            _vertices[_currentVertexArrayPos++] = x;
            _vertices[_currentVertexArrayPos++] = _letterHeight;
            _vertices[_currentVertexArrayPos++] = 0;
        }
    }
}