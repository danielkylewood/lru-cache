using System;
using System.Collections.Generic;
using LRU.Cache;
using LRU.Cache.Cache;
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

        [Test]
        [TestCaseSource(nameof(AddCases))]
        public void When_Adding_To_The_Cache_Then_Entry_Is_Added_Correctly(dynamic objectToAdd)
        {
            // When value added to the cache
            _cache.Add(objectToAdd, objectToAdd);

            // Value exists in cache and added correctly
            Assert.That(_cache.ContainsKey(objectToAdd), Is.True);
            Assert.That(_cache.TryGetValue(objectToAdd, out dynamic result), Is.True);
            Assert.That(result, Is.EqualTo(objectToAdd));
        }

        [Test]
        public void When_Adding_To_The_Cache_Over_Capacity_Then_Oldest_Is_Removed()
        {
            // Given a value to add to cache
            const string keyValue = "oldest";

            // When key is added to cache and an additional max number of entries are added
            _cache.Add(keyValue, keyValue);
            AddCacheEntries(_cache, _maxSize);

            // Then cache contains only max items and first item added no longer exists
            Assert.That(_cache.Count, Is.LessThanOrEqualTo(_maxSize));
            Assert.That(_cache.ContainsKey(keyValue), Is.False);
        }

        [Test]
        public void When_Adding_To_The_Cache_Over_Capacity_And_Fetching_Oldest_Entry_Then_Second_Oldest_Is_Removed()
        {
            // Given two values to add to the cache
            const string valueToUpdate = "updateValue";
            const string secondOldestValue = "secondOldestValue";

            // When the value that will be updated is added and another added afterwards
            _cache.Add(valueToUpdate, valueToUpdate);
            _cache.Add(secondOldestValue, secondOldestValue);

            // And the cache is fully populated
            AddCacheEntries(_cache, _maxSize - 2);

            // And the first value added is refreshed by getting it 
            _cache.TryGetValue(valueToUpdate, out _);

            // And new entries are added to the cache
            AddCacheEntries(_cache, _maxSize - 2);

            // Then the updated value still exists in the cache and the second value does not
            Assert.That(_cache.Count, Is.EqualTo(_maxSize));
            Assert.That(_cache.ContainsKey(valueToUpdate), Is.True);
            Assert.That(_cache.ContainsKey(secondOldestValue), Is.False);
        }

        [Test]
        public void When_Setting_Value_Of_Existing_Key_Then_Value_Updated_And_Renewed()
        {
            // Given one value to add to the cache and another to later update that value to 
            const string updateValue = "updateValue";
            const string updatedValue = "updatedValue";

            // When the value is added and the cache fully populated
            _cache.Add(updateValue, updateValue);
            AddCacheEntries(_cache, _maxSize - 1);
            
            // And the original value is updated and the cache repopulated
            _cache[updateValue] = updatedValue;
            AddCacheEntries(_cache, _maxSize - 1);

            // The value still exists in the cache as it was updated
            Assert.That(_cache.Count, Is.EqualTo(_maxSize));
            Assert.That(_cache.ContainsKey(updateValue), Is.True);
            Assert.That(_cache[updateValue], Is.EqualTo(updatedValue));
        }

        [Test]
        [TestCaseSource(nameof(GetCases))]
        public void When_Getting_Value_Then_Value_Is_Returned_If_Exists(dynamic key, dynamic value, bool shouldAdd)
        {
            // When a cache is fully populated
            AddCacheEntries(_cache, _maxSize);

            // If a new value should be added to the cache and is added
            if (shouldAdd) _cache.Add(key, value);

            // Then it should be able to be fetched from the cache - otherwise it should not
            Assert.That(_cache.TryGetValue(key, out dynamic result), Is.EqualTo(shouldAdd));
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        [TestCaseSource(nameof(AddCases))]
        public void When_Value_Removed_Then_Value_No_Longer_In_Cache(dynamic objectToAdd)
        {
            // When a value is added to the cache
            _cache.Add(objectToAdd, objectToAdd);

            // Then it exists in the cache
            Assert.That(_cache.ContainsKey(objectToAdd), Is.True);

            // When a value is removed from the cache
            _cache.Remove(objectToAdd);

            // Then the value should not exist in the cache
            Assert.That(_cache.ContainsKey(objectToAdd), Is.False);
        }

        [Test]
        public void When_Retrieving_Directly_With_Non_Existing_Key_Then_Key_Not_Found_Exception_Thrown()
        {
            Assert.Throws<KeyNotFoundException>(() =>
            {
                // When a value that does not exist in the cache is referenced directly exception is thrown
                var unused = _cache["doesNotExist"];
            });
        }

        private static void AddCacheEntries(Cache<dynamic, dynamic> cache, int numAdditions)
        {
            for (var i = 0; i < numAdditions; i++)
            {
                var ticks = DateTime.Now.Ticks;
                var key = $"key{ticks}";
                var value = $"value{ticks}";
                cache.Add(key, value);
            }
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
