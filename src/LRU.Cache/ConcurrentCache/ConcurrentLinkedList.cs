using System;
using System.Collections.Generic;
using System.Text;
using LRU.Cache.Models;

namespace LRU.Cache.ConcurrentCache
{
    internal class ConcurrentLinkedList<TKey, TValue>
    {
        public ConcurrentLinkedList()
        {
            var meh = new LinkedList<TKey>();
            
        }

        public void AddFirst(Node<TKey, TValue> node)
        {
            
        }

        
    }
}
