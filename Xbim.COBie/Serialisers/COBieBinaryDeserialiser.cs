using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Contracts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Xbim.COBie.Serialisers
{
    public class COBieBinaryDeserialiser : ICOBieDeserialiser
    {
        private string _file;

        public COBieBinaryDeserialiser(string file)
        {
            _file = file;
        }
        

        public COBieWorkbook Deserialise()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            COBieWorkbook workBook = null;
            using (Stream stream = new FileStream(_file, FileMode.Open, FileAccess.Read, FileShare.None))
            { 
                workBook = (COBieWorkbook) formatter.Deserialize(stream);
            }
            workBook.CreateIndices();
            return workBook;
        }
    }
}
