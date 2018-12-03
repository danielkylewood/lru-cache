namespace ConcurrentLruCache.ConcurrentLinkedList
{
    interface IConcurrentLinkedList<T>
    {
        ConcurrentLinkedListNode<T> First { get; }

        void RemoveLast();
        void RemoveAndAddFirst(ConcurrentLinkedListNode<T> node);
        void AddFirst(ConcurrentLinkedListNode<T> node);
    }
}
