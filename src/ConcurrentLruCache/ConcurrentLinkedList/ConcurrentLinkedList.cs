using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentLruCache.ConcurrentLinkedList
{
    internal class ConcurrentLinkedList<T>
    {
        public ConcurrentLinkedListNode<T> First;
        public ConcurrentLinkedListNode<T> Last;

        public ConcurrentLinkedList()
        {
            var dummyNode = new ConcurrentLinkedListNode<T>();
            First = dummyNode;
            Last = dummyNode;
        }

        /// <summary>
        /// Adds a node to the beginning of the <see cref="ConcurrentLinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The node to add</param>
        public void AddFirst(ConcurrentLinkedListNode<T> node)
        {
            
            while (true)
            {
                var old = First;
                node.Next = old;
                var original = Interlocked.CompareExchange(ref First, node, old);
                if (ReferenceEquals(original, old))
                {
                    return;
                }
            }
        }

        public bool TryRemove(ConcurrentLinkedListNode<T> node, out ConcurrentLinkedListNode<T> outNode)
        {
            var originalState = node.CompareAndExchangeNodeState(NodeState.Invalid, NodeState.Valid);
            if (originalState == NodeState.Invalid)
            {
                outNode = node;
                return false;
            }

            outNode = null;
            return true;
        }

        public bool RemoveLast()
        {
            return true;
        }
    }
}
