using System;
using System.Collections.Generic;
using System.Linq;

namespace Moya.Runner.DataStructures
{
    public class MultiMap<TKey, TValue> : Dictionary<TKey, HashSet<TValue>>
    {
        public bool Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            HashSet<TValue> values = null;
            if (!base.TryGetValue(key, out values))
            {
                values = new HashSet<TValue>();
                base.Add(key, values);
            }
            return values.Add(value);
        }

        public bool ContainsValue(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            HashSet<TValue> values;
            if (base.TryGetValue(key, out values))
            {
                return values.Contains(value);
            }
            return false;
        }

        public bool Remove(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            HashSet<TValue> values;
            var removedItem = false;

            if (base.TryGetValue(key, out values))
            {
                removedItem = values.Remove(value);
                if (values.Count <= 0)
                {
                    base.Remove(key);
                }
            }
            return removedItem;
        }

        public HashSet<TValue> GetValues(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            HashSet<TValue> values;
            base.TryGetValue(key, out values);
            return values;
        }

        public TKey[] GetKeys()
        {
            return base.Keys.ToArray();
        } 
    }
}