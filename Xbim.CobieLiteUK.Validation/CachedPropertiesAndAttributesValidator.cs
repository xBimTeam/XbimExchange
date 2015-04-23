using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.CobieLiteUK.Validation.RequirementDetails;
using Xbim.COBieLiteUK;
using Attribute = Xbim.COBieLiteUK.Attribute;

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
            try
            {
                _dicAtt = assetToTest.Attributes.ToDictionary(att => att.Name, att => att);
            }
            catch (ArgumentException ex)
            {
                var names = _assetToTest.Attributes.Select(att => att.Name);
                var grps = names.GroupBy( i => i );
                var sb = new StringBuilder();
                sb.AppendLine("Invalid cobie data.");
                sb.AppendLine("Properties:");
                foreach( var grp in grps )
                {
                    if (grp.Count() > 1)
                    {
                        sb.AppendFormat(" - {0}\r\n", grp.Key);
                    }
                }
                sb.AppendFormat("Appear more than once in {0}", _assetToTest.Name);
                throw new ValidationException(sb.ToString(), ex);
            }
        }

        internal bool CanSatisfy(RequirementDetail req, out object retValue)
        {
            retValue = null;
            bool ret;
            var propValue = _assetToTest.GetCobieProperty(req.Name);
            if (propValue != null)
            {
                retValue = AttributeValue.CreateFromObject(propValue);
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
