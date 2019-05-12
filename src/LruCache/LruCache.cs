using System.Collections.Generic;

namespace LruCache
{
    public class LruCache<TKey, TValue> : ILruCache<TKey, TValue>
    {
        private readonly int _maxSize;
        private readonly LinkedList<LruCacheValue<TKey, TValue>> _linkedList;
        private readonly Dictionary<TKey, LinkedListNode<LruCacheValue<TKey, TValue>>> _dictionary;

        public int Count => _dictionary.Count;

        /// <summary>
        /// Creates a <see cref="LruCache{TKey, TValue}"/> with a specified number of entries.
        /// </summary>
        /// <param name="maxSize">The maximum number of entries in the least recently used cache</param>
        public LruCache(int maxSize)
        {
            _maxSize = maxSize;
            _linkedList = new LinkedList<LruCacheValue<TKey, TValue>>();
            _dictionary = new Dictionary<TKey, LinkedListNode<LruCacheValue<TKey, TValue>>>(_maxSize);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public TValue Remove(TKey key)
        {
            _dictionary.Remove(key, out var node);
            _linkedList.Remove(node);
            return node.Value.Value;
        }

        /// <inheritdoc />
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
