namespace ConcurrentLruCache.ConcurrentLruCache
{
    internal class ConcurrentLruCacheLinkedListNode<T>
    {
        public T Value;
        public ConcurrentLruCacheLinkedListNode<T> Next;
        public ConcurrentLruCacheLinkedListNode<T> Previous;

        public ConcurrentLruCacheLinkedListNode(T value)
        {
            Value = value;
        }
    }
}
