using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Medja.OpenTk.Rendering
{
    public class GLSphere
    {
        private readonly VertexBufferObject _vbo;

        public GLSphere()
        {
            _vbo = new VertexBufferObject();
        }

        public void Create(double r, int latitudes, int longitudes)
        {
            var vertices = new List<Vector3>();
            
            for (var i = 0; i <= latitudes; i++)
            {
                var lat0 = Math.PI * (-0.5 + (double) (i - 1) / latitudes);
                var z0 = Math.Sin(lat0);
                var zr0 = Math.Cos(lat0);

                var lat1 = Math.PI * (-0.5 + (double) i / latitudes);
                var z1 = Math.Sin(lat1);
                var zr1 = Math.Cos(lat1);

                for (var j = 0; j <= longitudes; j++)
                {
                    double lng = 2 * Math.PI * (j - 1) / longitudes;
                    double x = Math.Cos(lng);
                    double y = Math.Sin(lng);

                    
                    //GL.Normal3(x * zr0, y * zr0, z0);
                    //GL.Vertex3(x * zr0, y * zr0, z0);
                    vertices.Add(new Vector3((float)(x*zr0), (float) (y * zr0),(float) z0));
                    vertices.Add(new Vector3((float) (x * zr0), (float) (y * zr0), (float) z0));
                    
                        //GL.Normal3(x * zr1, y * zr1, z1);
                    //GL.Vertex3(x * zr1, y * zr1, z1);
                    vertices.Add(new Vector3((float) (x * zr1), (float) (y * zr1), (float) z1));
                    vertices.Add(new Vector3((float) (x * zr1), (float) (y * zr1), (float) z1));
                }
            }

            _vbo.DataType = VBODataType.VerticesAndNormals;
            _vbo.UpdateData(vertices);
        }

        public void Draw()
        {
            //GL.Translate(Position);
            _vbo.Draw(PrimitiveType.QuadStrip);
        }
    }
}