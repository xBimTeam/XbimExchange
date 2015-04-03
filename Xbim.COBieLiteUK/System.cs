using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xbim.XbimExtensions.Transactions.Extensions;

namespace Xbim.COBieLiteUK
{
    public partial class System
    {
        [XmlIgnore, JsonIgnore]
        public IEnumerable<Asset> AssetObjects
        {
            get
            {
                if (_facility == null)
                    throw new Exception(
                        "You have to call 'Refresh()' method on Facility object before you use this property.");
                if (Components == null) return new List<Asset>();
                var names = Components.Select(s => s.Name);
                return _facility.Get<Asset>(s => names.Contains(s.Name));
            }
        }

        internal protected override List<CobieObject> MergeDuplicates(List<CobieObject> objects, TextWriter log)
        {
            var candidates = objects.OfType<System>().ToList();
            if (!candidates.Any()) return new List<CobieObject>();

            var categoryStrings = (Categories ?? new List<Category>()).Select(c => c.CategoryString);
            var duplicates =
                candidates.Where(s => s.Name == Name).ToList();
            duplicates.Remove(this);
            if(!duplicates.Any()) return new List<CobieObject>();

            //get all components into this one
            foreach (var duplicate in duplicates)
            {
                if(duplicate.Components == null) continue;
                Components.AddRange(duplicate.Components);

                //check if the category is the same. It doesn't have to be according to COBie specification as it is a compound
                //key but it would make any attached attributes ambiguous
                var categoryDif = duplicate.Categories.Where(c => !categoryStrings.Contains(c.CategoryString)).ToList();
                if (categoryDif.Any())
                    log.WriteLine("There are multiple systems with the same name but different category. This is a legal in COBie sheet but will make any attributes related to this object ambiguous. Object: {0}, different category: {1}", duplicate.Name, String.Join("; ", categoryDif.Select(c => c.CategoryString)));
            }

            return duplicates.Cast<CobieObject>().ToList();
        }

        internal override IEnumerable<IEntityKey> GetKeys()
        {
            foreach (var key in base.GetKeys())
                yield return key;
            if (Components == null) yield break;
            foreach (var key in Components)
                yield return key;
        }
    }
}
