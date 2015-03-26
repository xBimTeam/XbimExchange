using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Extensions
{
    public  static class CategoryExtensions
    {
        public static bool IsChildOf(this Category testedCategory, Category requiredCategory)
        {
            return 
                testedCategory.Classification == requiredCategory.Classification 
                && 
                testedCategory.Code.StartsWith(requiredCategory.Code);
        }

        public static bool ExactlyMatches(this Category testedCategory, Category requiredCategory)
        {
            return
                testedCategory.Classification == requiredCategory.Classification
                &&
                testedCategory.Code == requiredCategory.Code;
        }



        public static bool ContainsChildOf(this IEnumerable<Category> testedCategories, Category requiredCategory)
        {
            return testedCategories.Any(testedCategory => testedCategory.IsChildOf(requiredCategory));
        }

        public static bool ContainsExactMatchTo(this IEnumerable<Category> testedCategories, Category requiredCategory)
        {
            return testedCategories.Any(testedCategory => testedCategory.ExactlyMatches(requiredCategory));
        }

        
    }
}
