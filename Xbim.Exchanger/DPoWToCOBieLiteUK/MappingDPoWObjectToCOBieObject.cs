using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.DPoW;
using Xbim.Properties;
using Attribute = Xbim.COBieLiteUK.Attribute;
using ProjectStage = Xbim.DPoW.ProjectStage;
using Version = Xbim.Properties.Version;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    internal class MappingDPoWObjectToCOBieObject<TFrom, TTo> : MappingAttributableObjectToCOBieObject<TFrom, TTo>
        where TFrom : DPoWObject
        where TTo : CobieObject, new()
    {
        protected override TTo Mapping(TFrom sObject, TTo tObject)
        {
            //perform base mapping where free attributes are converted
            base.Mapping(sObject, tObject);

            tObject.ExternalId = sObject.Id.ToString();
            tObject.ExternalEntity = sObject.GetType().Name;
            tObject.ExternalSystem = "DPoW";
            tObject.Name = sObject.Name;
            tObject.Description = sObject.Description;

            var suffix = sObject is Xbim.DPoW.AssetType ? ((sObject as Xbim.DPoW.AssetType).Variant ?? "") : "";
            if (tObject.Categories == null) tObject.Categories = new List<Category>();
            tObject.Categories.AddRange(
                Exchanger.SourceRepository.GetEncodedClassification(sObject.ClassificationReferenceIds, suffix));


            //LOD
            if (sObject.RequiredLOD != null)
            {
                if (tObject.Attributes == null) tObject.Attributes = new List<Attribute>();
                var lod = sObject.RequiredLOD;
                tObject.Attributes.Add("RequiredLODCode", "Required LOD Code", lod.Code, "RequiredLOD");
                tObject.Attributes.Add("RequiredLODDescription", "Required LOD Description", lod.Description,
                    "RequiredLOD");
                tObject.Attributes.Add("RequiredLODURI", "Required LOD URI", lod.URI, "RequiredLOD");
            }

            //LOI
            if (sObject.RequiredLOI != null)
            {
                if (tObject.Attributes == null) tObject.Attributes = new List<Attribute>();
                var loi = sObject.RequiredLOI;
                tObject.Attributes.Add("RequiredLOICode", "Required LOI Code", loi.Code, "RequiredLOI");
                tObject.Attributes.Add("RequiredLOIDescription", "Required LOI Description", loi.Description,
                    "RequiredLOI");
                //required attributes with encoded property set name
                foreach (var sAttr in loi.RequiredAttributes)
                    tObject.Attributes.Add(new Attribute
                    {
                        Categories = new List<Category> {new Category {Code = "required", Classification = "DPoW"}},
                        Name = sAttr.Name,
                        Description = sAttr.Description,
                        PropertySetName = GetPsetName(GetUniclass2015(tObject.Categories), sAttr.Name)
                    });
            }

            //Issues from Jobs + Responsibilities
            if (sObject.Jobs != null && sObject.Jobs.Any())
            {
                if (tObject.Issues == null) tObject.Issues = new List<Issue>();
                var jMap = Exchanger.GetOrCreateMappings<MappingJobToIssueType>();
                foreach (var job in sObject.Jobs)
                {
                    var issue = jMap.GetOrCreateTargetObject(job.Id.ToString());
                    jMap.AddMapping(job, issue);
                    tObject.Issues.Add(issue);
                }
            }

            //project stage
            var stage = Exchanger.Context as ProjectStage;
            if (stage != null)
            {
                if (tObject.ProjectStages == null) tObject.ProjectStages = new List<ProjectStageKey>();
                tObject.ProjectStages.Add(new ProjectStageKey {Name = stage.Name});
            }

            return tObject;
        }

        private static readonly Definitions<PropertySetDef> PsetDefinitions;
        public static Dictionary<string, string> _psetDefinitionsCache = new Dictionary<string, string>();

        static MappingDPoWObjectToCOBieObject()
        {
            PsetDefinitions = new Definitions<PropertySetDef>(Version.IFC4);
            PsetDefinitions.LoadIFC4AndCOBie();
        }


        private string GetPsetName(Category category, string propertyName)
        {
            //get rid of any spaces in the name for a search
            propertyName = propertyName.Replace(" ", "");

            //try to get from cache
            string name;
            if (_psetDefinitionsCache.TryGetValue(propertyName, out name))
                return name;

            var psets =
                PsetDefinitions.DefinitionSets.Where(pset => pset.Definitions.Any(p => p.Name == propertyName))
                    .ToArray();
            if (psets.Length == 1)
            {
                _psetDefinitionsCache.Add(propertyName, psets[0].Name);
                return psets[0].Name;
            }

            return category == null
                ? "DPoW_Specified"
                : String.Join("_", "DPoW", category.Code, category.Description);
        }

        private Category GetUniclass2015(IEnumerable<Category> cats)
        {
            var categories = cats as Category[] ?? cats.ToArray();
            if (cats == null || !categories.Any())
                return null;

            foreach (var category in categories)
            {
                if (String.IsNullOrWhiteSpace(category.Classification)) continue;

                var clsName = (category.Classification ?? "").ToLower();
                if (clsName == "uniclass2015")
                    return category;
                if (clsName.Contains("uniclass") && clsName.Contains("2015"))
                    return category;
            }

            return null;
        }
    }
}