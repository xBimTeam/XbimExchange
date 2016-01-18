using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using log4net;
using XbimExchanger.IfcToCOBieExpress.Classifications.Components;
using Assembly = System.Reflection.Assembly;

namespace XbimExchanger.IfcToCOBieExpress.Classifications
{
    internal class ClassificationMappingReader
    {
        private static readonly ILog Log = LogManager.GetLogger("Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.ClassificationMappingReader");

        private readonly Dictionary<string, Pointer> _mappingTable = new Dictionary<string, Pointer>();

        private string[] _classificationMappingFiles;

        private int _uniclassColumn, _nbsColumn, _nrmColumn, _row, _fileNumber, _totalColumns;
        private bool _uniclassColumnIsSet, _nbsColumnIsSet, _nrmColumnIsSet;

        public bool HasFiles
        {
            get { return _classificationMappingFiles != null && _classificationMappingFiles.Any(); }
        }

        public ClassificationMappingReader()
        {
            Init();
        }

        public Pointer GetMatchingPointer(string match)
        {
            Pointer pointer = null;
            _mappingTable.TryGetValue(match, out pointer);
            return pointer;

        }
        private void Init()
        {
            LoadClassificationMappingFiles();
            CheckMappingsForClassifications();
        }

        private void LoadClassificationMappingFiles()
        {
            //Get the executing Assembly
            var asmbly = Assembly.GetExecutingAssembly();

            //Get the Directory path by getting the filepath of the executing dll and removing the DLL files name from the end.
            var dirPath = asmbly.CodeBase.Substring(0, asmbly.CodeBase.IndexOf(asmbly.GetName().Name));

            //The Direcory (in relation to the dirPath) where the resources are stored.
            const string resourceDirectory = "IfcToCOBieLiteUK\\Classifications\\DataFiles";

            //Get the LocalPath of the resource directory
            var uri = (new Uri(dirPath + resourceDirectory).AbsolutePath).Replace("%20", " ");

            var configSection = ConfigurationManager.GetSection("Xbim.Exchanger") as NameValueCollection;
            if (configSection != null)
            {
                var configValue = configSection["ClassificationMappingFolder"];
                if (!string.IsNullOrEmpty(configValue))
                    uri = configValue;
            }
            var diLocal = new DirectoryInfo(".");
            Log.DebugFormat("Trying to load classification mapping files from '{0}' running in '{1}'", uri, diLocal.FullName);
            var d = new DirectoryInfo(uri);
            if (d.Exists)
                _classificationMappingFiles = Directory.GetFiles(uri);
            else
            {
                Log.ErrorFormat("Failed to load classification mapping files from '{0}' running in '{1}'", uri, diLocal.FullName);
            }
        }

