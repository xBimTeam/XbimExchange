using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{

    public partial class AssetType
    {
        public AssetType()
        {
            //init inner object for warranty.
            Warranty = new Warranty{GuarantorLabor = new ContactKey(), GuarantorParts = new ContactKey()};
        }

        internal override void AfterCobieRead()
        {
            base.AfterCobieRead();
            if (AssemblyOf != null && AssemblyOf.ChildAssetsOrTypes != null)
            {
                foreach (var key in AssemblyOf.ChildAssetsOrTypes)
                {
                    key.KeyType = EntityType.AssetType;
                }
            }
        }

        public AssetPortability AssetTypeEnum
        {
            get { return GetEnumeration<AssetPortability>(AssetTypeCustom); }
            set { SetEnumeration(value, s => AssetTypeCustom = s); }
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

            if (Warranty == null) yield break;
            if (Warranty.GuarantorLabor != null)
                yield return Warranty.GuarantorLabor;
            if (Warranty.GuarantorParts != null)
                yield return Warranty.GuarantorParts;
        }

        internal override void RemoveKey(IEntityKey key)
        {
            base.RemoveKey(key);
            if (Manufacturer == key)
                Manufacturer = null;
            if(Warranty == null) return;
            if (Warranty.GuarantorLabor == key)
                Warranty.GuarantorLabor = null;
            if (Warranty.GuarantorParts == key)
                Warranty.GuarantorParts = null;
        }
    }
}
