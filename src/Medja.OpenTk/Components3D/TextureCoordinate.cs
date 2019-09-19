namespace Medja.OpenTk.Components3D
{
    /// <summary>
    /// Texture coordinates for a character.
    /// </summary>
    public struct TextureCoordinate
    {
        public float U { get; set; }
        public float V { get; set; }

        public TextureCoordinate(float u, float v)
        {
            U = u;
            V = v;
        }
    }
}