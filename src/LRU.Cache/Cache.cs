using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LRU.Cache
{
    public class Cache<TKey, TValue>
    {
        private readonly int _maxSize;
        private readonly LinkedList<LinkedListValue<TKey, TValue>> _linkedList;
        private readonly Dictionary<TKey, LinkedListNode<LinkedListValue<TKey, TValue>>> _dictionary;

        public int Count => _dictionary.Count;

        /// <summary>
        /// Creates a least recently used cache
        /// </summary>
        /// <param name="maxSize">The maximum number of entries in the cache</param>
        public Cache(int maxSize)
        {
            _maxSize = maxSize;
            _linkedList = new LinkedList<LinkedListValue<TKey, TValue>>();
            _dictionary = new Dictionary<TKey, LinkedListNode<LinkedListValue<TKey, TValue>>>();
        }


        /// <summary>
        /// Add a key/value pair to the cache
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <param name="value">The value</param>
        /// <returns>The value added to the cache</returns>
        public TValue Add(TKey key, TValue value)
        {
            if (Count == _maxSize)
            {
                RemoveOldest();
            }

            var linkedListValue = new LinkedListValue<TKey, TValue>(key, value);
            var linkedListNode = new LinkedListNode<LinkedListValue<TKey, TValue>>(linkedListValue);
            _dictionary.Add(key, linkedListNode);
            _linkedList.AddFirst(linkedListNode);

            return value;
        }

        /// <summary>
        /// Retrieves a value from a key in the cache
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <param name="value">The retrieved value</param>
        /// <returns>The result of retrieving the value</returns>
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
        /// Removes a key/value pair from the cache
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <returns>The removed value from the cache</returns>
        public TValue Remove(TKey key)
        {
            _dictionary.Remove(key, out var node);
            _linkedList.Remove(node);
            return node.Value.Value;
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

        /// <summary>
        /// Updates a value in the cache
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <param name="value">The new value</param>
        private void UpdateValue(TKey key, TValue value)
        {
            if (!_dictionary.ContainsKey(key))
            {
                throw new KeyNotFoundException("The key was not found in the cache.");
            }
            
            Remove(key);
            Add(key, value);
        }

        /// <summary>
        /// Determines whether the cache contains the specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>Whether the cache contains the specified key</returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        private void RemoveOldest()
        {
            _dictionary.Remove(_linkedList.Last.Value.Key);
            _linkedList.RemoveLast();
        }
    }
}
