namespace Moya.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Extends the <see cref="IDictionary{TKey,TValue}"/> class.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds multiple entries from one <see cref="IDictionary{TKey,TValue}"/>
        /// to another <see cref="IDictionary{TKey,TValue}"/>. If the original
        /// <see cref="IDictionary{TKey,TValue}"/> already contains the specified
        /// <see cref="TKey"/>, it will be ignored, not overwritten.
        /// </summary>
        /// <typeparam name="TKey">The type of the key used in the <see cref="IDictionary{TKey,TValue}"/>.</typeparam>
        /// <typeparam name="TValue">The type of the value used in the <see cref="IDictionary{TKey,TValue}"/>.</typeparam>
        /// <param name="target">The target <see cref="IDictionary{TKey,TValue}"/> which all 
        ///         <see cref="TKey"/>-<see cref="TValue"/> pairs will be copied to.</param>
        /// <param name="collection">The <see cref="IDictionary{TKey,TValue}"/> to copy from.</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (var item in collection.Where(item => !target.ContainsKey(item.Key)))
            {
                target.Add(item.Key, item.Value);
            }
        }
    }
}