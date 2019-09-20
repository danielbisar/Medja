namespace Medja.OpenTk.Components3D
{
    public class CharTextureCoordinates
    {
        private readonly CharTextureCoordinate[] _coordinates;
        
        public CharTextureCoordinate this[char index]
        {
            get { return _coordinates[index]; }
            set { _coordinates[index] = value; }
        }
        
        /// <summary>
        /// Letter height in percentage (relative to width)
        /// </summary>
        public float LetterHeight {get;set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">Count of coordinates</param>
        public CharTextureCoordinates(int count)
        {
            _coordinates = new CharTextureCoordinate[count];
        }
    }
}