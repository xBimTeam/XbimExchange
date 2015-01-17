using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xbim.COBieLite;
using Xbim.DPoW.Interfaces;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;

namespace Tests.TestFiles
{
    [TestClass]
    [DeploymentItem(@"TestFiles\")]
    public class DpowInterfaceTests
    {
               
        /// <summary>
        /// Creates all the output files used by the DPoW toolkit
        /// </summary>
         [TestMethod]
        public void WriteDPoWOutputFiles()
        {

            const string ifcFileFullName = "Duplex_MEP_20110907.ifc";
            var fileName = Path.GetFileName(ifcFileFullName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var workingDir = Directory.GetCurrentDirectory();
            var coreFileName = Path.Combine(workingDir, fileNameWithoutExtension);
            var wexBimFileName = Path.ChangeExtension(coreFileName, "wexbim");
            var cobieFileName = Path.ChangeExtension(coreFileName, "json");
            var xbimFile = Path.ChangeExtension(coreFileName, "xbim");
            try
            {

                using (var wexBimFile = new FileStream(wexBimFileName, FileMode.Create))
                {
                    using (var binaryWriter = new BinaryWriter(wexBimFile))
                    {

                        using (var model = new XbimModel())
                        {
                            model.CreateFrom(ifcFileFullName, xbimFile, null, true, false);
                            var geomContext = new Xbim3DModelContext(model);
                            geomContext.CreateContext();
                            geomContext.Write(binaryWriter);
                            var helper = new CoBieLiteHelper(model, "UniClass");
                            var facilities = helper.GetFacilities();
                            int i = 0;
                            foreach (var facilityType in facilities) //we may have more than one
                            {
                                using (var cobieFile = new FileStream(cobieFileName, FileMode.Create))
                                {
                                    using (var textWriter = new StreamWriter(cobieFile))
                                    {
                                        CoBieLiteHelper.WriteJson(textWriter, facilityType);
                                    }
                                }
                                i++;
                                cobieFileName = Path.Combine(Path.GetDirectoryName(cobieFileName),
                                    Path.GetFileNameWithoutExtension(cobieFileName) + i,
                                    Path.GetExtension(cobieFileName));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to process " + ifcFileFullName + " - " + e.Message);
            }
        }
    



        [TestMethod]
        public void DeserialiseJsonTest()
        {
            using (StreamReader file = File.OpenText("dpowData.json"))
            {
                var serializer = new JsonSerializer();
                var dpow = (PlanOfWork)serializer.Deserialize(file, typeof(PlanOfWork));
            }
        }
    }
}
