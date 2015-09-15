using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBieLiteUK;
using Attribute = Xbim.COBieLiteUK.Attribute;


namespace Tests
{
    [TestClass]
    public class FacilityMergeTest
    {
        
        [TestMethod]
        public void MergeFacilityTest()
        {
            var baseFacility = CreateFacilty();
            AddDeepAttributes(baseFacility, false);
            var mergeFacility = CreateFacilty();
            AddDeepAttributes(mergeFacility, true); //create two attributes that will merge on deepest Attribute list
            using (StreamWriter sw = new StreamWriter("MergeFacilityTest.log"))
            {
                sw.AutoFlush = true;
                baseFacility.Merge(mergeFacility, sw);
            }
            var rootNo = baseFacility.Attributes.Count; //should be original 6 by baseFacility - CreateFacilty + 3 added by AddDeepAttributes
            var nest1No = baseFacility.Attributes.Last().Attributes.Count; // should be original 3 added by baseFacility - AddDeepAttributes but nothing merged as all mergeFacility - AddDeepAttributes are duplicated (3 duplicates not counted)
            var nert2No = baseFacility.Attributes.Last().Attributes.Last().Attributes.Count; //should be original 3 added by baseFacility - AddDeepAttributes + 2 merged form mergeFacility - AddDeepAttributes (1 duplicates not counted)
            var totalAtt = rootNo + nest1No + nert2No;
            Assert.IsTrue(totalAtt == 17);
        }

        //Temp function, delete
        private void AddDeepAttributes(Facility facility, bool change)
        {
            var addlist1 = new List<Attribute>
                    {
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 1",
                            Value = new StringAttributeValue {Value = "Depth 1a"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 2",
                            Value =  new StringAttributeValue {Value = "Depth 1b"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 3",
                            Value =  new StringAttributeValue {Value = "Duplicate"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
            };
            if (facility.Attributes != null)
            {
                facility.Attributes.AddRange(addlist1);

            }
            else
                facility.Attributes = addlist1;

            var addlist2 = new List<Attribute>
                    {
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 1",
                            Value = new StringAttributeValue {Value = "Depth 2a"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 2",
                            Value =  new StringAttributeValue {Value = "Depth 2b"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 3",
                            Value =  new StringAttributeValue {Value = "Duplicate"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
            };
            if (facility.Attributes.Last().Attributes != null)
            {
                facility.Attributes.Last().Attributes.AddRange(addlist2);
            }
            else
                facility.Attributes.Last().Attributes = addlist2;

            var addlist3 = new List<Attribute>
                    {
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 1",
                            Value = new StringAttributeValue {Value = "Depth 3a"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = !change ? "String attribute 2" :  "String attribute 2 modified",
                            Value =  new StringAttributeValue {Value = "Depth 3b"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
                        new Attribute
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = new ContactKey {Email = "martin.cerny@northumbria.ac.uk"},
                            Name = "String attribute 3",
                            Value =  new StringAttributeValue {Value = !change ? "Depth 3c" : "Depth 3 modified"},
                            Categories = new List<Category> {new Category {Code = "Submitted", Classification = "Sample"}},
                        },
            };
            if (facility.Attributes.Last().Attributes.Last().Attributes != null)
            {
                facility.Attributes.Last().Attributes.Last().Attributes.AddRange(addlist3);
            }
            else
                facility.Attributes.Last().Attributes.Last().Attributes = addlist3;


        }
    

    private Facility CreateFacilty ()
        {
            return new Facility
            {
                CreatedOn = DateTime.Now,
                CreatedBy = new ContactKey { Email = "martin.cerny@northumbria.ac.uk" },
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
                Attributes = new List<Attribute>
                {
                    new Attribute
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
