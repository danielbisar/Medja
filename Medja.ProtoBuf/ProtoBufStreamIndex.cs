using System.Collections.Generic;

namespace Medja.ProtoBuf
{
    /// <summary>
    /// An index on the messages in a <see cref="ProtoBufReader"/>.
    /// </summary>
    public class ProtoBufStreamIndex
    {
        private readonly HashSet<long> _hashSet;
        private readonly List<long> _indices;
        
        public int Count
        {
            get { return _indices.Count; }
        }

        public long this[int index]
        {
            get { return _indices[index]; }
        }
        
        public ProtoBufStreamIndex()
        {
            _hashSet = new HashSet<long>();
            _indices = new List<long>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>True if the index was added.</returns>
        public bool Add(long index)
        {
            if (_hashSet.Add(index))
            {
                _indices.Add(index);
                return true;
            }

            return false;
        }
        
        
    }
}