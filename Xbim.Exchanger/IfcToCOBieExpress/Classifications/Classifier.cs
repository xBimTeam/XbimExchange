using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using XbimExchanger.IfcToCOBieExpress.Classifications.Components;

namespace XbimExchanger.IfcToCOBieExpress.Classifications
{
    internal class Classifier
    {

        private readonly MappingIfcClassificationReferenceToCategory _classificationReferenceToCategory;
        private readonly MappingIfcClassificationToCobieClassification _classificationToClassification;

        private const string Uniclass2015Name = "Uniclass 2015 Reference (Inferred)";
        private const string NBSReferenceName = "NBS Reference (Inferred)";
        private const string NRMReferenceName = "NRM Reference (Inferred)";

        public Classifier(XbimExchanger<IfcStore, IModel> exchanger)
        {
            _classificationReferenceToCategory =
                exchanger.GetOrCreateMappings<MappingIfcClassificationReferenceToCategory>();
            _classificationToClassification =
                exchanger.GetOrCreateMappings<MappingIfcClassificationToCobieClassification>();
        }

        /// <summary>
        /// This is the constructor for the Classifier Class
        /// </summary>
        public void Classify(CobieFacility facility)
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
        private void AddClassificationsToAssets(CobieFacility facility)
        {
            var dataReader = new ClassificationMappingReader();//DataReader Object which will create and populate the mappings table.
            //Get Each AssetType
            foreach (var type in facility.Model.Instances.OfType<CobieType>())
            {
                //Get Each Asset
                foreach (var component in type.Components)
                {
                    //Get Each Property
                    foreach (var attribute in component.Attributes)
                    {
                        if (attribute.Value == null || component.Categories.Count != 0) continue;
                        //Get the Inferred Classifications

                        var inferredClassifications = FindInferredClassifications(attribute.Value.ToString(), dataReader);

                        foreach (var ic in inferredClassifications)
                        {
                            var uniclassMatch = false;
                            var nbsMatch = false;
                            var nrmMatch = false;

                            foreach (var cat in component.Categories)
                            {
                                if (ic.UniclassCode != null && cat.Value == ic.UniclassCode)
                                {
                                    uniclassMatch = true;
                                }
                                if (ic.NbsCode != null && cat.Value == ic.NbsCode)
                                {
                                    nbsMatch = true;
                                }
                                if (ic.NrmCode != null && cat.Value == ic.NrmCode)
                                {
                                    nrmMatch = true;
                                }
                            }
                            //Add the Classifications as categories if they exist.
                            if (ic.UniclassCode != null && !uniclassMatch)
                            {
                                var uniClass = GetCategory(ic.UniclassCode, ic.UniclassDescription);
                                uniClass.Classification = GetClassification(Uniclass2015Name);
                                component.Categories.Add(uniClass);
                            }
                            if (ic.NbsCode != null && !nbsMatch)
                            {
                                var nbs = GetCategory(ic.NbsCode, ic.NbsDescription);
                                nbs.Classification = GetClassification(NBSReferenceName);
                                component.Categories.Add(nbs);
                            }
                            if (ic.NrmCode != null && !nrmMatch)
                            {
                                var nrm = GetCategory(ic.NrmCode, ic.NrmDescription);
                                nrm.Classification = GetClassification(NRMReferenceName);
                                component.Categories.Add(nrm);
                            }
                        }
                    }
                }
            }
        }

        private CobieClassification GetClassification(string name)
        {
            CobieClassification classification;
            if (_classificationToClassification.GetOrCreateTargetObject(name, out classification))
                classification.Name = name;
            return classification;
        }

        private CobieCategory GetCategory(string code, string description)
        {
            CobieCategory category;
            if (_classificationReferenceToCategory.GetOrCreateTargetObject(code, out category))
            {
                category.Value = code;
                category.Description = description;
            }
            return category;
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

