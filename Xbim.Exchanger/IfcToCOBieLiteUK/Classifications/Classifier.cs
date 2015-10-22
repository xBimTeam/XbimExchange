using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xbim.IO;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Kernel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xbim.COBieLiteUK;
using XbimExchanger.IfcToCOBieLiteUK;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.Components;

namespace Xbim.Exchanger.IfcToCOBieLiteUK.Classifications
{
    public class Classifier
    {
        Dictionary<string, Pointer> Mappings = new Dictionary<string, Pointer>(); //The Mapping table where pointers will be stored
        private string IfcFile;
        public Facility facility { get; private set; }

        DataReader DataReader; //DataReader Object which will create and populate the mappings table.

        /// <summary>
        /// This is the constructor for the Classifier Class
        /// </summary>
        public Classifier(Facility facility)
        {
            this.facility = facility;
            DataReader = new DataReader();
            Mappings = DataReader.MappingTable; //Populate the Mappings Dictionary
            AddClassificationsToAssets();
        }

        /// <summary>
        /// This method searches through the facilities to look
        /// for properties inside the property sets of Assets
        /// to look for values that match Regex classification 
        /// formats, set out in the DataReader. It then adds any
        /// values that match the Regular Expression as a category
        /// of assets which conforms with the Schema.
        /// </summary>
        private void AddClassificationsToAssets()
        {
            Console.WriteLine("\n* Searching for Property Sets that match classification patterns...");
            var time = DateTime.Now; //Used for timing the operation.

            ////Get Each FacilityType
            //foreach (Facility facilityType in facilities)
            //{
                //Get Each AssetType
                foreach (AssetType at in facility.AssetTypes)
                {
                    //Get Each Asset
                    foreach (Asset a in at.Assets)
                    {
                        //Create a new Category for the Asset
                        a.Categories = new List<Category>();

                        //Get Each Property
                        foreach (Xbim.COBieLiteUK.Attribute Atr in a.Attributes)
                        {
                            if (Atr.Value.GetStringValue() != null && a.Categories.Count == 0)
                            {
                                //Get the Inferred Classifications
                                List<InferredClassification> ICs = FindInferredClassifications(Atr.Value.GetStringValue());

                                foreach (InferredClassification IC in ICs)
                                {
                                    bool UNIMatch = false;
                                    bool NBSMatch = false;
                                    bool NRMMatch = false;

                                    foreach (var cat in a.Categories)
                                    {
                                        if (IC.UNICode != null && cat.Code == IC.UNICode)
                                        {
                                            UNIMatch = true;
                                        }
                                        if (IC.NBSCode != null && cat.Code == IC.NBSCode)
                                        {
                                            NBSMatch = true;
                                        }
                                        if (IC.NRMCode != null && cat.Code == IC.NRMCode)
                                        {
                                            NRMMatch = true;
                                        }
                                    }
                                    //Add the Classifications as categories if they exist.
                                    if (IC.UNICode != null && !UNIMatch)
                                    {
                                        Xbim.COBieLiteUK.Category UNI = new Xbim.COBieLiteUK.Category();
                                        UNI.Classification = "Uniclass 2015 Reference (Inferred)";
                                        UNI.Code = IC.UNICode;
                                        UNI.Description = IC.UNIDescription;
                                        a.Categories.Add(UNI);
                                    }
                                    if (IC.NBSCode != null && !NBSMatch)
                                    {
                                        Xbim.COBieLiteUK.Category NBS = new Xbim.COBieLiteUK.Category();
                                        NBS.Classification = "NBS Reference (Inferred)";
                                        NBS.Code = IC.NBSCode;
                                        NBS.Description = IC.NBSDescription;
                                        a.Categories.Add(NBS);
                                    }
                                    if (IC.NRMCode != null && !NRMMatch)
                                    {
                                        Xbim.COBieLiteUK.Category NRM = new Xbim.COBieLiteUK.Category();
                                        NRM.Classification = "NRM Reference (Inferred)";
                                        NRM.Code = IC.NRMCode;
                                        NRM.Description = IC.NRMDescription;
                                        a.Categories.Add(NRM);
                                    }
                                }

                            }
                        }
                    }
                }
            //}
            //Console.WriteLine("\n* Operation Took " + (DateTime.Now - time).TotalSeconds + "s");
        }

