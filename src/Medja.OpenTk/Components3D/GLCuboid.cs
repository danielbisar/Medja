using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public class GLCuboid : GLModel
    {
        private readonly GLMesh _mesh;
        
        public GLCuboid(float xLength, float yLength, float zLength)
        {
            _mesh = new GLMesh();
            _mesh.PrimitiveType = PrimitiveType.Quads;

            _mesh.AddVertex(0, 0, 0);
            _mesh.AddVertex(xLength, 0, 0);
            _mesh.AddVertex(xLength, yLength, 0);
            _mesh.AddVertex(0, yLength, 0);

            _mesh.AddVertex(0, 0, zLength);
            _mesh.AddVertex(xLength, 0, zLength);
            _mesh.AddVertex(xLength, yLength, zLength);
            _mesh.AddVertex(0, yLength, zLength);

            _mesh.AddIndices(
                0, 1, 2, 3, // front
                1, 5, 6, 2, // right
                5, 4, 7, 6, // back
                0, 4, 7, 3, // left
                3, 2, 6, 7, // top
                0, 4, 5, 1); // bottom

            _mesh.CreateBuffers();
        }

        public override void Render()
        {
            _mesh.Render();
        }
    }
}