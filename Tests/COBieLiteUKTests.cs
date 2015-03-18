using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBieLiteUK;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace Tests
{
    [TestClass]
    public class CoBieLiteUkTests
    {
        //[TestMethod]
        //public void CoBieLiteUkCreation()
        //{
        //    var model = new ProjectType();
        //    model.ProjectStages = new List<ProjectStageType>();
        //    model.ProjectStages.AddRange(new[]{
        //        new ProjectStageType
        //        {
        //            externalID = Guid.NewGuid().ToString(),
        //            ProjectStageCode = "A4",
        //            ProjectStageDescription = "Stage A4",
        //            ProjectStageName = "Preliminary design",
        //            ProjectStageStartDate = DateTime.Now,
        //            ProjectStageEndDate = DateTime.Now.AddDays(28),
        //            ProjectStageAttributes = new AttributeCollectionType
        //            {
        //                Attribute = new List<AttributeType>(new []{
        //                    new AttributeType
        //                    {
        //                        AttributeName = "New attribute 1",
        //                        externalID = Guid.NewGuid().ToString(),
        //                        AttributeCategory = "Requirement",
        //                        AttributeValue = new AttributeValueType
        //                        {Item = new AttributeStringValueType {StringValue = "Value AAA"},
        //                            ItemElementName = ItemChoiceType.AttributeStringValue}
        //                    },
        //                    new AttributeType
        //                    {
        //                        AttributeName = "New attribute 2",
        //                        externalID = Guid.NewGuid().ToString(),
        //                        AttributeCategory = "Requirement",
        //                        AttributeValue = new AttributeValueType
        //                        {Item = new AttributeStringValueType {StringValue = "Value BBB"},
        //                            ItemElementName = ItemChoiceType.AttributeStringValue}
        //                    }
        //                })
        //            },
        //            Facility = new FacilityType
        //            {
        //                externalID = Guid.NewGuid().ToString(),
        //                FacilityDefaultAreaUnit = AreaUnitSimpleType.squaremeters,
        //                FacilityDefaultAreaUnitSpecified = true,
        //                FacilityDefaultCurrencyUnit = CurrencyUnitSimpleType.BritishPounds,
        //                FacilityDefaultCurrencyUnitSpecified = true,
        //                FacilityDefaultLinearUnit = LinearUnitSimpleType.millimeters,
        //                FacilityDefaultLinearUnitSpecified = true,
        //                FacilityDefaultMeasurementStandard = "NRM",
        //                FacilityDefaultVolumeUnit = VolumeUnitSimpleType.cubicmeters,
        //                FacilityDefaultVolumeUnitSpecified = true,
        //                FacilityDeliverablePhaseName = "Phase A",
        //                FacilityDescription = "New facility description",
        //                FacilityName = "Ellison Building",
        //                ProjectAssignment = new ProjectType {externalID = Guid.NewGuid().ToString(), ProjectName = "Project A"},
        //                SiteAssignment = new SiteType {externalID = Guid.NewGuid().ToString(), SiteName = "Site A"},
        //                Zones = new ZoneCollectionType
        //                {Zone = new List<ZoneType>(new []{
        //                    new ZoneType
        //                    {
        //                        externalID = Guid.NewGuid().ToString(),
        //                        ZoneName = "Zone A",
        //                        ZoneCategory = "45.789.78",
        //                        ZoneDescription = "Description of the zone A"
        //                    }
        //                })}
        //            }
        //        },
        //        new ProjectStageType
        //        {
        //            externalID = Guid.NewGuid().ToString(),
        //            ProjectStageCode = "A4",
        //            ProjectStageDescription = "Stage A4",
        //            ProjectStageName = "Preliminary design",
        //            ProjectStageStartDate = DateTime.Now,
        //            ProjectStageEndDate = DateTime.Now.AddDays(28),
        //            ProjectStageAttributes = new AttributeCollectionType
        //            {
        //                Attribute = new List<AttributeType>(new []{
        //                    new AttributeType
        //                    {
        //                        AttributeName = "New attribute 1",
        //                        externalID = Guid.NewGuid().ToString(),
        //                        AttributeCategory = "Requirement",
        //                        AttributeValue = new AttributeValueType
        //                        {Item = new AttributeStringValueType {StringValue = "Value AAA"},
        //                            ItemElementName = ItemChoiceType.AttributeStringValue}
        //                    },
        //                    new AttributeType
        //                    {
        //                        AttributeName = "New attribute 2",
        //                        externalID = Guid.NewGuid().ToString(),
        //                        AttributeCategory = "Requirement",
        //                        AttributeValue = new AttributeValueType
        //                        {Item = new AttributeStringValueType {StringValue = "Value BBB"},
        //                            ItemElementName = ItemChoiceType.AttributeStringValue}
        //                    }
        //                })
        //            },
        //            Facility = new FacilityType
        //            {
        //                externalID = Guid.NewGuid().ToString(),
        //                FacilityDefaultAreaUnit = AreaUnitSimpleType.squaremeters,
        //                FacilityDefaultAreaUnitSpecified = true,
        //                FacilityDefaultCurrencyUnit = CurrencyUnitSimpleType.BritishPounds,
        //                FacilityDefaultCurrencyUnitSpecified = true,
        //                FacilityDefaultLinearUnit = LinearUnitSimpleType.millimeters,
        //                FacilityDefaultLinearUnitSpecified = true,
        //                FacilityDefaultMeasurementStandard = "NRM",
        //                FacilityDefaultVolumeUnit = VolumeUnitSimpleType.cubicmeters,
        //                FacilityDefaultVolumeUnitSpecified = true,
        //                FacilityDeliverablePhaseName = "Phase A",
        //                FacilityDescription = "New facility description",
        //                FacilityName = "Ellison Building",
        //                ProjectAssignment = new ProjectType {externalID = Guid.NewGuid().ToString(), ProjectName = "Project A"},
        //                SiteAssignment = new SiteType {externalID = Guid.NewGuid().ToString(), SiteName = "Site A"},
        //                Zones = new ZoneCollectionType
        //                {Zone = new List<ZoneType>(new []{
        //                    new ZoneType
        //                    {
        //                        externalID = Guid.NewGuid().ToString(),
        //                        ZoneName = "Zone A",
        //                        ZoneCategory = "45.789.78",
        //                        ZoneDescription = "Description of the zone A"
        //                    }
        //                })}
        //            }
        //        },
        //        new ProjectStageType
        //        {
        //            externalID = Guid.NewGuid().ToString(),
        //            ProjectStageCode = "A4",
        //            ProjectStageDescription = "Stage A4",
        //            ProjectStageName = "Preliminary design",
        //            ProjectStageStartDate = DateTime.Now,
        //            ProjectStageEndDate = DateTime.Now.AddDays(28),
        //            ProjectStageAttributes = new AttributeCollectionType
        //            {
        //                Attribute = new List<AttributeType>(new []{
        //                    new AttributeType
        //                    {
        //                        AttributeName = "New attribute 1",
        //                        externalID = Guid.NewGuid().ToString(),
        //                        AttributeCategory = "Requirement",
        //                        AttributeValue = new AttributeValueType
        //                        {
        //                            Item = new AttributeStringValueType {StringValue = "Value AAA"},
        //                            ItemElementName = ItemChoiceType.AttributeStringValue
        //                        }
        //                    },
        //                    new AttributeType
        //                    {
        //                        AttributeName = "New attribute 2",
        //                        externalID = Guid.NewGuid().ToString(),
        //                        AttributeCategory = "Requirement",
        //                        AttributeValue = new AttributeValueType
        //                        {
        //                            Item = new AttributeStringValueType {StringValue = "Value BBB"},
        //                            ItemElementName = ItemChoiceType.AttributeStringValue
        //                        }
        //                    }
        //                })
        //            },
        //            Facility = new FacilityType
        //            {
        //                externalID = Guid.NewGuid().ToString(),
        //                FacilityDefaultAreaUnit = AreaUnitSimpleType.squaremeters,
        //                FacilityDefaultAreaUnitSpecified = true,
        //                FacilityDefaultCurrencyUnit = CurrencyUnitSimpleType.BritishPounds,
        //                FacilityDefaultCurrencyUnitSpecified = true,
        //                FacilityDefaultLinearUnit = LinearUnitSimpleType.millimeters,
        //                FacilityDefaultLinearUnitSpecified = true,
        //                FacilityDefaultMeasurementStandard = "NRM",
        //                FacilityDefaultVolumeUnit = VolumeUnitSimpleType.cubicmeters,
        //                FacilityDefaultVolumeUnitSpecified = true,
        //                FacilityDeliverablePhaseName = "Phase A",
        //                FacilityDescription = "New facility description",
        //                FacilityName = "Ellison Building",
        //                ProjectAssignment = new ProjectType {externalID = Guid.NewGuid().ToString(), ProjectName = "Project A"},
        //                SiteAssignment = new SiteType {externalID = Guid.NewGuid().ToString(), SiteName = "Site A"},
        //                Zones = new ZoneCollectionType
        //                {Zone = new List<ZoneType>(new []{
        //                    new ZoneType
        //                    {
        //                        externalID = Guid.NewGuid().ToString(),
        //                        ZoneName = "Zone A",
        //                        ZoneCategory = "45.789.78",
        //                        ZoneDescription = "Description of the zone A"
        //                    }
        //                })}
        //            }
        //        }
        //    });
        //    using (var file = File.Create("test.xml"))
        //    {
        //        model.Save(file);
        //        file.Close();                
        //    }
        //}

        [TestMethod]
        public void CoBieLiteUkCreation()
        {
            var facility = new Facility
            {
                ExternalID = System.Guid.NewGuid().ToString(),
                AreaUnits = AreaUnit.squaremeters,
                CurrencyUnit = CurrencyUnit.BritishPounds,
                LinearUnits = LinearUnit.millimeters,
                AreaMeasurement = "NRM",
                VolumeUnits = VolumeUnit.cubicmeters,
                Phase = "Phase A",
                Description = "New facility description",
                Name = "Ellison Building",
                ExternalProjectIdentifier = Guid.NewGuid().ToString(),
                ProjectName = "Project A",
                ExternalSiteIdentifier = Guid.NewGuid().ToString(),
                SiteName = "Site A",
                Zones = new List<Zone>
                {
                    new Zone
                    {
                        ExternalID = Guid.NewGuid().ToString(),
                        Name = "Zone A",
                        Category = "45.789.78",
                        Description = "Description of the zone A",
                        SpacesAssignment = new List<SpaceKey>
                        {
                            new SpaceKey {Name = "A001 - Front Room"},
                            new SpaceKey {Name = "A002 - Living Room"},
                            new SpaceKey {Name = "A003 - Bedroom"},
                        }
                    }
                },
                Contacts = new List<Contact>
                {
                    new Contact{CreatedOn = DateTime.Now, Category = "12.45.56", FamilyName = "Martin", Email = "martin.cerny@northumbria.ac.uk", GivenName = "Cerny"},
                    new Contact{CreatedOn = DateTime.Now, Category = "12.45.56", FamilyName = "Peter", Email = "peter.pan@northumbria.ac.uk", GivenName = "Pan"},
                    new Contact{CreatedOn = DateTime.Now, Category = "12.45.56", FamilyName = "Paul", Email = "paul.mccartney@northumbria.ac.uk", GivenName = "McCartney"}
                },
                Floors = new List<Floor> { new Floor
                {
                    CreatedOn = DateTime.Now,
                    Elevation = 15000,
                    Height = 3400,
                    Spaces = new List<Space>
                    {
                        new Space
                        {
                            CreatedOn = DateTime.Now,
                            Category = "Sp_02_78_98",
                            Description = "First front room in COBieLiteUK ever",
                            Name = "A001 - Front Room"
                        },
                        new Space
                        {
                            CreatedOn = DateTime.Now,
                            Category = "Sp_02_78_98",
                            Description = "First living room in COBieLiteUK ever",
                            Name = "A002 - Living Room"
                        },
                        new Space
                        {
                            CreatedOn = DateTime.Now,
                            Category = "Sp_02_78_98",
                            Description = "First bedroom in COBieLiteUK ever",
                            Name = "A003 - Bedroom"
                        }
                    }
                } },
                AssetTypes = new List<AssetType>
                {
                    new AssetType
                    {
                        CreatedOn = DateTime.Now,
                        Name = "Brick layered wall",
                        Assets = new List<Asset>
                        {
                            new Asset{CreatedOn = DateTime.Now, Name = "120mm partition wall"},
                            new Asset{CreatedOn = DateTime.Now, Name = "180mm partition wall"},
                            new Asset{CreatedOn = DateTime.Now, Name = "350mm external brick wall"}
                        }
                    }
                },
                Attributes = new List<Attribute>
                {
                    new Attribute{Name = "String attribute", Item = new StringAttributeValue{Value = "Almukantarant"}},
                    new Attribute{Name = "Boolean attribute", Item = new BooleanAttributeValue{Value = true}},
                    new Attribute{Name = "Datetime attribute", Item = new DateTimeAttributeValue{Value = DateTime.Now}},
                    new Attribute{Name = "Decimal attribute", Item = new DecimalAttributeValue{Value = 256.2}},
                    new Attribute{Name = "Integer attribute", Item = new IntegerAttributeValue{Value = 7}}
                }

            };

            //save model to file to check it
            var serializer = new XmlSerializer(typeof (Facility));
            using (var writer = File.CreateText("facility.cobielite.xml"))
            {
                using (var xmlWriter = new XmlTextWriter(writer){Formatting = Formatting.Indented})
                {
                    serializer.Serialize(xmlWriter, facility, 
                        new XmlSerializerNamespaces(new[] { new XmlQualifiedName("cobielite", "http://openbim.org/schemas/cobieliteuk")}));
                    xmlWriter.Close();
                }
            }

            //read model back to prove nothing is broken in the middle
            using (var reader = File.OpenText("facility.cobielite.xml"))
            {
                var facility2 = serializer.Deserialize(reader) as Facility;

            }
        }

    }
}
