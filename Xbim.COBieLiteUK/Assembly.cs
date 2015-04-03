using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    public partial class Assembly 
    {
        internal protected override List<CobieObject> MergeDuplicates(List<CobieObject> objects, TextWriter log)
        {
            var candidates = objects.OfType<Assembly>().ToList();
            if (!candidates.Any()) return new List<CobieObject>();

            var duplicates =
                candidates.Where(
                    s => s.Name == Name && s._parentNameValue == _parentNameValue && s._parentSheet == _parentSheet)
                    .ToList();
            duplicates.Remove(this);
            if (!duplicates.Any()) return new List<CobieObject>();

            //get all components into this one
            foreach (var duplicate in duplicates)
            {
                if (duplicate.ChildAssetsOrTypes == null) continue;
                ChildAssetsOrTypes.AddRange(duplicate.ChildAssetsOrTypes);
            }

            return duplicates.Cast<CobieObject>().ToList();
        }

        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (ChildAssetsOrTypes == null) yield break;
            foreach (var key in ChildAssetsOrTypes)
                yield return key;
        }
    }
}
