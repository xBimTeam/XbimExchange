using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xbim.COBieLiteUK;
using Xbim.CobieLiteUK.Validation.RequirementDetails;


namespace Xbim.CobieLiteUK.Validation
{
    class CachedPropertiesAndAttributesValidator
    {
        private Asset _modelAsset;
        private AssetType _candidateType;
        private readonly Dictionary<string, Attribute> _dicAtt;
        private  Dictionary<RequirementDetail, bool> _dicReqs = new Dictionary<RequirementDetail, bool>();

        public CachedPropertiesAndAttributesValidator(AssetType typeToTest)
        {
            _candidateType = typeToTest;
            _dicAtt = typeToTest.Attributes.ToDictionary(att => att.Name, att => att);
        }

        public CachedPropertiesAndAttributesValidator(Asset assetToTest)
        {

            this._modelAsset = assetToTest;
            _dicAtt = assetToTest.Attributes.ToDictionary(att => att.Name, att => att);
        }

        internal bool CanSatisfy(RequirementDetail req, out object retValue)
        {
            retValue = null;
            if (!_dicAtt.ContainsKey(req.Name))
            {
                return Report(req, false);
            }
            retValue = _dicAtt[req.Name].Value; // report value in any case.
            var ret = req.IsSatisfiedBy(_dicAtt[req.Name]);
            return Report(req, ret);
        }

        private bool Report(RequirementDetail req, bool p)
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
