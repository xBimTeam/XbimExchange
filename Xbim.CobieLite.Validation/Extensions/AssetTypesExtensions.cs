using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLite.Validation.Extensions
{
    static public class AssetTypesExtensions
    {
        static public IEnumerable<AssetType> GetClassificationSubset(this IEnumerable<AssetType> types, Category requiredCategory, bool includeCategoryChildren = true)
        {
            if (requiredCategory == null)
                return Enumerable.Empty<AssetType>();
            return includeCategoryChildren 
                ? types.Where(x => x.Categories != null && x.Categories.ContainsChildOf(requiredCategory)) 
                : types.Where(x => x.Categories != null && x.Categories.ContainsExactMatchTo(requiredCategory));
        }
    }
}
