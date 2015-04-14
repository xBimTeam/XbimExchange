using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
{
    internal class AssetTypeCategoryMatch<T> where T : CobieObject, new()
    {
        public T MatchedAssetType;
        public List<Category> MatchingCategories;

        public AssetTypeCategoryMatch(T evaluatingType)
        {
            MatchedAssetType = evaluatingType;
            MatchingCategories = new List<Category>();
        }
    }
}
