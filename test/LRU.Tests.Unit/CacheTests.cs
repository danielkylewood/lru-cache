using System;
using System.Collections.Generic;
using LRU.Cache;
using NUnit.Framework;

namespace LRU.Tests.Unit
{
    public class CacheTests
    {
        private const int _maxSize = 20;
        private Cache<dynamic, dynamic> _cache;
        [SetUp]
        public void Setup()
        {
            _cache = new Cache<dynamic, dynamic>(_maxSize);
        }

        public void AddCacheEntries(Cache<dynamic, dynamic> cache, int numAdditions)
        {
            for (var i = 0; i < numAdditions; i++)
            {
                var ticks = DateTime.Now.Ticks;
                var key = $"key{ticks}";
                var value = $"value{ticks}";
                cache.Add(key, value);
            }
        }

        [Test]
        [TestCaseSource(nameof(AddCases))]
        public void When_Adding_To_The_Cache_Then_Entry_Is_Added_Correctly(dynamic objectToAdd)
        {
            _cache.Add(objectToAdd, objectToAdd);

            Assert.That(_cache.ContainsKey(objectToAdd), Is.True);
            Assert.That(_cache.TryGetValue(objectToAdd, out dynamic result), Is.True);
            Assert.That(result, Is.EqualTo(objectToAdd));
        }

       [Test]
        public void When_Adding_To_The_Cache_Over_Capacity_Then_Oldest_Is_Removed()
        {
            const string keyValue = "oldest";

            _cache.Add(keyValue, keyValue);
            AddCacheEntries(_cache, _maxSize);

            Assert.That(_cache.Count, Is.LessThanOrEqualTo(_maxSize));
            Assert.That(_cache.ContainsKey(keyValue), Is.False);
        }

        [Test]
        public void When_Adding_To_The_Cache_Over_Capacity_And_Fetching_Oldest_Entry_Then_Second_Oldest_Is_Removed()
        {
            const string updateValue = "updateValue";
            const string secondOldestValue = "secondOldestValue";

            _cache.Add(updateValue, updateValue);
            _cache.Add(secondOldestValue, secondOldestValue);
            AddCacheEntries(_cache, _maxSize - 2);

            _cache.TryGetValue(updateValue, out var result);
            AddCacheEntries(_cache, _maxSize - 2);

            Assert.That(_cache.Count, Is.EqualTo(_maxSize));
            Assert.That(_cache.ContainsKey(updateValue), Is.True);
            Assert.That(_cache.ContainsKey(secondOldestValue), Is.False);
        }

        [Test]
        public void When_Setting_Value_Of_Existing_Key_Then_Value_Updated_And_Renewed()
        {
            const string updateValue = "updateValue";
            const string updatedValue = "updatedValue";

            _cache.Add(updateValue, updateValue);
            AddCacheEntries(_cache, _maxSize - 1);

            _cache[updateValue] = updatedValue;
            AddCacheEntries(_cache, _maxSize - 1);

            Assert.That(_cache.Count, Is.EqualTo(_maxSize));
            Assert.That(_cache.ContainsKey(updateValue), Is.True);
            Assert.That(_cache[updateValue], Is.EqualTo(updatedValue));
        }

        [Test]
        [TestCaseSource(nameof(GetCases))]
        public void When_Getting_Value_Then_Value_Is_Returned_If_Exists(dynamic key, dynamic value, bool shouldAdd)
        {
            AddCacheEntries(_cache, _maxSize);
            if (shouldAdd) _cache.Add(key, value);
            Assert.That(_cache.TryGetValue(key, out dynamic result), Is.EqualTo(shouldAdd));
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        [TestCaseSource(nameof(AddCases))]
        public void When_Value_Removed_Then_Value_No_Longer_In_Cache(dynamic objectToAdd)
        {
            _cache.Add(objectToAdd, objectToAdd);
            Assert.That(_cache.ContainsKey(objectToAdd), Is.True);

            _cache.Remove(objectToAdd);
            Assert.That(_cache.ContainsKey(objectToAdd), Is.False);
        }

        [Test]
        public void When_Retrieving_Directly_With_Non_Existing_Key_Then_Key_Not_Found_Exception_Thrown()
        {
            Assert.Throws<KeyNotFoundException>(() =>
            {
                var doesNotExistValue = _cache["doesNotExist"];
            });
        }

        private static readonly object[] AddCases =
        {
            new object[] { new List<string>() },
            new object[] { "key" },
            new object[] { 12 }
        };

        public static readonly object[] GetCases =
        {
            new object[] {new List<string>(), "value", true},
            new object[] {"key", new List<string>(), true},
            new object[] {12, null, false}
        };
    }
}
