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
            // the modelmatrix applies first the scaling, than the movement and rotation (can be in any order)
            // for some reason we need to divide the components of the translation vector to get the correct
            // position
            var unscaledVector = new Vector4(
                vector.X / _modelMatrix.Scaling.X,
                vector.Y / _modelMatrix.Scaling.Y,
                vector.Z / _modelMatrix.Scaling.Z,
                vector.W);
            
            // https://learnopengl.com/Getting-started/Coordinate-Systems
            // note OpenTK matrix multiplication order: left to right
            var clipCoordinates = unscaledVector * _modelMatrix.Matrix * viewProjection;

            // so called perspective divide 
            // http://www.songho.ca/opengl/gl_transform.html
            var normalizedDeviceCoordinates = new Vector3(
                clipCoordinates.X / clipCoordinates.W, 
                clipCoordinates.Y / clipCoordinates.W,
                clipCoordinates.Z / clipCoordinates.W);
            
            var width = _mainWindowPos.Width;
            var height = _mainWindowPos.Height;

            // http://www.songho.ca/opengl/gl_transform.html -> window coordinates
            // for an unknown reason we need to use 1/4th of the width instead of half for
            // the first factor; -height because OpenGL <=> SkiaSharps Y axis are inverted 
            // (OpenGL Y = 0 => bottom, Skia Y = 0 => top)
            // the (0 + ...) is a placeholder for x and y, if you would specify a viewport not starting at 0
            // currently all controls get a viewport starting at (0, 0)
            var xWindow = width * 0.25f * normalizedDeviceCoordinates.X + (0 + width * 0.5f);
            var yWindow = -height * 0.25f * normalizedDeviceCoordinates.Y + (0 + height * 0.5f);
           
            var result = new Point(xWindow - _containerPos.X, yWindow - _containerPos.Y);

            // Console.WriteLine($"clip: {clipCoordinates}, ndc: {normalizedDeviceCoordinates}, x: {xWindow:F02}, y: {yWindow:F02}");
            // Console.WriteLine($"point: {result}");

            return result;
        }
    }
}