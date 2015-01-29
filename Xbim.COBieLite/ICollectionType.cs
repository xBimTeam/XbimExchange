using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLite
{
    public interface ICollectionType<T>
    {
        List<T> InnerList { get; }
    }

    public static class CollectionTypeExtensions
    {
        public static bool Any<T>(this ICollectionType<T> basef)
        {
            return basef.InnerList.Any();
        }

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
