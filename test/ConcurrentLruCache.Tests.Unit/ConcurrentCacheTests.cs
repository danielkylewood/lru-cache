using ConcurrentLruCache.LruCache;
using NUnit.Framework;

namespace ConcurrentLruCache.Tests.Unit
{
    public class ConcurrentCacheTests
    {
        private const int _maxSize = 20;
        private LruCache<dynamic, dynamic> _lruCache;

        [SetUp]
        public void Setup()
        {
            _lruCache = new LruCache<dynamic, dynamic>(_maxSize);
        }

        [Test]
        public void When_Adding_To_The_Cache_Then_Entry_Is_Added_Correctly()
        {
        }
    }
}
