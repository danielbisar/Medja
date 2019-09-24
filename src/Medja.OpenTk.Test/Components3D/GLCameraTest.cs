using Medja.OpenTk.Components3D;
using OpenTK;
using Xunit;
using Xunit.Abstractions;

namespace Medja.OpenTk.Test.Components3D
{
    public class GLCameraTest
    {
        private readonly ITestOutputHelper _output;

        public GLCameraTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void UpdatesViewMatrixOnPositionChanged()
        {
            var camera = new GLCamera();
            camera.Position = new Vector3(1,0,0);
            
            var first = new Matrix4(
                0, 0, 1, 0,
                0, 1, 0, 0,
               -1, 0, 0, 0,
                0, 0,-1, 1);

            Assert.Equal(first, camera.ViewMatrix);

            camera.Position = new Vector3(2, 0, 0);
            var second = new Matrix4(
                0, 0, 1, 0,
                0, 1, 0, 0,
                -1, 0, 0, 0,
                0, 0, -2, 1);

            Assert.Equal(second, camera.ViewMatrix);
        }

        [Fact]
        public void UpdatesViewMatrixOnTargetChanged()
        {
            var camera = new GLCamera();
            camera.Position = new Vector3(1, 0, 0);
            
            var first = new Matrix4(
                0, 0, 1, 0,
                0, 1, 0, 0,
                -1, 0, 0, 0,
                0, 0, -1, 1);

            Assert.Equal(first, camera.ViewMatrix);
            
            camera.TargetPosition = new Vector3(1, 0, 1);
            var second = new Matrix4(
                -1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, -1, 0,
                1, 0, 0, 1);

            Assert.Equal(second, camera.ViewMatrix);
        }

        [Fact]
        public void UpdatesViewMatrixOnUpChanged()
        {
            var camera = new GLCamera();
            camera.Position = new Vector3(1, 0, 0);
            
            var first = new Matrix4(
                0, 0, 1, 0,
                0, 1, 0, 0,
                -1, 0, 0, 0,
                0, 0, -1, 1);

            Assert.Equal(first, camera.ViewMatrix);

            camera.UpVector = new Vector3(0, -1, 0);
            var second = new Matrix4(
                0, 0, 1, 0,
                0, -1, 0, 0,
                1, 0, 0, 0,
                0, 0, -1, 1);

            Assert.Equal(second, camera.ViewMatrix);
        }
    }
}