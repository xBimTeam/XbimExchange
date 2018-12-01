using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Xbim.CobieExpress;
using Xbim.CobieLiteUk;
using Xbim.CobieLiteUk.Validation;
using Xbim.CobieLiteUk.Validation.Reporting;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.IO;
using Xbim.IO.Memory;
using Xbim.IO.Xml;
using Xbim.ModelGeometry.Scene;
using XbimExchanger.COBieLiteUkToIfc;
using XbimExchanger.IfcToCOBieExpress;
using XbimExchanger.IfcToCOBieLiteUK;
using Xbim.Ifc2x3;

namespace Xbim.Exchange
{
    internal class Program
    {
        static ILogger logger;

        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            SetupXbimLogging(serviceProvider);

            var settings = new ExchangeSettings();
            
            var someAction = false;
            foreach (var arg in args)
            { 
            
                if (!settings.IsOption(arg))
                {
                    ProcessFile(arg, settings);
                    someAction = true;
                }
            }
            if (settings.DisplayVersion)
            {
                someAction = true;
            }

            if (!someAction)
            {
                Console.WriteLine("No Ifc or xBim file specified");
                Console.WriteLine("Usage: Xbim.Exchange.exe [/out:<OutputFolder>] [/req:<DPoWRequirementFile>] <IfcFileName>");
                Console.WriteLine("\t<IfcFileName> non-optional, full or relative path to the ifc model to process.");
                Console.WriteLine("\t<DPoWRequirementFile> full or relative path to the Json DPoW requirement file for a single stage.");
                Console.WriteLine("\t<OutputFolder> full path of the output folder where to write the files.");
            }

            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        private static void ProcessFile(string fileName, ExchangeSettings settings)
        {
            var outDirectoryName = Path.GetDirectoryName(fileName);
            if (settings.OutputdDirectory != null)
                outDirectoryName = settings.OutputdDirectory.FullName;
            if (outDirectoryName == null)
                return;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            var w = new Stopwatch();
            w.Start();
            Console.WriteLine("Reading " + fileName);
            using (var model = GetModel(fileName))
            {
                if (model == null)
                {
                    Console.WriteLine("No model to process. Press any key to exit");
                    Console.Read();
                    return;
                }
                Console.WriteLine("Model open in {0}ms", w.ElapsedMilliseconds);

                // wexbim
                w.Restart();
                var wexBimFilename = GetSaveName(outDirectoryName, fileNameWithoutExtension, ".wexBIM");
                Console.WriteLine("Creating " + wexBimFilename);
                var context = new Xbim3DModelContext(model);
                context.CreateContext();               
                using (var wexBiMfile = new FileStream(wexBimFilename, FileMode.Create, FileAccess.Write))
                {
                    using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                    {
                        model.SaveAsWexBim(wexBimBinaryWriter);
                        wexBimBinaryWriter.Close();
                    }
                    wexBiMfile.Close();
                }
                Console.WriteLine("wexBIM model created in {0}ms", w.ElapsedMilliseconds);

                //now do COBieExpress
                //
                w.Restart();
                Console.WriteLine("Creating CobieExpress memoryModel...");
                var cobie = new MemoryModel(new EntityFactoryCobieExpress());
                var cobieExpressFile = GetSaveName(outDirectoryName, fileNameWithoutExtension, ".cobie");
                var cobieExpressXmlFile = GetSaveName(outDirectoryName, fileNameWithoutExtension, ".cobieXml");
                var cobieExpressZipFile = GetSaveName(outDirectoryName, fileNameWithoutExtension, ".cobieZip");
                using (var txn = cobie.BeginTransaction("IFC data in"))
                {
                    var exchanger = new IfcToCoBieExpressExchanger(model, cobie);
                    exchanger.Convert();
                    txn.Commit();
                }

                Console.WriteLine("COBieExpress memoryModel created and commited in {0}ms...", w.ElapsedMilliseconds);
                w.Restart();
                cobie.SaveAsStep21(File.Create(cobieExpressFile));
                cobie.SaveAsStep21Zip(File.Create(cobieExpressZipFile));
                cobie.SaveAsXml(File.Create(cobieExpressXmlFile), new XmlWriterSettings {Indent = true, IndentChars = "\t"},
                     XbimXmlSettings.IFC4Add2);
                Console.WriteLine("3 COBieExpress files (.cobie., cobieXml and .cobieZip) saved in {0}ms",
                    w.ElapsedMilliseconds);

                //now do the DPoW files
                //
                w.Restart();
                Console.WriteLine("Creating CobieLiteUK Model...");
                
                
                var facilities = new List<Facility>();
                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(model, facilities);
                facilities = ifcToCoBieLiteUkExchanger.Convert();

                Console.WriteLine("{1} facilities converted in in {0}ms",
                    w.ElapsedMilliseconds,
                    facilities.Count
                    );

                var multipleFacilities = facilities.Count > 1;

                for (var index = 0; index < facilities.Count; index++)
                {
                    w.Restart();
                    var facility = facilities[index];
                    var dpowNameExtension = "DPoW";
                    if (multipleFacilities)
                        dpowNameExtension += index + 1;
                    // write json
                    
                    var dPoWFile = GetSaveName(outDirectoryName, fileNameWithoutExtension + "_" + dpowNameExtension, ".json");
                    Console.Write("Creating " + dPoWFile + "...");
                    facility.WriteJson(dPoWFile);
                    Console.WriteLine(" completed in {0}ms", w.ElapsedMilliseconds);

                    // xlsx
                    var cobieFile = Path.ChangeExtension(dPoWFile, "xlsx");
                    Console.WriteLine("Creating " + cobieFile + "...");
                    string error;
                    facility.WriteCobie(cobieFile, out error);
                    Console.WriteLine(" completed in {0}ms", w.ElapsedMilliseconds);
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("COBie Errors: " + error);
                        Console.ResetColor();
                    }

                    // dpow validation
                    //
                    if (!string.IsNullOrEmpty(settings.DpowRequirementFile))
                    {
                        w.Restart();
                        Console.Write("Reading DPoW requirement file: " + settings.DpowRequirementFile + "...");
                        var req = Facility.ReadJson(settings.DpowRequirementFile);
                        Console.WriteLine(" completed in {0}ms", w.ElapsedMilliseconds);
                        
                        w.Restart();
                        Console.Write("Validating DPOW...");
                        var validator = new FacilityValidator();
                        var result = validator.Validate(req, facility);
                        Console.WriteLine(" completed in {0}ms", w.ElapsedMilliseconds);

                        w.Restart();
                        var validationResults = Path.ChangeExtension(dPoWFile, "validationReport.xlsx");
                        Console.Write("writing validation report: " + validationResults);
                        //create report
                        using (var stream = File.Create(validationResults))
                        {
                            var report = new ExcelValidationReport();
                            report.Create(result, stream, ExcelValidationReport.SpreadSheetFormat.Xlsx);
                            stream.Close();
                        }
                        Console.WriteLine(" completed in {0}ms", w.ElapsedMilliseconds);
                    }

                    // now attempt basic content validation and recovery
                    //
                    w.Restart();
                    Console.WriteLine("Validating and recovering...");
                    facility.ValidateUK2012(Console.Out, true);
                    var cobieValidatedFile = Path.ChangeExtension(dPoWFile, "ValidationWithAttemptedRecovery.xlsx");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    facility.WriteCobie(cobieValidatedFile, out error);
                    Console.ResetColor();
                    Console.WriteLine("Completed in {0}ms", w.ElapsedMilliseconds);

                    // now ifc
                    // 
                    w.Restart();
                    dPoWFile = Path.ChangeExtension(dPoWFile, "ifc");
                    Console.Write("Creating " + dPoWFile + "...");
                    var credentials = new XbimEditorCredentials()
                    {
                        ApplicationDevelopersName = "XbimTeam",
                        ApplicationFullName = "Xbim.Exchanger",
                        EditorsOrganisationName = "Xbim Development Team",
                        EditorsFamilyName = "Xbim Tester",
                        ApplicationVersion = global::System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()
                    };
                    using (var ifcModel = IfcStore.Create(credentials, XbimSchemaVersion.Ifc2X3, XbimStoreType.EsentDatabase))
                    {
                        using (var txn = ifcModel.BeginTransaction("Convert from COBieLiteUK"))
                        {
                            var coBieLiteUkToIIfcExchanger = new CoBieLiteUkToIfcExchanger(facility, ifcModel);
                            coBieLiteUkToIIfcExchanger.Convert();
                            txn.Commit();
                            //var err = model.Validate(model.Instances, Console.Out);
                        }
                        ifcModel.SaveAs(dPoWFile, StorageType.Ifc);
                        ifcModel.Close();
                    }
                    Console.WriteLine(" completed in {0}ms", w.ElapsedMilliseconds);
                }
                model.Close();
            }
        }

        private static string GetSaveName(string outDirectoryName, string fileNameWithoutExtension, string extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;
            return Path.Combine(outDirectoryName, fileNameWithoutExtension + extension);
        }

        // mockup of what a .net core DI container
        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(conf => {
                conf.SetMinimumLevel(LogLevel.Debug);   // Set the minimum log level
                // Might also consider adding Serilog.Extensions.Logging.File to log to a rolling file.
                // loggerFactory.AddFile("Logs/Exchanger-{Date}.log")
                conf.AddConsole();
            });

            serviceCollection.AddSingleton<LoggerFactory>();
            return serviceCollection.BuildServiceProvider();
        }

        private static void SetupXbimLogging(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            XbimLogging.LoggerFactory = loggerFactory;

            logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Logging set up");
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

            if (!File.Exists(fileName))
                return null;

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