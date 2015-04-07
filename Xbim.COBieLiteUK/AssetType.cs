using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    public static class AssetTypeExtensions
    {
        static public IEnumerable<AssetType> Clone(this IEnumerable<AssetType> types)
        {
            // todo: retain jsonSerialiser for faster execution
            if (types == null)
                yield break;
            foreach (var assetType in types)
            {
                yield return Facility.Clone(assetType);
            }
        }
    }


    public partial class AssetType
    {
        public AssetPortability AssetTypeEnum
        {
            get
            {
                if (String.IsNullOrEmpty(AssetTypeCustom)) return AssetPortability.notdefined;

                //try to parse string value
                AssetPortability result;
                if (Enum.TryParse(AssetTypeCustom, true, out result))
                    return result;

                //try to use aliases
                var enumMembers = typeof(AssetPortability).GetFields();
                foreach (var member in from member in enumMembers
                                       let alias = member.GetCustomAttributes<AliasAttribute>()
                                           .FirstOrDefault(
                                               a => String.Equals(a.Value, AssetTypeCustom, StringComparison.CurrentCultureIgnoreCase))
                                       where alias != null
                                       select member)
                    return (AssetPortability)member.GetValue(result);

                //if nothing fits it is a user defined value
                return AssetPortability.userdefined;
            }
            set
            {
                switch (value)
                {
                    case AssetPortability.notdefined:
                        AssetTypeCustom = null;
                        break;
                    case AssetPortability.userdefined:
                        break;
                    default:
                        AssetTypeCustom = Enum.GetName(typeof(AreaUnit), value);
                        break;
                }
            }
        }

        internal override IEnumerable<CobieObject> GetChildren()
        {
            foreach (var child in base.GetChildren())
                yield return child;
            if(Assets != null)
                foreach (var asset in Assets)
                {
                    yield return asset;
                }
            if (Spares != null)
                foreach (var spare in Spares)
                {
                    yield return spare;
                }
            if (Jobs != null)
                foreach (var job in Jobs)
                {
                    yield return job;
                }
            if (AssemblyOf != null)
                yield return AssemblyOf;
        }

        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (Manufacturer != null)
                yield return Manufacturer;
            if (Warranty != null)
            {
                if (Warranty.GuarantorLabor != null)
                    yield return Warranty.GuarantorLabor;
                if (Warranty.GuarantorParts != null)
                    yield return Warranty.GuarantorParts;
            }
        }
    }
}
