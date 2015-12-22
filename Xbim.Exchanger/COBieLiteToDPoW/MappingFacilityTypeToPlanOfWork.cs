using System;
using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.COBieLiteToDPoW
{
    /// <summary>
    /// This is the main class used to convert COBieLite to DPoW. It does all the branching for different parts of the model 
    /// on the first level of object hierarchy.
    /// </summary>
    class MappingFacilityTypeToPlanOfWork : COBieLiteToDPoWMapping<FacilityType, PlanOfWork>
    {
        protected override PlanOfWork Mapping(FacilityType source, PlanOfWork target)
        {
            target.Facility = new Facility();
            var tFacility = target.Facility;

            //tFacility.Category = new ClassificationReference() { ClassificationCode = source.FacilityCategory };
            //tFacility.FacilityDescription = source.FacilityDescription;
            //tFacility.FacilityName = source.FacilityName;
            //tFacility.FacilitySiteDescription = source.SiteAssignment != null ? source.SiteAssignment.SiteDescription : null;
            //tFacility.FacilitySiteName = source.SiteAssignment != null ? source.SiteAssignment.SiteName : null;
            //
            //target.Project = new Project();
            //var tProject = target.Project;
            //tProject.AreaUnits = Helper.TryConvertEnum<AreaUnitSimpleType, AreaUnits>(source.FacilityDefaultAreaUnit, AreaUnits.squaremeters);
            //tProject.CurrencyUnits = Helper.TryConvertEnum<CurrencyUnitSimpleType, CurrencyUnits>(source.FacilityDefaultCurrencyUnit, CurrencyUnits.GBP);
            //tProject.LinearUnits = Helper.TryConvertEnum<LinearUnitSimpleType, LinearUnits>(source.FacilityDefaultLinearUnit, LinearUnits.millimeters);
            //tProject.ProjectDescription = source.ProjectAssignment != null ? source.ProjectAssignment.ProjectDescription : null;
            //tProject.ProjectName = source.ProjectAssignment != null ? source.ProjectAssignment.ProjectName : null;
            //
            //target.ClassificationSystem = new List<Classification>();
            //var cls = new Classification() { ClassificationName = "Default"};
            //target.ClassificationSystem.Add(cls);
            
            

            throw new NotImplementedException();
        }
    }
}
