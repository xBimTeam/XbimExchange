using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLite.Validation.Extensions
{
    static public class AssetTypesExtensions
    {
        static public IEnumerable<AssetType> GetClassificationSubset(this IEnumerable<AssetType> types, Category requiredCategory, bool includeCategoryChildren = true)
        {
            return includeCategoryChildren 
                ? types.Where(x => x.Categories.ContainsChildOf(requiredCategory)) 
                : types.Where(x => x.Categories.ContainsExactMatchTo(requiredCategory));
        }
    }
}
