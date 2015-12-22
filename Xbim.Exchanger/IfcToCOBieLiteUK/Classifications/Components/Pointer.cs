using System.Collections.Generic;

namespace Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.Components
{
    /// <summary>
    /// Struct for storing pointer data into the
    /// Dictionary of mappings.
    /// </summary>
    public class Pointer
    {
        private ClassificationSystem _classification;
        private List<int> _rows;
        private List<int> _fileNumbers;
        public bool HasMultipleReferences { get { return _rows.Count > 1; } }
        public ClassificationSystem Classification { get { return _classification; } }

        public IList<int> Rows { get { return _rows; } }
        public IList<int> FileNumbers { get { return _fileNumbers; } }
        public Pointer(ClassificationSystem classification, int row, int fileNumber)
        {
            _rows = new List<int>();
            _rows.Add(row);
            _fileNumbers = new List<int>(); 
            _fileNumbers.Add(fileNumber);
            _classification = classification;
        }
    }
}
