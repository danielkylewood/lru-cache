namespace LRU.Cache
{
    public class LinkedListValue<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public LinkedListValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
