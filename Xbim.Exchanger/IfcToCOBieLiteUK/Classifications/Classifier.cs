using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xbim.COBieLiteUK;
using Xbim.Exchanger.IfcToCOBieLiteUK.Classifications.Components;

namespace Xbim.Exchanger.IfcToCOBieLiteUK.Classifications
{
    static public class FacilityClassifierExtensions
    {
        /// <summary>
        /// This is the constructor for the Classifier Class
        /// </summary>
        static public void Classify(this Facility facility)
        {
            
            AddClassificationsToAssets(facility);
        }

        /// <summary>
        /// This method searches through the facilities to look
        /// for properties inside the property sets of Assets
        /// to look for values that match Regex classification 
        /// formats, set out in the DataReader. It then adds any
        /// values that match the Regular Expression as a category
        /// of assets which conforms with the Schema.
        /// </summary>
        static private void AddClassificationsToAssets(Facility facility)
        {
            var dataReader = new DataReader();//DataReader Object which will create and populate the mappings table.
            //Get Each AssetType
            foreach (AssetType at in facility.AssetTypes)
            {
                //Get Each Asset
                foreach (Asset a in at.Assets)
                {
                    //Create a new Category for the Asset
                    a.Categories = new List<Category>();

                    //Get Each Property
                    foreach (var attr in a.Attributes)
                    {
                        if (attr.Value.GetStringValue() != null && a.Categories.Count == 0)
                        {
                            //Get the Inferred Classifications

                            var ICs = FindInferredClassifications(attr.Value.GetStringValue(), dataReader);

                            foreach (var IC in ICs)
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
                                    var uniClass = new Xbim.COBieLiteUK.Category();
                                    uniClass.Classification = "Uniclass 2015 Reference (Inferred)";
                                    uniClass.Code = IC.UNICode;
                                    uniClass.Description = IC.UNIDescription;
                                    a.Categories.Add(uniClass);
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
        }

        /// <summary>
        /// Checks the property passed in as a parement to see
        /// if the property value matches the format of either
        /// Uniclass, NBS or NRM.
        /// </summary>
        /// <param name="property">A string value of the assets property</param>
        /// <returns>An InferredClassification which contains the classification mappings</returns>
        static private IEnumerable<InferredClassification> FindInferredClassifications(string property, DataReader dataReader)
        {
            //Check to see if the property is a valid classification            
            Pointer match = null;
            var classificationMatches = Regex.Match(property, RegexPatterns.UNIPattern);
            if (classificationMatches.Success)
                match = dataReader.GetMatchingPointer(classificationMatches.Value); //Get Uniclass matches from the Mappings Table
            else
            {
                classificationMatches = Regex.Match(property, RegexPatterns.NBSPattern);
                if (classificationMatches.Success)
                    match = dataReader.GetMatchingPointer(classificationMatches.Value);
                else
                {
                    classificationMatches = Regex.Match(property, RegexPatterns.NRMPattern);
                    if (classificationMatches.Success)
                        match = dataReader.GetMatchingPointer(classificationMatches.Value);
                }
            }
            //Get Mappings that match the InferredClassification
            if (match != null)
                return dataReader.GetInferredMapping(match);
            else
                return Enumerable.Empty<InferredClassification>();
        }


    }
}

