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
            var dataReader = new ClassificationMappingReader();//DataReader Object which will create and populate the mappings table.
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

                            var inferredClassifications = FindInferredClassifications(attr.Value.GetStringValue(), dataReader);

                            foreach (var ic in inferredClassifications)
                            {
                                bool UniclassMatch = false;
                                bool NbsMatch = false;
                                bool NrmMatch = false;

                                foreach (var cat in a.Categories)
                                {
                                    if (ic.UniclassCode != null && cat.Code == ic.UniclassCode)
                                    {
                                        UniclassMatch = true;
                                    }
                                    if (ic.NbsCode != null && cat.Code == ic.NbsCode)
                                    {
                                        NbsMatch = true;
                                    }
                                    if (ic.NrmCode != null && cat.Code == ic.NrmCode)
                                    {
                                        NrmMatch = true;
                                    }
                                }
                                //Add the Classifications as categories if they exist.
                                if (ic.UniclassCode != null && !UniclassMatch)
                                {
                                    var uniClass = new Xbim.COBieLiteUK.Category();
                                    uniClass.Classification = "Uniclass 2015 Reference (Inferred)";
                                    uniClass.Code = ic.UniclassCode;
                                    uniClass.Description = ic.UniclassDescription;
                                    a.Categories.Add(uniClass);
                                }
                                if (ic.NbsCode != null && !NbsMatch)
                                {
                                    Xbim.COBieLiteUK.Category Nbs = new Xbim.COBieLiteUK.Category();
                                    Nbs.Classification = "NBS Reference (Inferred)";
                                    Nbs.Code = ic.NbsCode;
                                    Nbs.Description = ic.NbsDescription;
                                    a.Categories.Add(Nbs);
                                }
                                if (ic.NrmCode != null && !NrmMatch)
                                {
                                    Xbim.COBieLiteUK.Category Nrm = new Xbim.COBieLiteUK.Category();
                                    Nrm.Classification = "NRM Reference (Inferred)";
                                    Nrm.Code = ic.NrmCode;
                                    Nrm.Description = ic.NrmDescription;
                                    a.Categories.Add(Nrm);
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
        static private IEnumerable<InferredClassification> FindInferredClassifications(string property, ClassificationMappingReader dataReader)
        {
            //Check to see if the property is a valid classification            
            Pointer match = null;
            var classificationMatches = Regex.Match(property, RegexPatterns.UniclassPattern);
            if (classificationMatches.Success)
                match = dataReader.GetMatchingPointer(classificationMatches.Value); //Get Uniclass matches from the Mappings Table
            else
            {
                classificationMatches = Regex.Match(property, RegexPatterns.NbsPattern);
                if (classificationMatches.Success)
                    match = dataReader.GetMatchingPointer(classificationMatches.Value);
                else
                {
                    classificationMatches = Regex.Match(property, RegexPatterns.NrmPattern);
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

