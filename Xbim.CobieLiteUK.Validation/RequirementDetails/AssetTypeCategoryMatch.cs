using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
{
    internal class AssetTypeCategoryMatch
    {
        public AssetType MatchedAssetType;
        public List<Category> MatchingCategories;

        public AssetTypeCategoryMatch(AssetType evaluatingType)
        {
            MatchedAssetType = evaluatingType;
            MatchingCategories = new List<Category>();
        }
    }
}
