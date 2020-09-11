﻿namespace SpaceLogistic.Utility
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        public static void RemoveWhere<TItem>(this IList<TItem> items, Func<TItem, bool> predicate)
        {
            var length = items.Count;
            for (var i = 0; i < length; )
            {
                if (predicate(items[i]))
                {
                    items.RemoveAt(i);
                    length--;
                }
                else
                {
                    i++;
                }
            }
        }

        public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> items, IEnumerable<TKey> keysToRemove)
        {
            foreach (var key in keysToRemove)
            {
                items.Remove(key);
            }
        }

        public static void Replace<TKey, TValue>(this IDictionary<TKey, TValue> items, IEnumerable<TValue> valuesToReplace, Func<TValue, TKey> keyFunc)
        {
            foreach (var value in valuesToReplace)
            {
                items[keyFunc(value)] = value;
            }
        }
    }
}