        /// <summary>
        /// Checks the property passed in as a parement to see
        /// if the property value matches the format of either
        /// Uniclass, NBS or NRM.
        /// </summary>
        /// <param name="property">A string value of the assets property</param>
        /// <returns>An InferredClassification which contains the classification mappings</returns>
        public List<InferredClassification> FindInferredClassifications(string property)
        {
            //Create a new InfferedClassification
            InferredClassification IC = new InferredClassification();
            List<InferredClassification> ICs = new List<InferredClassification>();


            //Check to see if the property is a valid classification
            var UNIMatches = Regex.Match(property, RegexPatterns.UNIPattern);
            var NBSMatches = Regex.Match(property, RegexPatterns.NBSPattern);
            var NRMMatches = Regex.Match(property, RegexPatterns.NRMPattern);

            IEnumerable<KeyValuePair<string, Pointer>> matches = null;

            //Get Key-Value pairs of matches if the regex was successful.
            if (UNIMatches.Success)
            {
                matches = GetMatches(UNIMatches.Value); //Get Uniclass matches from the Mappings Table
            }
            else if (NBSMatches.Success)
            {
                matches = GetMatches(NBSMatches.Value); //Get NBS matches from the Mappings Table
            }
            else if (NRMMatches.Success)
            {
                matches = GetMatches(NRMMatches.Value); //Get NRM matches from the Mappings Table
            }

            //Get Mappings that match the InferredClassification
            if (matches != null)
            {
                ICs = GetInferredMapping(matches, ICs);
            }
            else
            {
                ICs.Add(IC); // Add the empty InferredClassification
            }
            return ICs;
        }

        /// <summary>
        /// Get Mapped classifications that are associated to
        /// the matches provided as a parameter.
        /// </summary>
        /// <param name="matches">Key Value (String, Pointer) Enumerable list of references that match the asset's property</param>
        /// <param name="IC">The InferredClassification struct where the classification data is to be stored.</param>
        /// <returns>IC (After mapped classification data is added (if it exists).</returns>
        public List<InferredClassification> GetInferredMapping(IEnumerable<KeyValuePair<string, Pointer>> matches, List<InferredClassification> ICs)
        {
            //Check each match
            foreach (var match in matches)
            {
                //If the match has a value.
                if (match.Key != "" || match.Key != null)
                {
                    List<string> lines = new List<string>();

                    for (int i = 0; i < match.Value.Rows.Count; i++)
                    {
                        //Read one line of the file that matches the Pointer's file number and line number.
                        lines.Add(File.ReadLines(DataReader.GetDataFile(match.Value.FileNumbers[i])).Skip(match.Value.Rows[i]).Take(1).First());
                    }

                    foreach (string line in lines)
                    {
                        InferredClassification IC = new InferredClassification();
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
                }
            }
            return ICs;
        }

        /// <summary>
        /// Performs a LINQ Query to see if the 'match' passed
        /// in as a parameter is contained within the mapping 
        /// table dictionary.
        /// </summary>
        /// <param name="match">A classification used as a query for the mapping table</param>
        /// <returns>IEnumerable Key Value pair where Key is a classification code and
        /// the value is a Pointer to the file and line number of that reference.</returns>
        public IEnumerable<KeyValuePair<string, Pointer>> GetMatches(string match)
        {
            IEnumerable<KeyValuePair<string, Pointer>>
                matches = from map in Mappings
                          where map.Key.Contains(match)
                          select map;
            return matches;
        }
    }
}

