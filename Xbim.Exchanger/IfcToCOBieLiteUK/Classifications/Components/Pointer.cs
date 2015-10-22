using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.Components
{
    /// <summary>
    /// Struct for storing pointer data into the
    /// Dictionary of mappings.
    /// </summary>
    public struct Pointer
    {
        public bool HasMultipleReferences { get; set; }
        public int Classification { get; private set; }
        public List<int> Rows { get; set; }
        public List<int> FileNumbers { get; set; }
        public Pointer(ClassificationSystem classification, int row, int fileNumber)
            : this()
        {
            Rows = new List<int>();
            FileNumbers = new List<int>();

            Classification = (int)classification;
            Rows.Add(row);
            FileNumbers.Add(fileNumber);
        }
    }
}
