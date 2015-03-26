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

        public static Category Clone(this Category originalCategory)
        {
            return new Category()
            {
                Classification = originalCategory.Classification,
                Code = originalCategory.Code,
                Description = originalCategory.Description
            };
        }

        public static IEnumerable<Category> Clone(this IEnumerable<Category> originalCategories)
        {
            return originalCategories.Select(originalCategory => originalCategory.Clone());
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
