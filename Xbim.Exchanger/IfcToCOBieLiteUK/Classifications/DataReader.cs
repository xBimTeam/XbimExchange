using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.Components;
using Assembly = System.Reflection.Assembly;

namespace Xbim.Exchanger.IfcToCOBieLiteUK.Classifications
{
    class DataReader
    {
        private Dictionary<string, Pointer> _mappingTable = new Dictionary<string, Pointer>();

        private string[] _dataFiles;

        int UNIColumn, NBSColumn, NRMColumn, Row, FileNumber;
        bool UNIColumnIsSet, NBSColumnIsSet, NRMColumnIsSet;
        int totalColumns;

        public Pointer GetMatchingPointer(string match)
        {
            Pointer pointer = null;
            _mappingTable.TryGetValue(match, out pointer);
            return pointer;

        }
        private void Init()
        {
            GetDataFiles();
            CheckDataFilesForClassifications();
        }

        private void GetDataFiles()
        {
            //Get the executing Assembly
            Assembly asmbly = Assembly.GetExecutingAssembly();

            //Get the Directory path by getting the filepath of the executing dll and removing the DLL files name from the end.
            string dirPath = asmbly.CodeBase.Substring(0, asmbly.CodeBase.IndexOf(asmbly.GetName().Name.ToString()));

            //The Direcory (in relation to the dirPath) where the resources are stored.
            string resourceDirectory = "IfcToCOBieLiteUK\\Classifications\\DataFiles";

            //Get the LocalPath of the resource directory
            string uri = (new Uri(dirPath + resourceDirectory).AbsolutePath).Replace("%20", " ");

            //Get the data files from the resource directory.
            _dataFiles = Directory.GetFiles(uri);

            //DataFiles = Directory.GetFiles(@"IfcToCOBieLiteUK\Classifications\DataFiles\");
        }

        /// <summary>
        /// Reads the Data from 1..* DataFiles and builds a
        /// Dictionary of Classification numbers and a
        /// reference pointer to the file they come from.
        /// </summary>
        private void CheckDataFilesForClassifications()
        {
            //Skip if No DataFiles found.
            if (_dataFiles.Length > 0)
            {
                //Loop through DataFiles
                foreach (var DataFile in _dataFiles)
                {
                    FindColumnNumbers(DataFile);

                    using (var sr = new StreamReader(DataFile))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        Row = 0;

                        while ((line = sr.ReadLine()) != null)
                        {
                            //Split Columns by comma, ignoring commas inside of fields.
                            string[] columns = Regex.Split(line.Substring(0, line.Length - 1), RegexPatterns.CSVpattern);

                            //Only look at columns with UNIClass value
                            var match = Regex.Match(columns[UNIColumn], RegexPatterns.UNIPattern);
                            if (match.Value != "" && columns.Length >= 7)
                            {
                                //Check if both NBS and NRM existin DataFile and that current row has enough columns to have them
                                if (NBSColumnIsSet && NRMColumnIsSet && columns.Length == totalColumns)
                                {
                                    //Check if both NBS and NRM exist in DataFile, and have values in this row
                                    if (columns[NBSColumn] != "" && columns[NRMColumn] != "")
                                    {
                                        AddConditionalMapping(columns[UNIColumn], ClassificationSystem.UNI, Row, FileNumber);
                                        AddConditionalMapping(columns[NBSColumn], ClassificationSystem.NBS, Row, FileNumber);
                                        AddConditionalMapping(columns[NRMColumn], ClassificationSystem.NRM, Row, FileNumber);
                                    }
                                    //Check if both NBS and NRM exist in DataFile, but only NRM has a value
                                    else if (columns[NBSColumn] != "" && columns[NRMColumn] == "")
                                    {
                                        AddConditionalMapping(columns[UNIColumn], ClassificationSystem.UNI, Row, FileNumber);
                                        AddConditionalMapping(columns[NBSColumn], ClassificationSystem.NBS, Row, FileNumber);
                                    }
                                    //Check if both NBS and NRM exist in DataFile, but only NBS has a value
                                    else if (columns[NBSColumn] == "" && columns[NRMColumn] != "")
                                    {
                                        AddConditionalMapping(columns[UNIColumn], ClassificationSystem.UNI, Row, FileNumber);
                                        AddConditionalMapping(columns[NRMColumn], ClassificationSystem.NRM, Row, FileNumber);
                                    }
                                }
                                //Check if only NBS exists in DataFile and that current row has enough columns to have NBS
                                else if (NBSColumnIsSet && !NRMColumnIsSet && columns.Length == totalColumns)
                                {
                                    //Check if NBS value is set
                                    if (columns[NBSColumn] != "")
                                    {
                                        AddConditionalMapping(columns[UNIColumn], ClassificationSystem.UNI, Row, FileNumber);
                                        AddConditionalMapping(columns[NBSColumn], ClassificationSystem.NBS, Row, FileNumber);
                                    }
                                }
                                //Check if only NRM exists in DataFile and that current row has enough columns to have NRM
                                else if (!NBSColumnIsSet && NRMColumnIsSet && columns.Length == totalColumns)
                                {
                                    //Check if NRM value is set
                                    if (columns[NRMColumn] != "")
                                    {
                                        AddConditionalMapping(columns[UNIColumn], ClassificationSystem.UNI, Row, FileNumber);
                                        AddConditionalMapping(columns[NRMColumn], ClassificationSystem.NRM, Row, FileNumber);
                                    }
                                }
                            }
                            Row++; // End of Row
                        }
                    }
                    FileNumber++; // End of File
                }
            }
        }

