using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
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
