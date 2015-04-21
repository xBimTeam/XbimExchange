using System;
using System.Collections.Generic;
using System.IO;
using Xbim.COBieLiteUK;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger.COBieLiteUkToIfc;
using XbimExchanger.IfcToCOBieLiteUK;
using XbimGeometry.Interfaces;


namespace Xbim.Exchange
{
    class Program
    {
       
        static void Main(string[] args)
        {
            
            if (args.Length < 1)
            {
                Console.WriteLine("No Ifc or xBim file specified");
                return;
            }
            var fileName = args[0];
            Console.WriteLine("Reading " + fileName);
           
            using (var model = GetModel(fileName))
            {
                if (model != null)
                {
                   
                    var context = new Xbim3DModelContext(model);
                    context.CreateContext(geomStorageType: XbimGeometryType.PolyhedronBinary);
                    var wexBimFilename = Path.ChangeExtension(fileName, "wexBIM");
                    using (var wexBiMfile = new FileStream(wexBimFilename, FileMode.Create, FileAccess.Write))
                    {
                        using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                        {
                            Console.WriteLine("Creating " + wexBimFilename);
                            context.Write(wexBimBinaryWriter);
                            wexBimBinaryWriter.Close();
                        }
                        wexBiMfile.Close();
                    }
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
                       
                        //facility.ValidateUK2012();
                        dPoWFile = Path.ChangeExtension(dPoWFile, "xbim");
                        Console.WriteLine("Creating " + dPoWFile);
                        using (var ifcModel = XbimModel.CreateModel(dPoWFile))
                        {
                            ifcModel.Initialise("Xbim Tester", "XbimTeam", "Xbim.Exchanger", "Xbim Development Team", "3.0");
                            ifcModel.ReloadModelFactors();
                            using (var txn = ifcModel.BeginTransaction("Convert from COBieLiteUK"))
                            {
                                var coBieLiteUkToIfcExchanger = new CoBieLiteUkToIfcExchanger(facility, ifcModel);
                                coBieLiteUkToIfcExchanger.Convert();
                                txn.Commit();
                                //var err = model.Validate(model.Instances, Console.Out);
                            }
                            dPoWFile = Path.ChangeExtension(dPoWFile, "ifc");
                            Console.WriteLine("Creating " + dPoWFile);
                            ifcModel.SaveAs(dPoWFile, XbimStorageType.IFC);
                            ifcModel.Close();
                        }
                    }
                    model.Close();
                }
            }
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        private static XbimModel GetModel(string fileName)
        {
            XbimModel openModel = null;
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

            if (File.Exists(fileName))
            {
                extension = Path.GetExtension(fileName);
                if (String.Compare(extension, ".xbim", StringComparison.OrdinalIgnoreCase) == 0) //just open xbim
                {

                    try
                    {
                        var model = new XbimModel();
                        Console.WriteLine("Opening " + fileName);
                        model.Open(fileName, XbimDBAccess.ReadWrite);
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
                    var model = new XbimModel();
                    try
                    {
                        Console.WriteLine("Creating " + Path.ChangeExtension(fileName, ".xBIM"));
                        model.CreateFrom(fileName, null, null, true);
                        openModel = model;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to open model {0}, {1}", fileName, e.Message);
                        Console.WriteLine("Unable to open model {0}, {1}", fileName, e.Message);
                    }

                }
            }
            return openModel;
        }
    }
}
