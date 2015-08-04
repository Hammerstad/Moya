namespace Moya.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (var item in collection.Where(item => !source.ContainsKey(item.Key)))
            {
                source.Add(item.Key, item.Value);
            }
        }
    }
}