        internal IEnumerable<InferredClassification> GetInferredMapping(Pointer match)
        {
            var ICs = new List<InferredClassification>();
            //If the match has a value.
            var lines = new List<string>();
            for (int i = 0; i < match.Rows.Count; i++)
            {
                //Read one line of the file that matches the Pointer's file number and line number.
                lines.Add(File.ReadLines(GetDataFile(match.FileNumbers[i])).Skip(match.Rows[i]).Take(1).First());
            }
            foreach (string line in lines)
            {
                var IC = new InferredClassification();
                //Split the line using the CSV pattern so that columns are split by commas, unless the comma is inside quotes.
                string[] columns = Regex.Split(line.Substring(0, line.Length - 1), RegexPatterns.CSVpattern);

                //Search for classifications in each column.
                for (int i = 0; i < columns.Length; i++)
                {
                    //Proceed if the column is not empty.
                    if (columns[i] != null)
                    {
                        if (Regex.Match(columns[i], RegexPatterns.UNIPattern).Success) //If the column contains a Uniclass reference.
                        {
                            IC.UNICode = columns[i];
                            IC.UNIDescription = columns[5]; //Get the Uniclass Description from column 5
                        }
                        else if (Regex.Match(columns[i], RegexPatterns.NBSPattern).Success) //Else - If the column contains an NBS reference.
                        {
                            IC.NBSCode = Regex.Match(columns[i], RegexPatterns.NBSPattern).Value.Trim();
                            IC.NBSDescription = Regex.Split(columns[i], RegexPatterns.NBSPattern)[1].ToString().Trim().TrimEnd(new Char[] { ';' });
                        }
                        else if (Regex.Match(columns[i], RegexPatterns.NRMPattern).Success) //Else - If the column contains an NRM reference.
                        {
                            //Split the NRM column by ';' so to get all associated NRM references.
                            var NRMs = columns[i].Split(new Char[] { ';' });
                            //Loop through each NRM reference.
                            foreach (var NRM in NRMs)
                            {
                                //Only use values that contain '(default)'.
                                if (NRM.ToLower().Contains("(default)"))
                                {
                                    //Get just the NRM code
                                    IC.NRMCode = Regex.Match(NRM, RegexPatterns.NRMPattern).Value.Trim();

                                    //Get just the NRM description
                                    string str = Regex.Split(NRM, RegexPatterns.NRMPattern)[1].ToString().Trim();
                                    IC.NRMDescription = str.TrimEnd(new Char[] { ';' }).Substring(0, str.IndexOf('(') - 1).Trim();
                                    //IC.NRMDescription = str.Substring(0, str.IndexOf('(') - 1).Trim();
                                }
                            }
                        }
                    }
                }
                ICs.Add(IC);
            }
            return ICs;   
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
                while ((line = sr.ReadLine()) != null && !(UNIColumnIsSet && NBSColumnIsSet && NRMColumnIsSet))
                {
                    //Split the line with CSV pattern
                    string[] columns = Regex.Split(line.Substring(0, line.Length - 1), RegexPatterns.CSVpattern);

                    for (int i = 0; i < columns.Length; i++)
                    {
                        // Set column number if the column is not set and a Uniclass pattern is found.
                        if ((Regex.Match(columns[i], RegexPatterns.UNIPattern).Value != "") && !UNIColumnIsSet)
                        {
                            UNIColumn = i;
                            UNIColumnIsSet = true;
                        }
                        // Set column number if the column is not set and a NBS pattern is found.
                        if ((Regex.Match(columns[i], RegexPatterns.NBSPattern).Value != "") && !NBSColumnIsSet)
                        {
                            NBSColumn = i;
                            NBSColumnIsSet = true;
                            totalColumns = columns.Length;
                        }
                        // Set column number if the column is not set and a NRM pattern is found.
                        if ((Regex.Match(columns[i], RegexPatterns.NRMPattern).Value != "") && !NRMColumnIsSet)
                        {
                            NRMColumn = i;
                            NRMColumnIsSet = true;
                            totalColumns = columns.Length;
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
            if (classifier == ClassificationSystem.NRM)
            {
                //Split the NRM column by ';' so to get all associated NRM references.
                var NRMs = classReference.Split(new Char[] { ';' });
                //Loop through each NRM reference.
                foreach (var NRM in NRMs)
                {
                    //Only use values that contain '(default)'.
                    if (NRM.ToLower().Contains("(default)"))
                    {
                        //classReference = Regex.Match(NRM, NRMPattern).Value;
                        //If the Mapping table does not contain this NRM value set classReference to the processed NRM value.
                        if (!_mappingTable.Keys.Contains(NRM.Trim()))
                        {
                            classReference = Regex.Match(NRM, RegexPatterns.NRMPattern).Value;
                        }
                    }
                }
            }

            // If NBS value
            else if (classifier == ClassificationSystem.NBS)
            {
                // Set classReference to just the NBS code taken from the column
                classReference = Regex.Match(classReference, RegexPatterns.NBSPattern).Value;
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
            UNIColumnIsSet = false;
            NBSColumnIsSet = false;
            NRMColumnIsSet = false;
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
            return _dataFiles[fileNumber];
        }
    }
}