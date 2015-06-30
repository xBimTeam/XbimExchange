using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.COBie.Graph.ContractResolvers;
using Xbim.COBieLiteUK;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace Xbim.Cobie.Graph.Tests
{
    [TestClass]
    public class CobieSerializationTests
    {
        [TestMethod]
        public void NoReferencesSerialization()
        {
            var serializer = new JsonSerializer {ContractResolver = new NoReferencesContractResolver(), Formatting = Formatting.Indented};
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var facility = CreateSampleFacility();

            serializer.Serialize(writer, facility);
            writer.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            var facility2 = Facility.ReadJson(stream);

            var reader = new StreamReader(new MemoryStream(stream.GetBuffer()));
            var data = reader.ReadToEnd();
            Debug.WriteLine(data);

            //there should be no references objects
            Assert.IsTrue(facility2.Floors == null);
            Assert.IsTrue(facility2.Contacts == null);
            Assert.IsTrue(facility2.Zones == null);
        }

        public Facility CreateSampleFacility()
        {
            return new Facility
            {
                CreatedOn = DateTime.Now,
                CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                Categories =
                    new List<Category>
                    {
                        new Category {Code = "Bd_34_54", Description = "Schools", Classification = "Sample"}
                    },
                ExternalId = Guid.NewGuid().ToString(),
                AreaUnits = AreaUnit.squaremeters,
                CurrencyUnit = CurrencyUnit.GBP,
                LinearUnits = LinearUnit.millimeters,
                VolumeUnits = VolumeUnit.cubicmeters,
                AreaMeasurement = "NRM",
                Phase = "Phase A",
                Description = "New facility description",
                Name = "Ellison Building",
                Project = new Project
                {
                    ExternalId = Guid.NewGuid().ToString(),
                    Name = "Project A"
                },
                Site = new Site
                {
                    ExternalId = Guid.NewGuid().ToString(),
                    Name = "Site A"
                },
                Zones = new List<Zone>
                {
                    new Zone
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        ExternalId = Guid.NewGuid().ToString(),
                        Name = "Zone A",
                        Categories = new List<Category> {new Category {Code = "45.789.78", Classification = "Sample"}},
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
                    new Contact
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Categories = new List<Category> {new Category {Code = "12.45.56", Classification = "Sample"}},
                        FamilyName = "Martin",
                        Email = "martin.cerny@northumbria.ac.uk",
                        GivenName = "Cerny"
                    },
                    new Contact
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Categories = new List<Category> {new Category {Code = "12.45.56", Classification = "Sample"}},
                        FamilyName = "Peter",
                        Email = "peter.pan@northumbria.ac.uk",
                        GivenName = "Pan"
                    },
                    new Contact
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Categories = new List<Category> {new Category {Code = "12.45.56", Classification = "Sample"}},
                        FamilyName = "Paul",
                        Email = "paul.mccartney@northumbria.ac.uk",
                        GivenName = "McCartney"
                    }
                },
                Floors = new List<Floor>
                {
                    new Floor
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Elevation = 15000,
                        Height = 3400,
                        Spaces = new List<Space>
                        {
                            new Space
                            {
                                CreatedOn = DateTime.Now,
                                CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                                Categories =
                                    new List<Category> {new Category {Code = "Sp_02_78_98", Classification = "Sample"}},
                                Description = "First front room in COBieLiteUK ever",
                                Name = "A001 - Front Room",
                                UsableHeight = 3500,
                                NetArea = 6
                            },
                            new Space
                            {
                                CreatedOn = DateTime.Now,
                                CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                                Categories =
                                    new List<Category> {new Category {Code = "Sp_02_78_98", Classification = "Sample"}},
                                Description = "First living room in COBieLiteUK ever",
                                Name = "A002 - Living Room",
                                UsableHeight = 4200,
                                NetArea = 55
                            },
                            new Space
                            {
                                CreatedOn = DateTime.Now,
                                CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                                Categories =
                                    new List<Category> {new Category {Code = "Sp_02_78_98", Classification = "Sample"}},
                                Description = "First bedroom in COBieLiteUK ever",
                                Name = "A003 - Bedroom",
                                UsableHeight = 4100,
                                NetArea = 25
                            }
                        }
                    }
                },
                AssetTypes = new List<AssetType>
                {
                    new AssetType
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Name = "Brick layered wall",
                        Assets = new List<Asset>
                        {
                            new Asset
                            {
                                CreatedOn = DateTime.Now,
                                Name = "120mm partition wall",
                                Representations = new List<Representation>
                                {
                                    new Representation
                                    {
                                        CreatedOn = DateTime.Now,
                                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                                        X = 0,
                                        Y = 0,
                                        Z = 0,
                                        SizeX = 1000,
                                        SizeY = 2000,
                                        SizeZ = 200,
                                        Name = Guid.NewGuid().ToString()
                                    }
                                },
                                CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"}
                            },
                            new Asset
                            {
                                CreatedOn = DateTime.Now,
                                Name = "180mm partition wall",
                                Representations = new List<Representation>
                                {
                                    new Representation
                                    {
                                        CreatedOn = DateTime.Now,
                                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                                        X = 0,
                                        Y = 0,
                                        Z = 0,
                                        SizeX = 1000,
                                        SizeY = 2000,
                                        SizeZ = 200,
                                        Name = Guid.NewGuid().ToString()
                                    }
                                },
                                CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"}
                            },
                            new Asset
                            {
                                CreatedOn = DateTime.Now,
                                Name = "350mm external brick wall",
                                Representations = new List<Representation>
                                {
                                    new Representation
                                    {
                                        CreatedOn = DateTime.Now,
                                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                                        X = 0,
                                        Y = 0,
                                        Z = 0,
                                        SizeX = 1000,
                                        SizeY = 2000,
                                        SizeZ = 200,
                                        Name = Guid.NewGuid().ToString()
                                    }
                                },
                                CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"}
                            }
                        }
                    }
                },
                Attributes = new List<Xbim.COBieLiteUK.Attribute>
                {
                    new COBieLiteUK.Attribute
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Name = "String attribute",
                        Value = new StringAttributeValue {Value = "Almukantarant"},
                        Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                    },
                    new Attribute
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Name = "Boolean attribute",
                        Value = new BooleanAttributeValue {Value = true},
                        Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                    },
                    new Attribute
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Name = "Datetime attribute",
                        Value = new DateTimeAttributeValue {Value = DateTime.Now},
                        Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                    },
                    new Attribute
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Name = "Decimal attribute",
                        Value = new DecimalAttributeValue {Value = 256.2},
                        Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                    },
                    new Attribute
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Name = "Integer attribute",
                        Value = new IntegerAttributeValue {Value = 7},
                        Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                    },
                    new Attribute
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                        Name = "Null attribute"
                    }
                },
                Stages = new List<ProjectStage>(new[]
                {
                    new ProjectStage
                    {
                        Name = "Stage 0",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(5),
                        End = DateTime.Now.AddDays(10),
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"}
                    },
                    new ProjectStage
                    {
                        Name = "Stage 1",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(10),
                        End = DateTime.Now.AddDays(20),
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"}
                    },
                    new ProjectStage
                    {
                        Name = "Stage 2",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(20),
                        End = DateTime.Now.AddDays(110),
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"}
                    },
                    new ProjectStage
                    {
                        Name = "Stage 3",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(110),
                        End = DateTime.Now.AddDays(300),
                        CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"}
                    },
                })
            };
        }
    }
}
