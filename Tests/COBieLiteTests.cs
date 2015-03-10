using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.COBieLite;
using Xbim.COBieLite.Converters;
using Xbim.IO;

// ReSharper disable once CheckNamespace
namespace Xbim.Tests.COBie
{
    [TestClass] 
    [DeploymentItem(@"TestFiles\")]
    public class CoBieLiteTests
    {
        [TestMethod]
        public void CanReadSerialisedJson()
        {
            var filename =
                @"007-Lakeside_Restaurant.dpow.json";
            //filename = "007-Lakeside_Restaurant-stage6-COBie.json";
            filename =
                Path.Combine(@"C:\Users\Bonghi\Google Drive\UNN\_Research\2014 12 01 - DPOW\_modelInfo\requirements4\",
                    filename);

            //var data = File.ReadAllText();
            //var t = JsonConvert.DeserializeObject<FacilityType>(data);
            var facility = FacilityType.ReadJson(filename);

        }

        [TestMethod]
        public void CanReadSerialisedXml()
        {
            try
            {
                CoBieLiteHelper.ReadXml(@"Facility1.xml");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    Debug.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void CanOpenTemporaryModel()
        {
            var model = XbimModel.CreateTemporaryModel();
            model.Initialise();
            var helper = new CoBieLiteHelper(model, "UniClass");
            helper.GetFacilities();
        }

        [TestMethod]
        public void ConvertCoBieLiteToJson()
        {
            using (var m = new XbimModel())
            {
                var IfcTestFile = "2012-03-23-Duplex-Handover.ifc";
                IfcTestFile = @"D:\Users\steve\My Documents\DPoW\001 NBS Lakeside Restaurant 2014.ifc";
                var XbimTestFile = Path.ChangeExtension(IfcTestFile, "xbim");
                var JsonFile = Path.ChangeExtension(IfcTestFile, "json");
                m.CreateFrom(IfcTestFile, XbimTestFile, null, true, true);
                var helper = new CoBieLiteHelper(m,"UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    using (var fs = new StreamWriter(JsonFile))
                    {
                        CoBieLiteHelper.WriteJson(fs, facilityType);
                        fs.Close();
                    }
                    
                }
            }
        }

        [TestMethod]
        public void ConvertCoBieLiteToXml()
        {

            using (var m = new XbimModel())
            {
                const string ifcTestFile = "2012-03-23-Duplex-Handover.ifc";
               // var IfcTestFile = @"D:\Users\steve\xBIM\Test Models\BimAlliance BillEast\Model 1 Duplex Apartment\Duplex_MEP_20110907.ifc";
                var xbimTestFile = Path.ChangeExtension(ifcTestFile, "xbim");
                m.CreateFrom(ifcTestFile, xbimTestFile, null, true, true);
                var helper = new CoBieLiteHelper(m, "UniClass");
                var facilities = helper.GetFacilities();
                var i = 1;
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    var outName = string.Format("Facility{0}.xml", i++);
                   
                    using (TextWriter writer = File.CreateText(outName))
                    {
                        CoBieLiteHelper.WriteXml(writer, facilityType);
                    }
                    CoBieLiteHelper.WriteXml(Console.Out, facilityType);

                    // attempt reading
                    CoBieLiteHelper.ReadXml(outName);
                }
            }
        } 
        [TestMethod]
        public void ConvertCoBieLiteToBson()
        {

            using (var m = new XbimModel())
            {
                const string ifcTestFile = "2012-03-23-Duplex-Handover.ifc";
               // IfcTestFile = @"C:\Data\dev\XbimTeam\XbimExchange\Tests\TestFiles\Standard_Classroom_CIC_6_Project_mod2.ifc";
                var xbimTestFile = Path.ChangeExtension(ifcTestFile, "xbim");
                m.CreateFrom(ifcTestFile, xbimTestFile, null, true, true);
                var helper = new CoBieLiteHelper(m, "UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    Assert.IsTrue(facilityType.FacilityDefaultLinearUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultAreaUnitSpecified);
                    Assert.IsTrue(facilityType.FacilityDefaultVolumeUnitSpecified);
                    using (var sw = new FileStream("facility.bson",FileMode.Create))
                    {
                        using (var bw = new BinaryWriter(sw))
                        {
                            CoBieLiteHelper.WriteBson(bw, facilityType);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void ConvertCoBieLiteToIfc()
        {

            using (var m = new XbimModel())
            {
                m.CreateFrom("2012-03-23-Duplex-Handover.ifc", "2012-03-23-Duplex-Handover.xbim", null, true, true);
                var helper = new CoBieLiteHelper(m, "UniClass");
                var facilities = helper.GetFacilities();
                foreach (var facilityType in facilities)
                {
                    using (new FileStream("facility.bson", FileMode.Create))
                    {                       
                         helper.WriteIfc(Console.Out, facilityType);
                    }
                }
            }
        }

        [TestMethod]
        public void ReadAndWriteCOBieLiteJSONWithValueConverter()
        {
            #region Facility definition
            var facility = new FacilityType()
            {
                FacilityAttributes = new AttributeCollectionType()
                {
                    Attribute = new List<AttributeType>(new []
                    {
                        new AttributeType()
                        {
                            AttributeName = "Null value",
                            AttributeValue = null
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null boolean item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeBooleanValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null datetime item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeDateTimeValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null date item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeDateValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null decimal item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeDecimalValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null int item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeIntegerValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null monetary item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeMonetaryValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null string item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeStringValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "Null time item value",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = null,
                                ItemElementName = ItemChoiceType.AttributeTimeValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "AttributeBooleanValue",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = new BooleanValueType(){BooleanValue = true, BooleanValueSpecified = true},
                                ItemElementName = ItemChoiceType.AttributeBooleanValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "AttributeDateTimeValue",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = DateTime.Now,
                                ItemElementName = ItemChoiceType.AttributeDateTimeValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "AttributeDateValue",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = DateTime.Now,
                                ItemElementName = ItemChoiceType.AttributeDateValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "AttributeTimeValue",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = DateTime.Now,
                                ItemElementName = ItemChoiceType.AttributeTimeValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "AttributeDecimalValue",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = new AttributeDecimalValueType(){DecimalValue = 0.12},
                                ItemElementName = ItemChoiceType.AttributeDecimalValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "AttributeIntegerValue",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = new AttributeIntegerValueType(){IntegerValue = 75},
                                ItemElementName = ItemChoiceType.AttributeIntegerValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "AttributeMonetaryValue",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = new AttributeMonetaryValueType(){MonetaryValue = 45},
                                ItemElementName = ItemChoiceType.AttributeMonetaryValue
                            }
                        },
                        new AttributeType()
                        {
                            AttributeName = "String Attribute",
                            AttributeValue = new AttributeValueType()
                            {
                                Item = new AttributeStringValueType(){StringValue = "String value"},
                                ItemElementName = ItemChoiceType.AttributeStringValue
                }
            }
                    })
                }
            };
            #endregion;

            using (var file = File.CreateText("facility.json"))
            {
                facility.WriteJson(file);
                file.Close();
            }

            //read it back and check the values
            var facility2 = FacilityType.ReadJson("facility.json");
            var attrs = facility2.FacilityAttributes.Attribute;
            var bAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeBooleanValue && a.AttributeValue.Item != null);
            var dAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeDateValue && a.AttributeValue.Item != null);
            var dtAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeDateTimeValue && a.AttributeValue.Item != null);
            var decAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeDecimalValue && a.AttributeValue.Item != null);
            var iAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeIntegerValue && a.AttributeValue.Item != null);
            var mAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeMonetaryValue && a.AttributeValue.Item != null);
            var sAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeStringValue && a.AttributeValue.Item != null);
            var tAttr = attrs.FirstOrDefault(a => a.AttributeValue != null && a.AttributeValue.ItemElementName == ItemChoiceType.AttributeTimeValue && a.AttributeValue.Item != null);

            Assert.IsNotNull(bAttr);
            Assert.IsNotNull(dAttr);
            Assert.IsNotNull(dtAttr);
            Assert.IsNotNull(decAttr);
            Assert.IsNotNull(iAttr);
            Assert.IsNotNull(mAttr);
            Assert.IsNotNull(sAttr);
            Assert.IsNotNull(tAttr);

            //check values
            Assert.IsTrue((bAttr.AttributeValue.Item as BooleanValueType).BooleanValue == true);
            var date = (DateTime)dAttr.AttributeValue.Item;
            Assert.IsTrue(date != default(DateTime));
            date = (DateTime)tAttr.AttributeValue.Item;
            Assert.IsTrue(date != default(DateTime));
            date = (DateTime)dtAttr.AttributeValue.Item;
            Assert.IsTrue(date != default(DateTime));

            Assert.IsTrue(Math.Abs((decAttr.AttributeValue.Item as AttributeDecimalValueType).DecimalValue) - 1e-9 < 1e-5);
            Assert.IsTrue((iAttr.AttributeValue.Item as AttributeIntegerValueType).IntegerValue == 75);
            Assert.IsTrue((mAttr.AttributeValue.Item as AttributeMonetaryValueType).MonetaryValue == 45);
            Assert.IsTrue((sAttr.AttributeValue.Item as AttributeStringValueType).StringValue == "String value");

        }
    }
}
