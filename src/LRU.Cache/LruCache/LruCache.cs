using System.Collections.Generic;
using LeastRecentlyUsedCache.Models;

namespace LeastRecentlyUsedCache.LruCache
{
    public class LruCache<TKey, TValue>
    {
        private readonly int _maxSize;
        private readonly LinkedList<LruCacheValue<TKey, TValue>> _linkedList;
        private readonly Dictionary<TKey, LinkedListNode<LruCacheValue<TKey, TValue>>> _dictionary;

        public int Count => _dictionary.Count;

        /// <summary>
        /// Creates a least recently used cache with a specified number of entries.
        /// </summary>
        /// <param name="maxSize">The maximum number of entries in the least recently used cache</param>
        public LruCache(int maxSize)
        {
            _maxSize = maxSize;
            _linkedList = new LinkedList<LruCacheValue<TKey, TValue>>();
            _dictionary = new Dictionary<TKey, LinkedListNode<LruCacheValue<TKey, TValue>>>(_maxSize);
        }

        /// <summary>
        /// Add a specified key/value pair to the least recently used cache.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns>The value that was added to the least recently used cache</returns>
        public TValue Add(TKey key, TValue value)
        {
            if (Count == _maxSize)
            {
                RemoveOldest();
            }

            var linkedListValue = new LruCacheValue<TKey, TValue>(key, value);
            var linkedListNode = new LinkedListNode<LruCacheValue<TKey, TValue>>(linkedListValue);
            _dictionary.Add(key, linkedListNode);
            _linkedList.AddFirst(linkedListNode);

            return value;
        }

        /// <summary>
        /// Retrieves a value by a specified key from the least recently used cache.
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <param name="value">The retrieved value</param>
        /// <returns>True if the value was successfully retrieved, else false</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_dictionary.ContainsKey(key))
            {
                value = default(TValue);
                return false;
            }

            value = Remove(key);
            Add(key, value);

            return true;
        }

        /// <summary>
        /// Removes a specified key/value pair from the least recently used cache.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value removed from the least recently used cache</returns>
        public TValue Remove(TKey key)
        {
            _dictionary.Remove(key, out var node);
            _linkedList.Remove(node);
            return node.Value.Value;
        }

        /// <summary>
        /// Determines whether the least recently used cache contains the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>True if the key exists, else false</returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out var value))
                {
                    throw new KeyNotFoundException();
                }

                return value;
            }
            set => UpdateValue(key, value);
        }

        private void RemoveOldest()
        {
            _dictionary.Remove(_linkedList.Last.Value.Key);
            _linkedList.RemoveLast();
        }

        private void UpdateValue(TKey key, TValue value)
        {
            if (!_dictionary.ContainsKey(key))
            {
                throw new KeyNotFoundException("The key was not found in the cache.");
            }

            Remove(key);
            Add(key, value);
        }
    }
}
