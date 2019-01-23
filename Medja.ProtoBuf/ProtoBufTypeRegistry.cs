using System;
using System.Collections.Generic;

namespace Medja.ProtoBuf
{
    /// <summary>
    /// A simple registry used by <see cref="ProtoBufReader"/> and <see cref="ProtoBufWriter"/> to keep track of the
    /// use message types.
    /// </summary>
    public class ProtoBufTypeRegistry
    {
        private readonly Dictionary<Type, int> _typeToId;
        private readonly List<Type> _types;

        public ProtoBufTypeRegistry()
        {
            _typeToId = new Dictionary<Type, int>();
            _types = new List<Type>();
        }

        /// <summary>
        /// Gets if the given type is present in the registry.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if the type is present, else false.</returns>
        public bool HasType(Type type)
        {
            return _typeToId.ContainsKey(type); // TODO test if faster as _types.Contains
        }

        /// <summary>
        /// Adds a type to the registry.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The new type id.</returns>
        public int Add(Type type)
        {
            var typeId = _typeToId.Count;
            
            _typeToId.Add(type, typeId);
            _types.Add(type);

            return typeId;
        }

        /// <summary>
        /// Gets the id for a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type id.</returns>
        public int GetTypeId(Type type)
        {
            return _typeToId[type];
        }

        /// <summary>
        /// Gets a type based on the type id.
        /// </summary>
        /// <param name="id">The type id.</param>
        /// <returns>The type.</returns>
        public Type GetTypeById(int id)
        {
            return _types[id];
        }
    }
}