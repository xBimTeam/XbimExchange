using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.Extensions;
using Xbim.CobieLiteUK.Validation.RequirementDetails;


namespace Xbim.CobieLiteUK.Validation
{
    class CachedPropertiesAndAttributesValidator<T> where T : CobieObject, new()
    {
        private readonly T _assetToTest;
        private readonly Dictionary<string, Attribute> _dicAtt;
        private readonly Dictionary<RequirementDetail, bool> _dicReqs = new Dictionary<RequirementDetail, bool>();

        public CachedPropertiesAndAttributesValidator(T assetToTest)
        {
            _assetToTest = assetToTest;
            if (assetToTest.Attributes == null)
                return;
            _dicAtt = assetToTest.Attributes.ToDictionary(att => att.Name, att => att);
        }

        internal bool CanSatisfy(RequirementDetail req, out object retValue)
        {
            retValue = null;
            bool ret;
            var propValue = _assetToTest.GetCobieProperty(req.Name);
            if (propValue != null)
            {
                retValue = propValue.ToObject();
                ret = req.IsSatisfiedBy(propValue);
                if (ret)
                    return RememberResult(req, true);
            }

            if (_dicAtt == null || !_dicAtt.ContainsKey(req.Name))
            {
                return RememberResult(req, false);
            }
            retValue = _dicAtt[req.Name].Value; // report value in any case.
            ret = req.IsSatisfiedBy(_dicAtt[req.Name]);
            return RememberResult(req, ret);
        }

        private bool RememberResult(RequirementDetail req, bool p)
        {
            if (!_dicReqs.ContainsKey(req))
                _dicReqs.Add(req, p);
            else 
                _dicReqs[req] = p;
            return p;
        }

        internal bool AlreadySatisfies(RequirementDetail req)
        {
            return _dicReqs.ContainsKey(req) && _dicReqs[req];
        }
    }
}
