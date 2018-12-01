using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using LRU.Cache.Models;

[assembly: InternalsVisibleTo("LRU.Tests.Unit")]
namespace LRU.Cache.ConcurrentCache
{
    /// <summary>
    /// Concurrent linked list that is threadsafe only in conjuction with <see cref="ConcurrentCache{TKey,TValue}"/>.
    /// </summary>
    internal class ConcurrentLinkedList<T>
    {
        public ConcurrentLinkedListNode<T> Last;
        public ConcurrentLinkedListNode<T> First;
        
        public ConcurrentLinkedList(ConcurrentLinkedListNode<T> initial)
        {
            First = initial;
            Last = initial;
        }

        public ConcurrentLinkedListNode<T> AddFirst(ConcurrentLinkedListNode<T> node)
        {
            // TODO: Approach fails to provide wait-freedom and threads may starve due to contention
            while (true)
            {
                var tempFirst = First;
                node.Next = tempFirst;
                var original = Interlocked.CompareExchange(ref First, node, tempFirst);
                if (ReferenceEquals(original, tempFirst))
                {
                    return node;
                }
            }
        }
    }
}
