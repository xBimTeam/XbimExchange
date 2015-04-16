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
