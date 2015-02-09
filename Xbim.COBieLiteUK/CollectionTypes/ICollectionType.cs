using System.Collections.Generic;

namespace Xbim.COBieLite.CollectionTypes
{
    public interface ICollectionType<T>
    {
        List<T> InnerList { get; }
    }

    
    public static class CollectionTypeExtensions
    {
        public static void Add<T>(this ICollectionType<T> basef, T item)
        {
            basef.InnerList.Add(item);
        }

        public static void AddRange<T>(this ICollectionType<T> basef, IEnumerable<T> items)
        {
            basef.InnerList.AddRange(items);
        }
    }
}