        /// <summary>
        /// Reads the Data from 1..* DataFiles and builds a
        /// Dictionary of Classification numbers and a
        /// reference pointer to the file they come from.
        /// </summary>
        private void CheckMappingsForClassifications()
        {
            //Skip if No DataFiles found.
            if (_classificationMappingFiles.Length > 0)
            {
                //Loop through DataFiles
                foreach (var classificationMappingFile in _classificationMappingFiles)
                {
                    FindColumnNumbers(classificationMappingFile);

                    using (var sr = new StreamReader(classificationMappingFile))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        _row = 0;

                        while ((line = sr.ReadLine()) != null)
                        {
                            //Split Columns by comma, ignoring commas inside of fields.
                            var columns = Regex.Split(line.Substring(0, line.Length - 1), RegexPatterns.CsvPattern);

                            //Only look at columns with UNIClass value
                            var match = Regex.Match(columns[_uniclassColumn], RegexPatterns.UniclassPattern);
                            if (match.Value != "" && columns.Length >= 7)
                            {
                                //Check if both NBS and NRM existin DataFile and that current row has enough columns to have them
                                if (_nbsColumnIsSet && _nrmColumnIsSet && columns.Length == _totalColumns)
                                {
                                    //Check if both NBS and NRM exist in DataFile, and have values in this row
                                    if (columns[_nbsColumn] != "" && columns[_nrmColumn] != "")
                                    {
                                        AddConditionalMapping(columns[_uniclassColumn], ClassificationSystem.Uniclass, _row, _fileNumber);
                                        AddConditionalMapping(columns[_nbsColumn], ClassificationSystem.Nbs, _row, _fileNumber);
                                        AddConditionalMapping(columns[_nrmColumn], ClassificationSystem.Nrm, _row, _fileNumber);
                                    }
                                    //Check if both NBS and NRM exist in DataFile, but only NRM has a value
                                    else if (columns[_nbsColumn] != "" && columns[_nrmColumn] == "")
                                    {
                                        AddConditionalMapping(columns[_uniclassColumn], ClassificationSystem.Uniclass, _row, _fileNumber);
                                        AddConditionalMapping(columns[_nbsColumn], ClassificationSystem.Nbs, _row, _fileNumber);
                                    }
                                    //Check if both NBS and NRM exist in DataFile, but only NBS has a value
                                    else if (columns[_nbsColumn] == "" && columns[_nrmColumn] != "")
                                    {
                                        AddConditionalMapping(columns[_uniclassColumn], ClassificationSystem.Uniclass, _row, _fileNumber);
                                        AddConditionalMapping(columns[_nrmColumn], ClassificationSystem.Nrm, _row, _fileNumber);
                                    }
                                }
                                //Check if only NBS exists in DataFile and that current row has enough columns to have NBS
                                else if (_nbsColumnIsSet && !_nrmColumnIsSet && columns.Length == _totalColumns)
                                {
                                    //Check if NBS value is set
                                    if (columns[_nbsColumn] != "")
                                    {
                                        AddConditionalMapping(columns[_uniclassColumn], ClassificationSystem.Uniclass, _row, _fileNumber);
                                        AddConditionalMapping(columns[_nbsColumn], ClassificationSystem.Nbs, _row, _fileNumber);
                                    }
                                }
                                //Check if only NRM exists in DataFile and that current row has enough columns to have NRM
                                else if (!_nbsColumnIsSet && _nrmColumnIsSet && columns.Length == _totalColumns)
                                {
                                    //Check if NRM value is set
                                    if (columns[_nrmColumn] != "")
                                    {
                                        AddConditionalMapping(columns[_uniclassColumn], ClassificationSystem.Uniclass, _row, _fileNumber);
                                        AddConditionalMapping(columns[_nrmColumn], ClassificationSystem.Nrm, _row, _fileNumber);
                                    }
                                }
                            }
                            _row++; // End of Row
                        }
                    }
                    _fileNumber++; // End of File
                }
            }
        }

        internal IEnumerable<InferredClassification> GetInferredMapping(Pointer match)
        {
            var inferredClassifications = new List<InferredClassification>();
            //If the match has a value.
            var lines = new List<string>();
            for (var i = 0; i < match.Rows.Count; i++)
            {
                //Read one line of the file that matches the Pointer's file number and line number.
                lines.Add(File.ReadLines(GetDataFile(match.FileNumbers[i])).Skip(match.Rows[i]).Take(1).First());
            }
            foreach (var line in lines)
            {
                var ic = new InferredClassification();
                //Split the line using the CSV pattern so that columns are split by commas, unless the comma is inside quotes.
                var columns = Regex.Split(line.Substring(0, line.Length - 1), RegexPatterns.CsvPattern);

                //Search for classifications in each column.
                for (var i = 0; i < columns.Length; i++)
                {
                    //Proceed if the column is not empty.
                    if (columns[i] != null)
                    {
                        if (Regex.Match(columns[i], RegexPatterns.UniclassPattern).Success) //If the column contains a Uniclass reference.
                        {
                            ic.UniclassCode = columns[i];
                            ic.UniclassDescription = columns[5]; //Get the Uniclass Description from column 5
                        }
                        else if (Regex.Match(columns[i], RegexPatterns.NbsPattern).Success) //Else - If the column contains an NBS reference.
                        {
                            ic.NbsCode = Regex.Match(columns[i], RegexPatterns.NbsPattern).Value.Trim();
                            ic.NbsDescription = Regex.Split(columns[i], RegexPatterns.NbsPattern)[1].ToString().Trim().TrimEnd(new Char[] { ';' });
                        }
                        else if (Regex.Match(columns[i], RegexPatterns.NrmPattern).Success) //Else - If the column contains an NRM reference.
                        {
                            //Split the NRM column by ';' so to get all associated NRM references.
                            var nrms = columns[i].Split(new Char[] { ';' });
                            //Loop through each NRM reference.
                            foreach (var nrm in nrms)
                            {
                                //Only use values that contain '(default)'.
                                if (nrm.ToLower().Contains("(default)"))
                                {
                                    //Get just the NRM code
                                    ic.NrmCode = Regex.Match(nrm, RegexPatterns.NrmPattern).Value.Trim();

                                    //Get just the NRM description
                                    var str = Regex.Split(nrm, RegexPatterns.NrmPattern)[1].ToString().Trim();
                                    ic.NrmDescription = str.TrimEnd(new Char[] { ';' }).Substring(0, str.IndexOf('(') - 1).Trim();
                                    //IC.NRMDescription = str.Substring(0, str.IndexOf('(') - 1).Trim();
                                }
                            }
                        }
                    }
                }
                inferredClassifications.Add(ic);
            }
            return inferredClassifications;   
    }

