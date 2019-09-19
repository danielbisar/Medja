using Medja.OpenTk.Components3D;
using Medja.Utils;
using Xunit;

namespace Medja.OpenTk.Test.Components3D
{
    public class GLLabelVertexFactoryTest
    {
        [Fact]
        public void CreatesCorrectIndices()
        {
            var factory = new GLLabelVertexFactory(new CharTexture(),"123", 2);
            var indices = factory.CreateIndices();
            
            MedjaAssert.Equal(new uint[]
            {
                0, 1, 2, 0, 2, 3, // plane 0
                4, 5, 6, 4, 6, 7, // plane 1
                8, 9,10, 8,10,11  // plane 2
            }, indices);
        }

        [Fact]
        public void CreatesCorrectVertices()
        {
            // Note: var letterWidth = 1;
            var factory = new GLLabelVertexFactory(new CharTexture(),"abc", 2);
            var vertices = factory.CreateVertices();

            MedjaAssert.Equal(new float[]
            {
                0, 0, 0,  // vertex 0 => letter at 0
                0, -2, 0, // vertex 1
                1, -2, 0, // vertex 2
                1, 0, 0,  // vertex 3
                
                1, 0, 0,  // vertex 4 => letter at 1
                1, -2, 0,  
                2, -2, 0,
                2,  0, 0,

                2, 0, 0, // vertex ... => letter at 2
                2, -2, 0,
                3, -2, 0,
                3, 0, 0
            }, vertices);
        }

        [Fact]
        public void CreateCorrectTextureCoordinates()
        {
            var texture = new CharTexture();
            texture.Coordinates['a'] = new CharTextureCoordinate
            {
                BottomLeft = new TextureCoordinate(0,0),
                BottomRight = new TextureCoordinate(1, 0),
                TopLeft = new TextureCoordinate(0, 1),
                TopRight = new TextureCoordinate(1, 1),
            };

            texture.Coordinates['b'] = new CharTextureCoordinate
            {
                BottomLeft = new TextureCoordinate(1, 0),
                BottomRight = new TextureCoordinate(2, 0),
                TopLeft = new TextureCoordinate(1, 1),
                TopRight = new TextureCoordinate(2, 1),
            };

            texture.Coordinates['c'] = new CharTextureCoordinate
            {
                BottomLeft = new TextureCoordinate(2, 0),
                BottomRight = new TextureCoordinate(3, 0),
                TopLeft = new TextureCoordinate(2, 1),
                TopRight = new TextureCoordinate(3, 1),
            };
            
            var factory = new GLLabelVertexFactory(texture, "abc", 2);
            var coordinates = factory.CreateTextureCoordinates();

            MedjaAssert.Equal(new float[]
            {
                0, 1, // vertex 0 => letter at 0
                0, 0, // vertex 1
                1, 0, // vertex 2
                1, 1, // vertex 3

                1, 1,  // vertex 4 => letter at 1
                1, 0,
                2, 0,
                2, 1, 

                2, 1,  // vertex ... => letter at 2
                2, 0,
                3, 0, 
                3, 1
            }, coordinates);
        }
    }
}