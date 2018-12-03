using System.Threading;

namespace ConcurrentLruCache.ConcurrentLinkedList
{
    internal class ConcurrentLinkedListNode<T>
    {
        public T Value;
        public ConcurrentLinkedListNode<T> Next;
        public ConcurrentLinkedListNode<T> Previous;
        public object Lock;

        private int _state;
        private readonly bool _isDummy;
        internal NodeState NodeState
        {
            get => (NodeState)_state;
            set => _state = (int)value;
        }

        public ConcurrentLinkedListNode(T value)
        {
            Value = value;
            _isDummy = false;
        }

        public ConcurrentLinkedListNode()
        {
            _isDummy = true;
        }

        internal NodeState CompareAndExchangeNodeState(NodeState value, NodeState compare)
        {
            return (NodeState)Interlocked.CompareExchange(ref _state, (int)value, (int)compare);
        }

        internal bool IsDummy()
        {
            return _isDummy;
        }
    }
}
