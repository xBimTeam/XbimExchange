using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBieLiteUK;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xbim.IO;
using XbimExchanger.IfcToCOBieLiteUK;
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
                Stages = new List<ProjectStage>(new []
                {
                    new ProjectStage
                    {
                        Name = "Stage 0",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(5),
                        End = DateTime.Now.AddDays(10),
                        CreatedBy = new ContactKey{Email = "martin.cerny@northumbria.ac.uk"}
                    },
                    new ProjectStage
                    {
                        Name = "Stage 1",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(10),
                        End = DateTime.Now.AddDays(20),
                        CreatedBy = new ContactKey{Email = "martin.cerny@northumbria.ac.uk"}
                    },
                    new ProjectStage
                    {
                        Name = "Stage 2",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(20),
                        End = DateTime.Now.AddDays(110),
                        CreatedBy = new ContactKey{Email = "martin.cerny@northumbria.ac.uk"}
                    },
                    new ProjectStage
                    {
                        Name = "Stage 3",
                        CreatedOn = DateTime.Now,
                        Start = DateTime.Now.AddDays(110),
                        End = DateTime.Now.AddDays(300),
                        CreatedBy = new ContactKey{Email = "martin.cerny@northumbria.ac.uk"}
                    },
                })
            };

            //save model to file to check it
            string msg;
            const string xmlFile = "facility.cobielite.xml";
            const string jsonFile = "facility.cobielite.json";
            const string xlsxFile = "facility.cobielite.xlsx";
            facility.WriteXml(xmlFile, true);
            facility.WriteJson(jsonFile, true);
            facility.WriteCobie(xlsxFile, out msg);

            var facility2 = Facility.ReadXml(xmlFile);
            var facility3 = Facility.ReadJson(jsonFile);    
        }

        [TestMethod]
        [DeploymentItem("TestFiles\\2012-03-23-Duplex-Design.xlsx")]
        public void ReadingSpreadsheet()
        {
            string msg;
            var facility = Facility.ReadCobie("2012-03-23-Duplex-Design.xlsx", out msg);
            facility.WriteJson("..\\..\\2012-03-23-Duplex-Design.cobielite.json", true);

            Assert.AreEqual(AreaUnit.squaremeters, facility.AreaUnits);
            Assert.IsTrue(String.IsNullOrEmpty(msg));

            var log = new StringWriter();
            facility.ValidateUK2012(log, true);
            Debug.Write(log.ToString());
            Debug.WriteLine("----------------------------------------------------------------------");
            
            //second run after fixings
            log = new StringWriter();
            facility.ValidateUK2012(log, true);
            Debug.Write(log.ToString());

            facility.WriteCobie("..\\..\\2012-03-23-Duplex-Design.fixed.xlsx", out msg);

            var f2 = Facility.ReadJson("..\\..\\2012-03-23-Duplex-Design.cobielite.json");
        }

        [TestMethod]
        [DeploymentItem("TestFiles\\OBN1-COBie-UK-2014.xlsx")]
        public void ReadingUkSpreadsheet()
        {
            string msg;
            var facility = Facility.ReadCobie("OBN1-COBie-UK-2014.xlsx", out msg);
            facility.WriteJson("..\\..\\OBN1-COBie-UK-2014.cobielite.json", true);

            var log = new StringWriter();
            facility.ValidateUK2012(log, true);
            Debug.Write(log.ToString());
        }

        //[TestMethod]
        public void CobieFix()
        {
            var files = new[]
            {
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Ext01.fixed.xlsx",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Ext01.xlsx",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Ext01.xls",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Struc.xls",
                @"C:\Users\mxfm2\Downloads\Bad Cobie\Site.xls",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\INT02.xls",
                //@"C:\Users\mxfm2\Downloads\Bad Cobie\Int01.xls"
            };
            foreach (var file in files)
            {
                Stopwatch completeWatch = new Stopwatch();
                completeWatch.Start();

                var dir = Path.GetDirectoryName(file);
                var name = Path.GetFileNameWithoutExtension(file);
                var newFile = Path.Combine(dir ?? "", name + ".fixed.xlsx");
                Debug.WriteLine("============ Processing: " + (name ?? ""));

                using (var log = File.CreateText(Path.Combine(dir ?? "", name + ".fixed.txt")))
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    string msg;
                    var facility = Facility.ReadCobie(file, out msg);
                    stopWatch.Stop();

                    if(!String.IsNullOrEmpty(msg))
                        log.WriteLine(msg);
                    Debug.WriteLine("Reading COBie: " + stopWatch.ElapsedMilliseconds);

                    stopWatch.Reset();
                    stopWatch.Start();
                    facility.ValidateUK2012(log, true);
                    stopWatch.Stop();

                    Debug.WriteLine("Validating COBie: " + stopWatch.ElapsedMilliseconds);


                    stopWatch.Reset();
                    stopWatch.Start();
                    //Debug.Write(msg);
                    //Debug.Write(log.ToString());    
                    facility.WriteCobie(newFile, out  msg);
                    stopWatch.Stop();
                    if (!String.IsNullOrEmpty(msg))
                        log.WriteLine(msg);
                    Debug.WriteLine("Writing COBie: " + stopWatch.ElapsedMilliseconds);
                    log.Close();
                }

                completeWatch.Stop();
                Debug.WriteLine("========== Complete processing of {0}: {1}ms", name, completeWatch.ElapsedMilliseconds);
            }
        }

        [TestMethod]
        public void CobieAttributesCreation()
        {
            int i = 1;
            var attI = AttributeValue.CreateFromObject(i);

            Int16 i16 = 13;
            attI = AttributeValue.CreateFromObject(i16);

            Int32 i32 = 13;
            attI = AttributeValue.CreateFromObject(i32);

            DateTime d = DateTime.Now;
            var attD = AttributeValue.CreateFromObject(i);

            string s = "Yes";
            var attS = AttributeValue.CreateFromObject(s);

            bool b = true;
            var attB = AttributeValue.CreateFromObject(b);

            double dbl = 3.14;
            var attDbl = AttributeValue.CreateFromObject(dbl);

            AttributeValue dAt = new DecimalAttributeValue() {Value = Math.E };
            var fromA = AttributeValue.CreateFromObject(dAt);
        }

        [TestMethod]
        [DeploymentItem("TestFiles\\2012-03-23-Duplex-Design.xlsx")]
        public void WritingSpreadsheet()
        {
            string msg;
            var facility = Facility.ReadCobie("2012-03-23-Duplex-Design.xlsx", out msg);
            facility.WriteCobie("..\\..\\2012-03-23-Duplex-Design_enhanced.xlsx", out msg);
        }

        [TestMethod]
        [DeploymentItem("TestFiles\\OBN1-COBie-UK-2014.xlsx")]
        public void WritingUkSpreadsheet()
        {
            string msg;
            var facility = Facility.ReadCobie("OBN1-COBie-UK-2014.xlsx", out msg);
            facility.WriteCobie("..\\..\\OBN1-COBie-UK-2014_plain.xlsx", out msg, "UK2012", false);
        }

        [TestMethod]
        public void DeepSearchTest()
        {
            #region Model
            var facility = new Facility
            {
                Contacts = new List<Contact>(new []{
                    new Contact
                {
                Name    = "martin.cerny@northumbria.ac.uk"
                } 
                }),
                Floors = new List<Floor>(new[]
                {
                    new Floor
                    {
                        Name = "Floor 0",
                        Spaces = new List<Space>(new[]
                        {
                            new Space
                            {
                                Name = "Space A",
                                Attributes = new List<Attribute>(new[]
                                {
                                    new Attribute {Name = "Space A attribute 1"},
                                    new Attribute {Name = "Space A attribute 2"},
                                })
                            },
                            new Space
                            {
                                Name = "Space B",
                                Attributes = new List<Attribute>(new[]
                                {
                                    new Attribute {Name = "Space B attribute 1"},
                                    new Attribute {Name = "Space B attribute 2"},
                                })
                            }
                        })
                    },
                    new Floor
                    {
                        Name = "Floor 1",
                        Spaces = new List<Space>(new[]
                        {
                            new Space
                            {
                                Name = "Space C",
                                Attributes = new List<Attribute>(new[]
                                {
                                    new Attribute {Name = "Space C attribute 1"},
                                    new Attribute {Name = "Space C attribute 2"},
                                })
                            },
                            new Space
                            {
                                Name = "Space D",
                                Attributes = new List<Attribute>(new[]
                                {
                                    new Attribute {Name = "Space D attribute 1"},
                                    new Attribute {Name = "Space D attribute 2"},
                                })
                            }
                        })
                    },
                    new Floor
                    {
                        Name = "Floor 2",
                        Spaces = new List<Space>(new[]
                        {
                            new Space
                            {
                                Name = "Space E",
                                Attributes = new List<Attribute>(new[]
                                {
                                    new Attribute {Name = "Space E attribute 1"},
                                    new Attribute {Name = "Space E attribute 2"},
                                })
                            },
                            new Space
                            {
                                Name = "Space F",
                                Attributes = new List<Attribute>(new[]
                                {
                                    new Attribute {Name = "Space F attribute 1"},
                                    new Attribute {Name = "Space F attribute 2"},
                                })
                            }
                        })
                    }
                })
            };
            #endregion

            var allAttributes = facility.Get<Attribute>();
            Assert.AreEqual(12, allAttributes.Count());

            var allSpaces = facility.Get<Space>();
            Assert.AreEqual(6, allSpaces.Count());

            var spaceA = facility.Get<Space>(s => s.Name == "Space A");
            Assert.AreEqual(1, spaceA.Count());

            var self = facility.Get<Facility>().FirstOrDefault();
            Assert.IsNotNull(self);

            var contact = facility.Get<CobieObject>(c => c.GetType() == typeof (Contact) && c.Name == "martin.cerny@northumbria.ac.uk");
        }
        [DeploymentItem("TestFiles\\OBN1-COBie-UK-2014.xlsx")]
        [TestMethod]
        [DeploymentItem("ValidationFiles\\Lakeside_Restaurant.ifc")]
        public void IfcToCoBieLiteUkTest()
        {
            using (var m = new XbimModel())
            {
                const string ifcTestFile = @"Lakeside_Restaurant.ifc";
                var xbimTestFile = Path.ChangeExtension(ifcTestFile, "xbim");
                var jsonFile = Path.ChangeExtension(ifcTestFile, "json");
                m.CreateFrom(ifcTestFile, xbimTestFile, null, true, true);
                var facilities = new List<Facility>();
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(m, facilities);
                facilities = ifcToCoBieLiteUkExchanger.Convert();

                foreach (var facilityType in facilities)
                {
                    var log = new StringWriter();
                    facilityType.ValidateUK2012(log, true);

                    string msg;
                    facilityType.WriteJson(jsonFile, true);
                    facilityType.WriteCobie("..\\..\\Lakeside_Restaurant.xlsx", out msg, "UK2012", true);

                    
                    break;
                }
            }
        }
    }
}