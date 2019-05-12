namespace LruCache
{
    public interface ILruCache<in TKey, TValue>
    {
        /// <summary>
        /// Determines the number of key/value pairs that are currently in the <see cref="LruCache{TKey, TValue}"/>.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Determines whether the <see cref="LruCache{TKey, TValue}"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>True if the key exists, else false</returns>
        /// <exception cref="System.ArgumentException">Thrown when adding a duplicate key to the cache.</exception>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Add a specified key/value pair to the <see cref="LruCache{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns>The value that was added to the least recently used cache</returns>
        TValue Add(TKey key, TValue value);

        /// <summary>
        /// Retrieves a value by a specified key from the <see cref="LruCache{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <param name="value">The retrieved value</param>
        /// <returns>True if the value was successfully retrieved, else false</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Removes a specified key/value pair from the <see cref="LruCache{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value removed from the least recently used cache</returns>
        TValue Remove(TKey key);

        TValue this[TKey key] { get; set; }
    }
}
