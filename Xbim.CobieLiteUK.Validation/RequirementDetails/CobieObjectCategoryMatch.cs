using System.Collections.Generic;
using Xbim.CobieLiteUk;

namespace Xbim.CobieLiteUk.Validation.RequirementDetails
{
    internal class CobieObjectCategoryMatch 
    {
        public CobieObject MatchedObject;
        public List<Category> MatchingCategories;

        public CobieObjectCategoryMatch(CobieObject evaluatingType)
        {
            MatchedObject = evaluatingType;
            MatchingCategories = new List<Category>();
        }
    }
}
