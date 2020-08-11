namespace SpaceLogistic.Utility
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
    }
}
