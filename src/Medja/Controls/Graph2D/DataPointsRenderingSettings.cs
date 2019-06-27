namespace Medja.Controls
{
    public class DataPointsRenderingSettings
    {
        /// <summary>
        /// The minimal X value to render.
        /// </summary>
        public float MinX { get; set; }
        
        /// <summary>
        /// The maximal X value to render.
        /// </summary>
        public float MaxX { get; set; }

        /// <summary>
        /// The minimal Y value to render.
        /// </summary>
        public float MinY { get; set; }
        
        /// <summary>
        /// The maximal Y value to render.
        /// </summary>
        public float MaxY { get; set; }
        
        /// <summary>
        /// The pixel count available for rendering on the X axis.
        /// </summary>
        public int WidthPixels { get; set; }
        
        /// <summary>
        /// The pixel count available for rendering on the Y axis.
        /// </summary>
        public int HeightPixels { get; set; }
        
        
    }
}