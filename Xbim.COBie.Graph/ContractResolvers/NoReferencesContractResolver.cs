using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.Graph.ContractResolvers
{
    /// <summary>
    /// This contract resolver disables serialization of referenced CobieObjects (like attribtes or anything else) 
    /// or list of such objects. Primary purpose of this is to be able to create independent lightweight objects
    /// where references are resolved in a different manner in graph database by using the actual relationships.
    /// </summary>
    public class NoReferencesContractResolver: DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            //don't serialize if the value is null. Ignore this rule if there is another rule defined already
            if (prop.ShouldSerialize == null && member is PropertyInfo) prop.ShouldSerialize = obj =>
                 ((PropertyInfo)member).GetValue(obj) != null;


            //disable serialization of nested cobie objects or lists of cobie objects
            if (typeof(CobieObject).IsAssignableFrom(prop.PropertyType) ||
                typeof(IEnumerable<CobieObject>).IsAssignableFrom(prop.PropertyType))
            {
                //This overwrites the rule above and is more restrictive
                prop.ShouldSerialize = obj => false;
            }

            return prop;
        }
    }
}
