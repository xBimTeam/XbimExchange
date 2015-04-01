using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.CobieLiteUK.Validation.RequirementDetails
{
    internal class RequirementProvision<T>
    {
        public RequirementDetail Requirement;
        public T ProvidedValue;
        
        public RequirementProvision(RequirementDetail reqDetail, T value)
        {
            Requirement = reqDetail;
            ProvidedValue = value;
        }
    }
}
