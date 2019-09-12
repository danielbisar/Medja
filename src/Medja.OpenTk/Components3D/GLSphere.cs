using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Medja.OpenTk.Components3D
{
    public class GLSphere : GLModel
    {
        private GLMesh _mesh;

        public GLSphere()
        {
            CreateMesh();
        }

        private void CreateMesh()
        {
            // create the vertices for an icosahedron
            // the benefit is that you don't have the problem of having closer smaller triangles
            // at the pols but the triangles are all equilateral
            var t = (float) ((1.0 + Math.Sqrt(5)) / 2.0);
            const float s = 1.0f;

            _mesh = new GLMesh();

            _mesh.AddVertex(-s, t, 0);
            _mesh.AddVertex(s, t, 0);
            _mesh.AddVertex(-s, -t, 0);
            _mesh.AddVertex(s, -t, 0);

            _mesh.AddVertex(0, -s, t);
            _mesh.AddVertex(0, s, t);
            _mesh.AddVertex(0, -s, -t);
            _mesh.AddVertex(0, s, -t);

            _mesh.AddVertex(t, 0, -s);
            _mesh.AddVertex(t, 0, s);
            _mesh.AddVertex(-t, 0, -s);
            _mesh.AddVertex(-t, 0, s);
            _mesh.NormalizeVectors();
            
            _mesh.AddIndices(
                0, 11, 5,
                0, 5, 1,
                0, 1, 7,
                0, 7, 10,
                0, 10, 11,
                1, 5, 9,
                5, 11, 4,
                11, 10, 2,
                10, 7, 6,
                7, 1, 8,
                3, 9, 4,
                3, 4, 2,
                3, 2, 6,
                3, 6, 8,
                3, 8, 9,
                4, 9, 5,
                2, 4, 11,
                6, 2, 10,
                8, 6, 7,
                9, 8, 1
                );
            
            _mesh.PrimitiveType = PrimitiveType.Triangles;
            _mesh.SubdivideFaces(GetMiddle);
            _mesh.SubdivideFaces(GetMiddle);
            _mesh.CreateBuffers();
        }

        private Vector3 GetMiddle(Vector3 v1, Vector3 v2)
        {
            var mid = new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
            mid.Normalize();

            return mid;
        }

        public override void RenderModel()
        {
            base.RenderModel();
            _mesh.RenderModel();
        }
    }
}