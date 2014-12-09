using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Contracts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Xbim.COBie.Serialisers
{
    public class COBieBinarySerialiser : ICOBieSerialiser
    {
        private string _file;

        public COBieBinarySerialiser(string file)
        {
            _file = file;

        }

        public void Serialise(COBieWorkbook workbook, ICOBieValidationTemplate ValidationTemplate = null)
        {
            if (workbook == null)
            {
                throw new ArgumentNullException("workbook", "Xbim");
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (Stream stream = new FileStream(_file, FileMode.Create, FileAccess.Write, FileShare.None))
            { 
                formatter.Serialize(stream, workbook);
            }

        }
    }
}
