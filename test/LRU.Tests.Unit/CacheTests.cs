using System;
using System.Collections.Generic;
using LeastRecentlyUsedCache.LruCache;
using NUnit.Framework;

namespace LeastRecentlyUsedCache.Tests.Unit
{
    public class CacheTests
    {
        private const int _maxSize = 20;
        private LruCache<dynamic, dynamic> _lruCache;

        [SetUp]
        public void Setup()
        {
            _lruCache = new LruCache<dynamic, dynamic>(_maxSize);
        }

        [Test]
        [TestCaseSource(nameof(AddCases))]
        public void When_Adding_To_The_Cache_Then_Entry_Is_Added_Correctly(dynamic objectToAdd)
        {
            // When value added to the lruCache
            _lruCache.Add(objectToAdd, objectToAdd);

            // Value exists in lruCache and added correctly
            Assert.That(_lruCache.ContainsKey(objectToAdd), Is.True);
            Assert.That(_lruCache.TryGetValue(objectToAdd, out dynamic result), Is.True);
            Assert.That(result, Is.EqualTo(objectToAdd));
        }

        [Test]
        public void When_Adding_To_The_Cache_Over_Capacity_Then_Oldest_Is_Removed()
        {
            // Given a value to add to lruCache
            const string keyValue = "oldest";

            // When key is added to lruCache and an additional max number of entries are added
            _lruCache.Add(keyValue, keyValue);
            AddCacheEntries(_lruCache, _maxSize);

            // Then lruCache contains only max items and first item added no longer exists
            Assert.That(_lruCache.Count, Is.LessThanOrEqualTo(_maxSize));
            Assert.That(_lruCache.ContainsKey(keyValue), Is.False);
        }

        [Test]
        public void When_Adding_To_The_Cache_Over_Capacity_And_Fetching_Oldest_Entry_Then_Second_Oldest_Is_Removed()
        {
            // Given two values to add to the lruCache
            const string valueToUpdate = "updateValue";
            const string secondOldestValue = "secondOldestValue";

            // When the value that will be updated is added and another added afterwards
            _lruCache.Add(valueToUpdate, valueToUpdate);
            _lruCache.Add(secondOldestValue, secondOldestValue);

            // And the lruCache is fully populated
            AddCacheEntries(_lruCache, _maxSize - 2);

            // And the first value added is refreshed by getting it 
            _lruCache.TryGetValue(valueToUpdate, out _);

            // And new entries are added to the lruCache
            AddCacheEntries(_lruCache, _maxSize - 2);

            // Then the updated value still exists in the lruCache and the second value does not
            Assert.That(_lruCache.Count, Is.EqualTo(_maxSize));
            Assert.That(_lruCache.ContainsKey(valueToUpdate), Is.True);
            Assert.That(_lruCache.ContainsKey(secondOldestValue), Is.False);
        }

        [Test]
        public void When_Setting_Value_Of_Existing_Key_Then_Value_Updated_And_Renewed()
        {
            // Given one value to add to the lruCache and another to later update that value to 
            const string updateValue = "updateValue";
            const string updatedValue = "updatedValue";

            // When the value is added and the lruCache fully populated
            _lruCache.Add(updateValue, updateValue);
            AddCacheEntries(_lruCache, _maxSize - 1);
            
            // And the original value is updated and the lruCache repopulated
            _lruCache[updateValue] = updatedValue;
            AddCacheEntries(_lruCache, _maxSize - 1);

            // The value still exists in the lruCache as it was updated
            Assert.That(_lruCache.Count, Is.EqualTo(_maxSize));
            Assert.That(_lruCache.ContainsKey(updateValue), Is.True);
            Assert.That(_lruCache[updateValue], Is.EqualTo(updatedValue));
        }

        [Test]
        [TestCaseSource(nameof(GetCases))]
        public void When_Getting_Value_Then_Value_Is_Returned_If_Exists(dynamic key, dynamic value, bool shouldAdd)
        {
            // When a lruCache is fully populated
            AddCacheEntries(_lruCache, _maxSize);

            // If a new value should be added to the lruCache and is added
            if (shouldAdd) _lruCache.Add(key, value);

            // Then it should be able to be fetched from the lruCache - otherwise it should not
            Assert.That(_lruCache.TryGetValue(key, out dynamic result), Is.EqualTo(shouldAdd));
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        [TestCaseSource(nameof(AddCases))]
        public void When_Value_Removed_Then_Value_No_Longer_In_Cache(dynamic objectToAdd)
        {
            // When a value is added to the lruCache
            _lruCache.Add(objectToAdd, objectToAdd);

            // Then it exists in the lruCache
            Assert.That(_lruCache.ContainsKey(objectToAdd), Is.True);

            // When a value is removed from the lruCache
            _lruCache.Remove(objectToAdd);

            // Then the value should not exist in the lruCache
            Assert.That(_lruCache.ContainsKey(objectToAdd), Is.False);
        }

        [Test]
        public void When_Retrieving_Directly_With_Non_Existing_Key_Then_Key_Not_Found_Exception_Thrown()
        {
            Assert.Throws<KeyNotFoundException>(() =>
            {
                // When a value that does not exist in the lruCache is referenced directly exception is thrown
                var unused = _lruCache["doesNotExist"];
            });
        }

        private static void AddCacheEntries(LruCache<dynamic, dynamic> lruCache, int numAdditions)
        {
            for (var i = 0; i < numAdditions; i++)
            {
                var ticks = DateTime.Now.Ticks;
                var key = $"key{ticks}";
                var value = $"value{ticks}";
                lruCache.Add(key, value);
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
