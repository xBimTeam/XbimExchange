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
        [TestMethod]
        public void CoBieLiteUkCreation()
        {
            var facility = new Facility
            {
                ExternalId = System.Guid.NewGuid().ToString(),
                AreaUnits = AreaUnit.squaremeters,
                CurrencyUnit = CurrencyUnit.GBP,
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
                        ExternalId = Guid.NewGuid().ToString(),
                        Name = "Zone A",
                        Category = "45.789.78",
                        Description = "Description of the zone A",
                        Spaces = new List<SpaceKey>
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
                        new XmlSerializerNamespaces(new[]
                        {
                            new XmlQualifiedName("cobielite", "http://openbim.org/schemas/cobieliteuk"),
                            new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance") 
                        }));
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
