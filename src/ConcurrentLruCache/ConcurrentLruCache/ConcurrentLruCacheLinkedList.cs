namespace ConcurrentLruCache.ConcurrentLruCache
{
    internal class ConcurrentLruCacheLinkedList<T>
    {
        public ConcurrentLruCacheLinkedListNode<T> First;
        public ConcurrentLruCacheLinkedListNode<T> Last;

        public bool AddFirst(T Value)
        {

            return true;
        }

        public bool Remove(T Value)
        {
            return true;
        }

        public bool RemoveLast()
        {
            return true;
        }
    }
}
