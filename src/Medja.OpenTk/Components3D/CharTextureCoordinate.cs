namespace Medja.OpenTk.Components3D
{
    public struct CharTextureCoordinate
    {
        public TextureCoordinate TopLeft { get; set; }
        public TextureCoordinate BottomLeft { get; set; }
        public TextureCoordinate BottomRight { get; set; }
        public TextureCoordinate TopRight { get; set; }
        
        /// <summary>
        /// The width of the letter in percentage. 
        /// </summary>
        public float WidthPercentage { get; set; }
    }
}