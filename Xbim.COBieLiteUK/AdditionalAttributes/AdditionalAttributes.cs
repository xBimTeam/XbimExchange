using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK.AdditionalAttributes
{
    public abstract class AdditionalAttributes
    {
        private CobieObject _object;

        protected AdditionalAttributes(CobieObject cObject)
        {
            _object = cObject;
        }

        protected Attribute GetOrCreateAttribute<T>(string name) where T: AttributeValue, new()
        {
            if (_object.Attributes == null)
                _object.Attributes = new List<Attribute>();
            var attribute = _object.Attributes.FirstOrDefault(a => a.Name == name);
            if (attribute != null) return attribute;

            attribute = new Attribute{Value = new T(), Name = name};
            _object.Attributes.Add(attribute);
            return attribute;
        }
     
    }
}
