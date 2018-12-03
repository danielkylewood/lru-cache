using System.Collections.Concurrent;

namespace ConcurrentLruCache.ConcurrentLruCache
{
    public class ConcurrentLruCache<TKey, TValue>
    {
        private readonly int _maxSize;
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Creates a concurrent least recently used cache with a specified number of entries.
        /// </summary>
        /// <param name="maxSize">The maximum number of entries in the least recently used cache</param>
        public ConcurrentLruCache(int maxSize)
        {
            _maxSize = maxSize;
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        public void AddOrUpdate()
        {
        }
    }
}
