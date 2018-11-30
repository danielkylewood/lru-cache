using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LRU.Tests.Unit")]
namespace LRU.Cache.Models
{
    internal class ConcurrentLinkedListNode<T>
    {
        public T Value { get; }

        public ConcurrentLinkedListNode<T> Next;
        public ConcurrentLinkedListNode<T> Previous;

        public ConcurrentLinkedListNode(T value)
        {
            Value = value;
        }
    }
}
