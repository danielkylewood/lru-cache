namespace LruCache
{
    internal class LruCacheValue<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public LruCacheValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
