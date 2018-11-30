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
            // TODO: This approach fails to provide wait-freedom and threads may starve due to contention
            while (true)
            {
                var oldFirst = First;
                node.Next = oldFirst;
                Interlocked.CompareExchange(ref First, node, oldFirst);
                if (ReferenceEquals(node, First))
                {
                    return node;
                }
            }
        }
    }
}
