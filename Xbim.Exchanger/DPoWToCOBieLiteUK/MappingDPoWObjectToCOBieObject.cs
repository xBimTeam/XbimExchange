using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.DPoW;
using Attribute = Xbim.COBieLiteUK.Attribute;
using ProjectStage = Xbim.DPoW.ProjectStage;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingDPoWObjectToCOBieObject<TFrom, TTo> : MappingAttributableObjectToCOBieObject<TFrom, TTo>
        where TFrom : DPoWObject
        where TTo : CobieObject, new() 
    {
        protected override TTo Mapping(TFrom sObject, TTo tObject)
        {
            //perform base mapping where free attributes are converted
            base.Mapping(sObject, tObject);

            tObject.ExternalId = sObject.Id.ToString();
            tObject.ExternalSystem = "DPoW";
            tObject.Name = sObject.Name;
            tObject.Description = sObject.Description;

            var suffix = sObject is Xbim.DPoW.AssetType ? ((sObject as Xbim.DPoW.AssetType).Variant ?? "") : "";
            if(tObject.Categories == null) tObject.Categories = new List<Category>();
            tObject.Categories.AddRange(Exchanger.SourceRepository.GetEncodedClassification(sObject.ClassificationReferenceIds, suffix));

            
            //LOD
            if (sObject.RequiredLOD != null)
            {
                if (tObject.Attributes == null) tObject.Attributes = new List<Attribute>();
                var lod = sObject.RequiredLOD;
                tObject.Attributes.Add("RequiredLODCode", "Required LOD Code", lod.Code, "RequiredLOD");
                tObject.Attributes.Add("RequiredLODDescription", "Required LOD Description", lod.Description, "RequiredLOD");
                tObject.Attributes.Add("RequiredLODURI", "Required LOD URI", lod.URI, "RequiredLOD");
            }

            //LOI
            if (sObject.RequiredLOI != null)
            {
                if (tObject.Attributes == null) tObject.Attributes = new List<Attribute>();
                var loi = sObject.RequiredLOI;
                tObject.Attributes.Add("RequiredLOICode", "Required LOI Code", loi.Code, "RequiredLOI");
                tObject.Attributes.Add("RequiredLOIDescription", "Required LOI Description", loi.Description, "RequiredLOI");
                //required attributes with encoded property set name
                foreach (var sAttr in loi.RequiredAttributes)
                    tObject.Attributes.Add(new Attribute
                    {
                        ExternalEntity = sAttr.PropertySetName,
                        Categories = new List<Category> { new Category{ Code = "required" , Classification = "DPoW"} },
                        Name = sAttr.Name, 
                        Description = sAttr.Description,
                    });
            }
            
            //Issues from Jobs + Responsibilities
            if (sObject.Jobs != null && sObject.Jobs.Any())
            {
                if(tObject.Issues == null) tObject.Issues = new List<Issue>();
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
                tObject.ProjectStages.Add(new ProjectStageKey{ Name = stage.Name });
            }

            return tObject;
        }
    }
}
