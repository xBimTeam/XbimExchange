using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xbim.CobieExpress;
using XbimExchanger.IfcToCOBieExpress.Classifications.Components;

namespace XbimExchanger.IfcToCOBieExpress.Classifications
{
    public static class FacilityClassifierExtensions
    {
        /// <summary>
        /// This is the constructor for the Classifier Class
        /// </summary>
        public static void Classify(this CobieFacility facility)
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
        private static void AddClassificationsToAssets(CobieFacility facility)
        {
            var dataReader = new ClassificationMappingReader();//DataReader Object which will create and populate the mappings table.
            //Get Each AssetType
            foreach (CobieType at in facility.AssetTypes)
            {
                //Get Each Asset
                foreach (var a in at.Components)
                {
                    //Get Each Property
                    foreach (var attr in a.Attributes)
                    {
                        if (attr.Value.GetStringValue() == null || a.Categories.Count != 0) continue;
                        //Get the Inferred Classifications

                        var inferredClassifications = FindInferredClassifications(attr.Value.GetStringValue(), dataReader);

                        foreach (var ic in inferredClassifications)
                        {
                            var uniclassMatch = false;
                            var nbsMatch = false;
                            var nrmMatch = false;

                            foreach (var cat in a.Categories)
                            {
                                if (ic.UniclassCode != null && cat.Code == ic.UniclassCode)
                                {
                                    uniclassMatch = true;
                                }
                                if (ic.NbsCode != null && cat.Code == ic.NbsCode)
                                {
                                    nbsMatch = true;
                                }
                                if (ic.NrmCode != null && cat.Code == ic.NrmCode)
                                {
                                    nrmMatch = true;
                                }
                            }
                            //Add the Classifications as categories if they exist.
                            if (ic.UniclassCode != null && !uniclassMatch)
                            {
                                var uniClass = new CobieCategory();
                                uniClass.Classification = "Uniclass 2015 Reference (Inferred)";
                                uniClass.Code = ic.UniclassCode;
                                uniClass.Description = ic.UniclassDescription;
                                a.Categories.Add(uniClass);
                            }
                            if (ic.NbsCode != null && !nbsMatch)
                            {
                                var nbs = new CobieCategory();
                                nbs.Classification = "NBS Reference (Inferred)";
                                nbs.Code = ic.NbsCode;
                                nbs.Description = ic.NbsDescription;
                                a.Categories.Add(nbs);
                            }
                            if (ic.NrmCode != null && !nrmMatch)
                            {
                                var nrm = new CobieCategory();
                                nrm.Classification = "NRM Reference (Inferred)";
                                nrm.Code = ic.NrmCode;
                                nrm.Description = ic.NrmDescription;
                                a.Categories.Add(nrm);
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
        /// <param name="dataReader"></param>
        /// <returns>An InferredClassification which contains the classification mappings</returns>
        private static IEnumerable<InferredClassification> FindInferredClassifications(string property, ClassificationMappingReader dataReader)
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
            return match != null ? 
                dataReader.GetInferredMapping(match) : 
                Enumerable.Empty<InferredClassification>();
        }


    }
}

