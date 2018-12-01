namespace LeastRecentlyUsedCache
{
    internal class CacheValue<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public CacheValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
