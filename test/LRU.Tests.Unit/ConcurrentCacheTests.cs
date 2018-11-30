using System;
using System.Collections.Generic;
using LRU.Cache;
using LRU.Cache.Cache;
using NUnit.Framework;

namespace LRU.Tests.Unit
{
    public class ConcurrentCacheTests
    {
        private const int _maxSize = 20;
        private Cache<dynamic, dynamic> _cache;

        [SetUp]
        public void Setup()
        {
            _cache = new Cache<dynamic, dynamic>(_maxSize);
        }

        [Test]
        public void When_Adding_To_The_Cache_Then_Entry_Is_Added_Correctly()
        {
        }
    }
}
