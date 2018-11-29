using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LRU.Cache
{
    public class ConcurrentCache<TKey, TValue>
    {
        private readonly int _maxSize;
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Creates a concurrent least recently used cache with a specified number of entries.
        /// </summary>
        /// <param name="maxSize">The maximum number of entries in the least recently used cache</param>
        public ConcurrentCache(int maxSize)
        {
            _maxSize = maxSize;
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
        }
    }
}
