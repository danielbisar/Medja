namespace Medja.Controls.Graph2D
{
//    /// <summary>
//    /// Sames as <see cref="DataPoints"/> but adds a cache for the downsampled curve.
//    /// </summary>
//    public class CachedDataPoints : DataPoints
//    {
//        private List<Point> _downsampledPoints;
//        private DataPointsRenderingSettings _previousSettings;
//
//        public void InvalidateCache()
//        {
//            _downsampledPoints = null;
//        }
//
//        public override List<Point> GetForRendering(DataPointsRenderingSettings settings)
//        {
//            if (_previousSettings != settings || _downsampledPoints == null)
//            {
//                _downsampledPoints = base.GetForRendering(settings);
//                _previousSettings = settings;
//            }
//
//            return _downsampledPoints;
//        }
//    }
}
