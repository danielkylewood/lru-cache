namespace LRU.Cache.Models
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
