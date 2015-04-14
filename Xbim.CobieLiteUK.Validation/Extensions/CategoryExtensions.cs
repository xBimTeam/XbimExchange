using System.Collections.Generic;
using System.Linq;
using NPOI.SS.Formula.Functions;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.RequirementDetails;

namespace Xbim.CobieLiteUK.Validation.Extensions
{
    /// <summary>
    /// Provides extension methods to simplify the execution of validation algorithms on CobieLiteUK categories.
    /// </summary>
    public  static class CategoryExtensions
    {

        /// <summary>
        /// Filters a provided enumerable of AssetTypes matching a specified category
        /// </summary>
        /// <param name="types">The initial enumerable</param>
        /// <param name="requiredCategory">Classification and Codes of the provided categories will be tested for matches</param>
        /// <param name="includeCategoryChildren">if true extends the matching rule to include all categories starting with the required code</param>
        /// <returns></returns>
        static internal IEnumerable<AssetTypeCategoryMatch<T>> GetClassificationMatches<T>(this Category requiredCategory, IEnumerable<T> types, bool includeCategoryChildren = true) where T : CobieObject, new()
        {
            if (requiredCategory == null)
                return Enumerable.Empty<AssetTypeCategoryMatch<T>>();

            var buildingDictionary = new Dictionary<T, AssetTypeCategoryMatch<T>>();

            foreach (var evaluatingType in types)
            {
                if (evaluatingType.Categories == null)
                    continue;

                var buffer = includeCategoryChildren
                    ? evaluatingType.Categories.MatchingChildrenOf(requiredCategory).ToList()
                    : evaluatingType.Categories.Matching(requiredCategory).ToList();

                if (!buffer.Any())
                    continue;
                if (!buildingDictionary.ContainsKey(evaluatingType))
                    buildingDictionary.Add(evaluatingType, new AssetTypeCategoryMatch<T>(evaluatingType));
                buildingDictionary[evaluatingType].MatchingCategories.AddRange(buffer);
            }

            return buildingDictionary.Values;
        }


        public static bool IsChildOf(this Category testedCategory, Category requiredCategory)
        {
            return 
                //testedCategory.Classification == requiredCategory.Classification 
                //&& 
                testedCategory.Code.StartsWith(requiredCategory.Code);
        }

        public static bool ExactlyMatches(this Category testedCategory, Category requiredCategory)
        {
            return
                //testedCategory.Classification == requiredCategory.Classification
                //&&
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
            if (originalCategories == null)
                yield break;

            foreach (var cat in originalCategories)
            {
                yield return cat.Clone();
            }
        }

        public static IEnumerable<string> MatchingClassifications(this IEnumerable<Category> initialList, IEnumerable<Category> otherList)
        {
            if (initialList == null || otherList == null)
                return Enumerable.Empty<string>();
            return initialList.Select(i => i.Classification).Intersect(otherList.Select(o => o.Classification));
        }

        public static bool ContainsChildOf(this IEnumerable<Category> testedCategories, Category requiredCategory)
        {
            return testedCategories.Any(testedCategory => testedCategory.IsChildOf(requiredCategory));
        }

        public static IEnumerable<Category>MatchingChildrenOf(this IEnumerable<Category> testedCategories, Category requiredCategory)
        {
            return testedCategories.Where(testedCategory => testedCategory.IsChildOf(requiredCategory));
        }

        public static IEnumerable<Category> Matching(this IEnumerable<Category> testedCategories, Category requiredCategory)
        {
            return testedCategories.Where(testedCategory => testedCategory.ExactlyMatches(requiredCategory));
        }

        public static bool ContainsExactMatchTo(this IEnumerable<Category> testedCategories, Category requiredCategory)
        {
            return testedCategories.Any(testedCategory => testedCategory.ExactlyMatches(requiredCategory));
        }

        
    }
}
