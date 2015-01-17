using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xbim.COBieLite;

namespace Xbim.CobieLite.Validation
{
    public enum ReturnCodes
    {
        Ok = 0,
        ErrorFileNotFound = 1,
        ErrorDesirializingModel = 2
    }


    class Program
    {
        static int Main(string[] args)
        {
            var cobieModelFileName = @"Standard_Classroom_CIC_6_Project_mod2.CobieLight.xml";
           // var cobieDPoWFileName = @"";

            // Model file identification
            //
            if (!File.Exists(cobieModelFileName))
                return (int)ReturnCodes.ErrorFileNotFound;

            // Model Deserialisation 
            //
            FacilityType theFacility = null;
            try
            {
                var x = new XmlSerializer(typeof(FacilityType));
                var reader = new XmlTextReader(cobieModelFileName);
                theFacility = (FacilityType)x.Deserialize(reader);
                reader.Close();
            }
            catch (Exception)
            {
                return (int)ReturnCodes.ErrorDesirializingModel;
            }

            foreach (var assetType in theFacility.AssetTypes.AssetType)
            {
                var fnd = assetType.AssetTypeAttributes.Attribute.Where(attribute => attribute.AttributeName == @"Ciao");
            }

            return (int)ReturnCodes.Ok;
        }
    }
}
