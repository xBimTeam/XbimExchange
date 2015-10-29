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
        public Dictionary<string, Pointer> MappingTable { get; private set; }

        public string[] DataFiles;

        int UNIColumn, NBSColumn, NRMColumn, Row, FileNumber;
        bool UNIColumnIsSet, NBSColumnIsSet, NRMColumnIsSet;
        int totalColumns;

        /// <summary>
        /// Constructor for DataReader Class
        /// </summary>
        /// <param name="dataFiles">Takes an array of DataFile URIs as strings</param>
        public DataReader()
        {
            GetDataFiles();

            MappingTable = new Dictionary<string, Pointer>();
            CheckDataFilesForClassifications();
        }

        public void GetDataFiles()
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
            DataFiles = Directory.GetFiles(uri);

            //DataFiles = Directory.GetFiles(@"IfcToCOBieLiteUK\Classifications\DataFiles\");
        }

        /// <summary>
        /// Reads the Data from 1..* DataFiles and builds a
        /// Dictionary of Classification numbers and a
        /// reference pointer to the file they come from.
        /// </summary>
        public void CheckDataFilesForClassifications()
        {
            //Skip if No DataFiles found.
            if (DataFiles.Length > 0)
            {
                //Loop through DataFiles
                foreach (var DataFile in DataFiles)
                {
                    FindColumnNumbers(DataFile);

                    using (StreamReader sr = new StreamReader(DataFile))
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

        /// <summary>
        /// Finds whether the data contains valid
        /// classifications and stores the column
        /// number of where they exist in the file.
        /// </summary>
        /// <param name="DataFile">The URI of the data file as a string.</param>
        private void FindColumnNumbers(string DataFile)
        {
            using (StreamReader sr = new StreamReader(DataFile))
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
                        if (!MappingTable.Keys.Contains(NRM.Trim()))
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
            if (!MappingTable.Keys.Contains(classReference))
            {
                //MappingTable.Add(classReference, new Pointer(classifier, row, fileNumber));
                MappingTable.Add(classReference, new Pointer(classifier, row, fileNumber));
            }
            else
            {
                var map = MappingTable[classReference];
                map.Rows.Add(row);
                map.FileNumbers.Add(fileNumber);
                map.HasMultipleReferences = true;
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
            return DataFiles[fileNumber];
        }
    }
}