        /// <summary>
        /// Finds whether the data contains valid
        /// classifications and stores the column
        /// number of where they exist in the file.
        /// </summary>
        /// <param name="DataFile">The URI of the data file as a string.</param>
        private void FindColumnNumbers(string DataFile)
        {
            using (var sr = new StreamReader(DataFile))
            {
                // Unset the boolean values that say whether a column has bee set.
                UnsetColumns();

                string line; //String line for StreamReader
                //Get Line from StreamReader if the ColumnIsSet values are false
                while ((line = sr.ReadLine()) != null && !(_uniclassColumnIsSet && _nbsColumnIsSet && _nrmColumnIsSet))
                {
                    //Split the line with CSV pattern
                    var columns = Regex.Split(line.Substring(0, line.Length - 1), RegexPatterns.CsvPattern);

                    for (var i = 0; i < columns.Length; i++)
                    {
                        // Set column number if the column is not set and a Uniclass pattern is found.
                        if ((Regex.Match(columns[i], RegexPatterns.UniclassPattern).Value != "") && !_uniclassColumnIsSet)
                        {
                            _uniclassColumn = i;
                            _uniclassColumnIsSet = true;
                        }
                        // Set column number if the column is not set and a NBS pattern is found.
                        if ((Regex.Match(columns[i], RegexPatterns.NbsPattern).Value != "") && !_nbsColumnIsSet)
                        {
                            _nbsColumn = i;
                            _nbsColumnIsSet = true;
                            _totalColumns = columns.Length;
                        }
                        // Set column number if the column is not set and a NRM pattern is found.
                        if ((Regex.Match(columns[i], RegexPatterns.NrmPattern).Value != "") && !_nrmColumnIsSet)
                        {
                            _nrmColumn = i;
                            _nrmColumnIsSet = true;
                            _totalColumns = columns.Length;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This checks to see if the Classification number already
        /// exists as a unique Key in the Dictionary. It also checks
        /// any NRM inputs to see if they contain multiples and only
        /// uses the 'NRM (default)' as a key.
        /// </summary>
        /// <param name="classReference">The Uniclass, NBS or NRM classification number.</param>
        /// <param name="classifier">Enum of type Classifier.</param>
        /// <param name="row">The current row number in the file.</param>
        /// <param name="fileNumber">The array index in DataFile which matches the current file.</param>
        private void AddConditionalMapping(string classReference, ClassificationSystem classifier, int row, int fileNumber)
        {
            // If NRM value
            if (classifier == ClassificationSystem.Nrm)
            {
                //Split the NRM column by ';' so to get all associated NRM references.
                var nrms = classReference.Split(new Char[] { ';' });
                //Loop through each NRM reference.
                foreach (var nrm in nrms)
                {
                    //Only use values that contain '(default)'.
                    if (nrm.ToLower().Contains("(default)"))
                    {
                        //classReference = Regex.Match(NRM, NRMPattern).Value;
                        //If the Mapping table does not contain this NRM value set classReference to the processed NRM value.
                        if (!_mappingTable.Keys.Contains(nrm.Trim()))
                        {
                            classReference = Regex.Match(nrm, RegexPatterns.NrmPattern).Value;
                        }
                    }
                }
            }

            // If NBS value
            else if (classifier == ClassificationSystem.Nbs)
            {
                // Set classReference to just the NBS code taken from the column
                classReference = Regex.Match(classReference, RegexPatterns.NbsPattern).Value;
            }
            //Add the references to the mapping table if they don't already exist.
            if (!_mappingTable.Keys.Contains(classReference))
            {
                //MappingTable.Add(classReference, new Pointer(classifier, row, fileNumber));
                _mappingTable.Add(classReference, new Pointer(classifier, row, fileNumber));
            }
            else
            {
                var map = _mappingTable[classReference];
                map.Rows.Add(row);
                map.FileNumbers.Add(fileNumber);               
            }
        }

        /// <summary>
        /// Resets the ColumnIsSet booleans
        /// at the start of reading a new file.
        /// </summary>
        private void UnsetColumns()
        {
            _uniclassColumnIsSet = false;
            _nbsColumnIsSet = false;
            _nrmColumnIsSet = false;
        }

        /// <summary>
        /// Returns the string URL of the file
        /// located at a 0 based index in the 
        /// array of DataFiles
        /// </summary>
        /// <param name="fileNumber">0 based integer pointer</param>
        /// <returns>URL of the data file as a string</returns>
        public string GetDataFile(int fileNumber)
        {
            return _classificationMappingFiles[fileNumber];
        }
    }
}