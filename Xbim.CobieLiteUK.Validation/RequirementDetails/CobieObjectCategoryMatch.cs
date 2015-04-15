using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
{
    internal class CobieObjectCategoryMatch<T> where T : CobieObject
    {
        public T MatchedObject;
        public List<Category> MatchingCategories;

        public CobieObjectCategoryMatch(T evaluatingType)
        {
            MatchedObject = evaluatingType;
            MatchingCategories = new List<Category>();
        }
    }
}
