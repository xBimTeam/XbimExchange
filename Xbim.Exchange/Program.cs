using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Xbim.CobieExpress;
using Xbim.CobieLiteUK.Validation;
using Xbim.CobieLiteUK.Validation.Reporting;
using Xbim.Common.Step21;
using Xbim.COBieLiteUK;
using Xbim.Ifc;
using Xbim.IO;
using Xbim.IO.Memory;
using Xbim.ModelGeometry.Scene;
using XbimExchanger.COBieLiteUkToIfc;
using XbimExchanger.IfcToCOBieExpress;
using XbimExchanger.IfcToCOBieLiteUK;



namespace Xbim.Exchange
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            if (args.Length < 1)
            {
                Console.WriteLine("No IIfc or xBim file specified");
                return;
            }
            var fileName = args[0];
            Console.WriteLine("Reading " + fileName);
           
            using (var model = GetModel(fileName))
            {
                if (model == null)
                {
                    Console.WriteLine("No model to process. Press any key to exit");
                    Console.Read();
                    return;
                }
                   
                    var context = new Xbim3DModelContext(model);
                    context.CreateContext();
                    var wexBimFilename = Path.ChangeExtension(fileName, "wexBIM");
                    using (var wexBiMfile = new FileStream(wexBimFilename, FileMode.Create, FileAccess.Write))
                    {
                        using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                        {
                            Console.WriteLine("Creating " + wexBimFilename);
                            model.SaveAsWexBim(wexBimBinaryWriter);
                            wexBimBinaryWriter.Close();
                        }
                        wexBiMfile.Close();
                    }
                //now do COBieExpress
                var cobie = new MemoryModel(new EntityFactory());
                var cobieExpressFile = Path.ChangeExtension(fileName, ".cobie");
                var cobieExpressXmlFile = Path.ChangeExtension(fileName, ".cobieXml");
                var cobieExpressZipFile = Path.ChangeExtension(fileName, ".cobieZip");
                var w = new Stopwatch();
                using (var txn = cobie.BeginTransaction("IFC data in"))
                {
                    var exchanger = new IfcToCoBieExpressExchanger(model, cobie);
                    w.Start();
                    exchanger.Convert();
                    w.Stop();
                    txn.Commit();
                }
                Console.WriteLine("COBieExpress model created in {0}ms", w.ElapsedMilliseconds);
                cobie.SaveAsStep21(File.Create(cobieExpressFile));
                cobie.SaveAsStep21Zip(File.Create(cobieExpressZipFile));
                cobie.SaveAsXml(File.Create(cobieExpressXmlFile), new XmlWriterSettings{Indent = true, IndentChars = "\t"});


                    //now do the DPoW files
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    var fileDirectoryName = Path.GetDirectoryName(fileName);
                    var facilities = new List<Facility>();
                    var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(model, facilities);
                    facilities = ifcToCoBieLiteUkExchanger.Convert();
                    
                    var facilityNumber = 0;

                    foreach (var facility in facilities)
                    {
                        var dpow = "DPoW";
                        if (facilities.Count > 1) 
                            dpow += ++facilityNumber;
                        // ReSharper disable AssignNullToNotNullAttribute
                        var dPoWFile = Path.Combine(fileDirectoryName, fileNameWithoutExtension + "_" + dpow);
                        // ReSharper restore AssignNullToNotNullAttribute
                        dPoWFile = Path.ChangeExtension(dPoWFile, "json");
                        Console.WriteLine("Creating " + dPoWFile);

                        facility.WriteJson(dPoWFile);
                        string cobieFile = Path.ChangeExtension(dPoWFile, "Xlsx");
                        Console.WriteLine("Creating " + cobieFile);
                        string error;
                        facility.WriteCobie(cobieFile, out error);
                        if (!string.IsNullOrWhiteSpace(error))
                            Console.WriteLine("COBie Errors: " + error);

                        dPoWFile = Path.ChangeExtension(dPoWFile, "xml");
                        Console.WriteLine("Creating " + dPoWFile);
                       // facility.WriteXml(dPoWFile);
                        var req = Facility.ReadJson(@"..\..\Tests\ValidationFiles\Lakeside_Restaurant-stage6-COBie.json");
                        var validator = new FacilityValidator();
                        var result = validator.Validate(req, facility);
                        var verificationResults = Path.ChangeExtension(dPoWFile, "verified.xlsx");
                        Console.WriteLine("Creating " + verificationResults);
                        //create report
                        using (var stream = File.Create(verificationResults))
                        {
                            var report = new ExcelValidationReport();
                            report.Create(result, stream, ExcelValidationReport.SpreadSheetFormat.Xlsx);
                            stream.Close();
                        }

                    facility.ValidateUK2012(Console.Out, true);
                        string cobieValidatedFile = Path.ChangeExtension(dPoWFile, "Validated.Xlsx");
                        facility.WriteCobie(cobieValidatedFile, out error);
                        dPoWFile = Path.ChangeExtension(dPoWFile, "xbim");
                        var credentials = new XbimEditorCredentials()
                        {
                            ApplicationDevelopersName = "XbimTeam",
                            ApplicationFullName = "Xbim.Exchanger",
                            EditorsOrganisationName = "Xbim Development Team",
                            EditorsFamilyName = "Xbim Tester",
                            ApplicationVersion = "3.0"
                        };
                        Console.WriteLine("Creating " + dPoWFile);
                    using (
                        var ifcModel = IfcStore.Create(credentials, IfcSchemaVersion.Ifc2X3,
                            XbimStoreType.EsentDatabase))
                        {                          
                            using (var txn = ifcModel.BeginTransaction("Convert from COBieLiteUK"))
                            {
                                var coBieLiteUkToIIfcExchanger = new CoBieLiteUkToIfcExchanger(facility, ifcModel);
                                coBieLiteUkToIIfcExchanger.Convert();
                                txn.Commit();
                                //var err = model.Validate(model.Instances, Console.Out);
                            }
                            dPoWFile = Path.ChangeExtension(dPoWFile, "ifc");
                            Console.WriteLine("Creating " + dPoWFile);
                            ifcModel.SaveAs(dPoWFile, IfcStorageType.Ifc);
                            ifcModel.Close();
                        }
                    }
                    model.Close();
                }
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        private static IfcStore GetModel(string fileName)
        {
            IfcStore openModel = null;
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                if (File.Exists(Path.ChangeExtension(fileName, "xbim"))) //use xBIM if exists
                    fileName = Path.ChangeExtension(fileName, "xbim");
                else if (File.Exists(Path.ChangeExtension(fileName, "ifc"))) //use ifc if exists
                    fileName = Path.ChangeExtension(fileName, "ifc");
                else if (File.Exists(Path.ChangeExtension(fileName, "ifczip"))) //use ifczip if exists
                    fileName = Path.ChangeExtension(fileName, "ifczip");
                else if (File.Exists(Path.ChangeExtension(fileName, "ifcxml"))) //use ifcxml if exists
                    fileName = Path.ChangeExtension(fileName, "ifcxml");
            }

            if (!File.Exists(fileName)) return null;

                extension = Path.GetExtension(fileName);
            if (string.Compare(extension, ".xbim", StringComparison.OrdinalIgnoreCase) == 0) //just open xbim
                {

                    try
                    {
                        
                        Console.WriteLine("Opening " + fileName);
                        var model = IfcStore.Open(fileName);
                        //delete any geometry
                        openModel = model;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to open model {0}, {1}", fileName, e.Message);
                        Console.WriteLine("Unable to open model {0}, {1}", fileName, e.Message);
                    }

                }
                else //we need to create the xBIM file
                {
                   
                    try
                    {
                        Console.WriteLine("Creating " + Path.GetFileNameWithoutExtension(fileName));
                        openModel = IfcStore.Open(fileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to open model {0}, {1}", fileName, e.Message);
                        Console.WriteLine("Unable to open model {0}, {1}", fileName, e.Message);
                    }

                }
            return openModel;
        }
    }
}
