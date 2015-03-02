using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBieLiteUK;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    [TestClass]
    public class CoBieLiteUkTests
    {
        [TestMethod]
        public void CoBieLiteUkCreation()
        {
            var model = new ProjectType();
            model.ProjectStages = new List<ProjectStageType>();
            model.ProjectStages.AddRange(new[]{
                new ProjectStageType
                {
                    externalID = Guid.NewGuid().ToString(),
                    ProjectStageCode = "A4",
                    ProjectStageDescription = "Stage A4",
                    ProjectStageName = "Preliminary design",
                    ProjectStageStartDate = DateTime.Now,
                    ProjectStageEndDate = DateTime.Now.AddDays(28),
                    ProjectStageAttributes = new AttributeCollectionType
                    {
                        Attribute = new List<AttributeType>(new []{
                            new AttributeType
                            {
                                AttributeName = "New attribute 1",
                                externalID = Guid.NewGuid().ToString(),
                                AttributeCategory = "Requirement",
                                AttributeValue = new AttributeValueType
                                {Item = new AttributeStringValueType {StringValue = "Value AAA"},
                                    ItemElementName = ItemChoiceType.AttributeStringValue}
                            },
                            new AttributeType
                            {
                                AttributeName = "New attribute 2",
                                externalID = Guid.NewGuid().ToString(),
                                AttributeCategory = "Requirement",
                                AttributeValue = new AttributeValueType
                                {Item = new AttributeStringValueType {StringValue = "Value BBB"},
                                    ItemElementName = ItemChoiceType.AttributeStringValue}
                            }
                        })
                    },
                    Facility = new FacilityType
                    {
                        externalID = Guid.NewGuid().ToString(),
                        FacilityDefaultAreaUnit = AreaUnitSimpleType.squaremeters,
                        FacilityDefaultAreaUnitSpecified = true,
                        FacilityDefaultCurrencyUnit = CurrencyUnitSimpleType.BritishPounds,
                        FacilityDefaultCurrencyUnitSpecified = true,
                        FacilityDefaultLinearUnit = LinearUnitSimpleType.millimeters,
                        FacilityDefaultLinearUnitSpecified = true,
                        FacilityDefaultMeasurementStandard = "NRM",
                        FacilityDefaultVolumeUnit = VolumeUnitSimpleType.cubicmeters,
                        FacilityDefaultVolumeUnitSpecified = true,
                        FacilityDeliverablePhaseName = "Phase A",
                        FacilityDescription = "New facility description",
                        FacilityName = "Ellison Building",
                        ProjectAssignment = new ProjectType {externalID = Guid.NewGuid().ToString(), ProjectName = "Project A"},
                        SiteAssignment = new SiteType {externalID = Guid.NewGuid().ToString(), SiteName = "Site A"},
                        Zones = new ZoneCollectionType
                        {Zone = new List<ZoneType>(new []{
                            new ZoneType
                            {
                                externalID = Guid.NewGuid().ToString(),
                                ZoneName = "Zone A",
                                ZoneCategory = "45.789.78",
                                ZoneDescription = "Description of the zone A"
                            }
                        })}
                    }
                },
                new ProjectStageType
                {
                    externalID = Guid.NewGuid().ToString(),
                    ProjectStageCode = "A4",
                    ProjectStageDescription = "Stage A4",
                    ProjectStageName = "Preliminary design",
                    ProjectStageStartDate = DateTime.Now,
                    ProjectStageEndDate = DateTime.Now.AddDays(28),
                    ProjectStageAttributes = new AttributeCollectionType
                    {
                        Attribute = new List<AttributeType>(new []{
                            new AttributeType
                            {
                                AttributeName = "New attribute 1",
                                externalID = Guid.NewGuid().ToString(),
                                AttributeCategory = "Requirement",
                                AttributeValue = new AttributeValueType
                                {Item = new AttributeStringValueType {StringValue = "Value AAA"},
                                    ItemElementName = ItemChoiceType.AttributeStringValue}
                            },
                            new AttributeType
                            {
                                AttributeName = "New attribute 2",
                                externalID = Guid.NewGuid().ToString(),
                                AttributeCategory = "Requirement",
                                AttributeValue = new AttributeValueType
                                {Item = new AttributeStringValueType {StringValue = "Value BBB"},
                                    ItemElementName = ItemChoiceType.AttributeStringValue}
                            }
                        })
                    },
                    Facility = new FacilityType
                    {
                        externalID = Guid.NewGuid().ToString(),
                        FacilityDefaultAreaUnit = AreaUnitSimpleType.squaremeters,
                        FacilityDefaultAreaUnitSpecified = true,
                        FacilityDefaultCurrencyUnit = CurrencyUnitSimpleType.BritishPounds,
                        FacilityDefaultCurrencyUnitSpecified = true,
                        FacilityDefaultLinearUnit = LinearUnitSimpleType.millimeters,
                        FacilityDefaultLinearUnitSpecified = true,
                        FacilityDefaultMeasurementStandard = "NRM",
                        FacilityDefaultVolumeUnit = VolumeUnitSimpleType.cubicmeters,
                        FacilityDefaultVolumeUnitSpecified = true,
                        FacilityDeliverablePhaseName = "Phase A",
                        FacilityDescription = "New facility description",
                        FacilityName = "Ellison Building",
                        ProjectAssignment = new ProjectType {externalID = Guid.NewGuid().ToString(), ProjectName = "Project A"},
                        SiteAssignment = new SiteType {externalID = Guid.NewGuid().ToString(), SiteName = "Site A"},
                        Zones = new ZoneCollectionType
                        {Zone = new List<ZoneType>(new []{
                            new ZoneType
                            {
                                externalID = Guid.NewGuid().ToString(),
                                ZoneName = "Zone A",
                                ZoneCategory = "45.789.78",
                                ZoneDescription = "Description of the zone A"
                            }
                        })}
                    }
                },
                new ProjectStageType
                {
                    externalID = Guid.NewGuid().ToString(),
                    ProjectStageCode = "A4",
                    ProjectStageDescription = "Stage A4",
                    ProjectStageName = "Preliminary design",
                    ProjectStageStartDate = DateTime.Now,
                    ProjectStageEndDate = DateTime.Now.AddDays(28),
                    ProjectStageAttributes = new AttributeCollectionType
                    {
                        Attribute = new List<AttributeType>(new []{
                            new AttributeType
                            {
                                AttributeName = "New attribute 1",
                                externalID = Guid.NewGuid().ToString(),
                                AttributeCategory = "Requirement",
                                AttributeValue = new AttributeValueType
                                {
                                    Item = new AttributeStringValueType {StringValue = "Value AAA"},
                                    ItemElementName = ItemChoiceType.AttributeStringValue
                                }
                            },
                            new AttributeType
                            {
                                AttributeName = "New attribute 2",
                                externalID = Guid.NewGuid().ToString(),
                                AttributeCategory = "Requirement",
                                AttributeValue = new AttributeValueType
                                {
                                    Item = new AttributeStringValueType {StringValue = "Value BBB"},
                                    ItemElementName = ItemChoiceType.AttributeStringValue
                                }
                            }
                        })
                    },
                    Facility = new FacilityType
                    {
                        externalID = Guid.NewGuid().ToString(),
                        FacilityDefaultAreaUnit = AreaUnitSimpleType.squaremeters,
                        FacilityDefaultAreaUnitSpecified = true,
                        FacilityDefaultCurrencyUnit = CurrencyUnitSimpleType.BritishPounds,
                        FacilityDefaultCurrencyUnitSpecified = true,
                        FacilityDefaultLinearUnit = LinearUnitSimpleType.millimeters,
                        FacilityDefaultLinearUnitSpecified = true,
                        FacilityDefaultMeasurementStandard = "NRM",
                        FacilityDefaultVolumeUnit = VolumeUnitSimpleType.cubicmeters,
                        FacilityDefaultVolumeUnitSpecified = true,
                        FacilityDeliverablePhaseName = "Phase A",
                        FacilityDescription = "New facility description",
                        FacilityName = "Ellison Building",
                        ProjectAssignment = new ProjectType {externalID = Guid.NewGuid().ToString(), ProjectName = "Project A"},
                        SiteAssignment = new SiteType {externalID = Guid.NewGuid().ToString(), SiteName = "Site A"},
                        Zones = new ZoneCollectionType
                        {Zone = new List<ZoneType>(new []{
                            new ZoneType
                            {
                                externalID = Guid.NewGuid().ToString(),
                                ZoneName = "Zone A",
                                ZoneCategory = "45.789.78",
                                ZoneDescription = "Description of the zone A"
                            }
                        })}
                    }
                }
            });
            using (var file = File.Create("test.xml"))
            {
                model.Save(file);
                file.Close();                
            }
        }

        [TestMethod]
        public void DpoWToCoBieLiteUk()
        {

        }

    }
}
