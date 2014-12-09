using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie;
using Xbim.COBie.Rows;
using System.IO;
using Xbim.COBie.Contracts;

namespace Xbim.COBie.Serialisers
{
    public class COBieJSONSerialiser : ICOBieSerialiser
    {
        const string DEFAULT_FILENAME = "COBie.json";
        const string DEFAULT_TEMPLATE_NAME = @"Templates\COBie-UK-2012-template.xls";

        public COBieJSONSerialiser() : this (DEFAULT_FILENAME, DEFAULT_TEMPLATE_NAME)
        { }

        public COBieJSONSerialiser(string filename) : this (filename, DEFAULT_TEMPLATE_NAME)
        { }

        public COBieJSONSerialiser(string filename, string templateName)
        {
            FileName = filename;
            TemplateFileName = templateName;
        }

        public string FileName { get; set; }
        public string TemplateFileName { get; set; }

        public void Serialise(COBieWorkbook workbook, ICOBieValidationTemplate ValidationTemplate = null)
        {
            if (workbook == null) 
            {
                throw new ArgumentNullException("workbook", "Xbim.COBie.Serialisers.COBieJSONSerialiser.Serialise(COBieWorkbook) does not accept null as the workbook parameter");
            }

            FileName += ".json";

            using (FileStream stream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                StreamWriter writer = new StreamWriter(stream);

                // Serialise Workbook to JSON and write to file stream
                new Newtonsoft.Json.JsonSerializer().Serialize(writer, workbook);

                writer.Flush();
                writer.Close();
            }
        }
    }
}
