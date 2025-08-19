using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Systems
{
    public sealed class ArenaGridRepository
    {
        readonly Dictionary<Vector3, ArenaParcel.FlooringType> map = new Dictionary<Vector3, ArenaParcel.FlooringType>();
        public void Clear() => map.Clear();
        public void Set(Vector3 pos, ArenaParcel.FlooringType type) { map[pos] = type; }
        public bool TryGet(Vector3 pos, out ArenaParcel.FlooringType type) => map.TryGetValue(pos, out type);
        public IEnumerable<KeyValuePair<Vector3, ArenaParcel.FlooringType>> Enumerate() => map;
        public int Count => map.Count;
    }
}