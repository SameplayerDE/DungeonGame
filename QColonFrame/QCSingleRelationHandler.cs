using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonFrame
{
    public class QCSingleRelationHandler<K, V>
    {
        private Dictionary<K, V> _relations = new();

        public void AddRelation(K key)
        {
            _relations.Add(key, _relations[key]);
        }

        public void RemoveRelation(K key)
        {
            _relations.Remove(key);
        }

        public void SetRelation(K key, V value)
        {
            _relations[key] = value;
        }

        public V GetRelation(K key)
        {
            return _relations[key];
        }


    }
}
