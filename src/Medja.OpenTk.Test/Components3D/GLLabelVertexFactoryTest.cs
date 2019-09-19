using Medja.OpenTk.Components3D;
using Xunit;

namespace Medja.OpenTk.Test.Components3D
{
    public class GLLabelVertexFactoryTest
    {
        [Fact]
        public void CreatesCorrectIndices()
        {
            var factory = new GLLabelVertexFactory("123", 2);
            var indices = factory.CreateIndices();
            
            Assert.Equal(new uint[]
            {
                0, 1, 3, 0, 3, 2, // plane 0
                2,3,5,2,5,4,      // plane 1
                4,5,7,4,7,6 // plane 2
            }, indices);
        }

        [Fact]
        public void CreatesCorrectVertices()
        {
            // Note: var letterWidth = 1;
            var factory = new GLLabelVertexFactory("123", 2);
            var vertices = factory.CreateVertices();

            Assert.Equal(new float[]
            {
                0, 0, 0,  // vertex 0 => start letter 0
                0, -2, 0, // vertex 1 
                1, 0, 0,  // vertex 2 => shared between letter 0 and 1
                1, -2, 0, // vertex 3
                2, 0, 0,  // ... => shared between letter 1 and 2
                2, -2, 0,
                3, 0, 0,  // ... => last letter
                3, -2, 0
            }, vertices);
        }
    }
}