using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentLruCache.ConcurrentLinkedList
{
    internal class ConcurrentLinkedList<T> : IConcurrentLinkedList<T>
    {
        private ConcurrentLinkedListNode<T> _first;
        private ConcurrentLinkedListNode<T> _last;

        public ConcurrentLinkedListNode<T> First => _first;

        public ConcurrentLinkedList()
        {
            var dummyNode = new ConcurrentLinkedListNode<T>();
            _first = dummyNode;
            _last = dummyNode;
        }

        /// <summary>
        /// Adds a node to the beginning of the <see cref="ConcurrentLinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The node to add</param>
        public void AddFirst(ConcurrentLinkedListNode<T> node)
        {
            node.Next = null;
            node.Previous = null;

            while (true)
            {
                var old = _first;
                node.Next = old;
                var original = Interlocked.CompareExchange(ref _first, node, old);
                if (ReferenceEquals(original, old))
                {
                    SetFirstValidPrevious(_first, _first);
                    return;
                }
            }
        }

        public void RemoveAndAddFirst(ConcurrentLinkedListNode<T> node)
        {
            if (node.Equals(_first))
            {
                return;
            }

            var originalState = node.CompareAndExchangeNodeState(NodeState.Invalid, NodeState.Valid);
            if (originalState == NodeState.Invalid)
            {
                return;
            }
            
            SetFirstValidNext(node, node.Next);
            SetFirstValidPrevious(node, node.Previous);
            
            
            AddFirst(node);
        }

        public void RemoveLast()
        {
            var originalState = _last.CompareAndExchangeNodeState(NodeState.Invalid, NodeState.Valid);
            if (originalState == NodeState.Invalid)
            {
                RemoveNextLast(_last.Previous);
                return;
            }

            var current = _last.Previous;
            while (current != _last)
            {
                lock (current.Lock)
                {
                    if (current.NodeState == NodeState.Invalid)
                    {
                        current = current.Previous;
                    }
                    else
                    {
                        _last = current;
                        current.Next = null;
                    }
                }
            }
        }

        private void RemoveNextLast(ConcurrentLinkedListNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            var originalState = node.CompareAndExchangeNodeState(NodeState.Invalid, NodeState.Valid);
            if (originalState == NodeState.Invalid)
            {
                RemoveNextLast(node.Previous);
                return;
            }
        
            lock (node.Lock)
            {
                SetFirstValidNext(node, null);
            }
        
        }

        private static void SetFirstValidNext(ConcurrentLinkedListNode<T> current, ConcurrentLinkedListNode<T> value)
        {
            current = current.Previous;
            while (current != null)
            {
                lock (current.Lock)
                {
                    if (current.NodeState == NodeState.Invalid)
                    {
                        current = current.Previous;
                    }
                    else
                    {
                        current.Next = value;
                        return;
                    }
                }
            }
        }

        private static void SetFirstValidPrevious(ConcurrentLinkedListNode<T> current, ConcurrentLinkedListNode<T> value)
        {
            current = current.Next;
            while (current != null)
            {
                lock (current.Lock)
                {
                    if (current.NodeState == NodeState.Invalid)
                    {
                        current = current.Next;
                    }
                    else
                    {
                        current.Previous = value;
                        break;
                    }
                }
            }
        }
    }
}
