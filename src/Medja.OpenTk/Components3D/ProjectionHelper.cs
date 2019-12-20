using Medja.Primitives;
using OpenTK;

namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Calculates the projection of 3d vector to 2d coordinates (relative to it's container)
    /// </summary>
    public class ProjectionHelper
    {
        private readonly MRect _mainWindowPos;
        private readonly MRect _containerPos;
        private readonly ModelMatrix _modelMatrix;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="mainWindowPos">The position of the main window.</param>
        /// <param name="containerPos">The position of the control containing the element to render.</param>
        /// <param name="modelMatrix">The model matrix (translation, rotation, ...) is applied to the vector you wanna project.</param>
        public ProjectionHelper(MRect mainWindowPos, MRect containerPos, ModelMatrix modelMatrix)
        {
            _mainWindowPos = mainWindowPos;
            _containerPos = containerPos;
            _modelMatrix = modelMatrix;
        }

        /// <summary>
        /// Calculates the 2D point for a given vector.
        /// </summary>
        /// <param name="viewProjection">The view-projection matrix (camera).</param>
        /// <param name="vector">The 3d vector to calculate the 2d point for.</param>
        /// <returns>The 2D position relative to it's container.</returns>
        public Point ProjectTo2D(Matrix4 viewProjection, Vector4 vector)
        {
            // https://learnopengl.com/Getting-started/Coordinate-Systems
            // note OpenTK matrix multiplication order: left to right
            
            var clipCoordinates = vector * _modelMatrix.Matrix * viewProjection;

            // so called perspective divide 
            // http://www.songho.ca/opengl/gl_transform.html
            var normalizedDeviceCoordinates = new Vector3(
                clipCoordinates.X / clipCoordinates.W, 
                clipCoordinates.Y / clipCoordinates.W,
                clipCoordinates.Z / clipCoordinates.W);
            
            // OpenGL glViewport: xw = x in window coordinates, xnd = x in normalized device coordinates,
            // width = device coordinates, x of glViewport
            // xw = (xnd + 1) ⁢ width/2 + x
            // yw = (ynd + 1) ⁢ height/2 + y

            var width = _mainWindowPos.Width;
            var height = _mainWindowPos.Height;

            // x = 0 and y = 0, so left out
            // for whatever reason this is the correct formula, not the one mentioned on the opengl page...
            var xWindow = width / 2 + normalizedDeviceCoordinates.X * width / 4;
            var yWindow = height / 2 + normalizedDeviceCoordinates.Y * -height / 4;
            // var xWindow = (normalizedDeviceCoordinates.X + 1) * width / 2;
            // var yWindow = (normalizedDeviceCoordinates.Y + 1) * height / 2;

            // Console.WriteLine($"clip: {clipCoordinates}, ndc: {normalizedDeviceCoordinates}, x: {xWindow:F02}, y: {yWindow:F02}");

            var result = new Point(xWindow - _containerPos.X, yWindow - _containerPos.Y);

            // Console.WriteLine($"point: {result}");

            return result;
        }
    }
}