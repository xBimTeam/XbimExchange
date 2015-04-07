using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.Formula.Functions;
using Xbim.COBieLiteUK;
using Attribute = Xbim.COBieLiteUK.Attribute;
using Xbim.CobieLiteUK.Validation.RequirementDetails;

namespace Xbim.CobieLiteUK.Validation.Extensions
{
    /// <summary>
    /// Provides extension methods to simplify the execution of validation algorithms on CobieLiteUK AssetTypes.
    /// </summary>
    static internal class AssetTypesExtensions
    {

       
        /// <summary>
        /// Filters a provided enumerable of AssetTypes matching a specified category
        /// </summary>
        /// <param name="types">The initial enumerable</param>
        /// <param name="requiredCategory">Classification and Codes of the provided categories will be tested for matches</param>
        /// <param name="includeCategoryChildren">if true extends the matching rule to include all categories starting with the required code</param>
        /// <returns></returns>
        static public IEnumerable<AssetTypeCategoryMatch> GetClassificationMatches(this IEnumerable<AssetType> types, Category requiredCategory, bool includeCategoryChildren = true)
        {
            if (requiredCategory == null)
                return Enumerable.Empty<AssetTypeCategoryMatch>();


            var buildingDictionary = new Dictionary<AssetType, AssetTypeCategoryMatch>();

            foreach (var evaluatingType in types)
            {
                if (evaluatingType.Categories == null)
                    continue;

                var buffer = includeCategoryChildren
                    ? evaluatingType.Categories.MatchingChildrenOf(requiredCategory).ToList()
                    : evaluatingType.Categories.Matching(requiredCategory).ToList();

                if (!buffer.Any()) 
                    continue;
                if (!buildingDictionary.ContainsKey(evaluatingType))
                    buildingDictionary.Add(evaluatingType, new AssetTypeCategoryMatch(evaluatingType));
                buildingDictionary[evaluatingType].MatchingCategories.AddRange(buffer);
            }

            return buildingDictionary.Values;    
        }

        private const string MatchingCategoriesAttributeName = "DPoWMatchingCategories";
        private const string MatchingCodesAttributeName = "DPoWMatchingCodes";

        private const string RequirementCategoriesAttributeName = "DPoWRequirementCategories";
        private const string RequirementCodesAttributeName = "DPoWRequirementCodes";
        private const string RequirementDescsAttributeName = "DPoWRequirementDescs";

        private const string SubmittedAssetsAttributeName = "DPoWSubmittedAssetsCount";
        private const string ValidAssetsAttributeName = "DPoWValidSubmittedAssetsCount";
        private const string RequirementExternalSystemAttributeName = "RefRequirementExternalSystem";
        private const string RequirementExternalIdAttributeName = "RefRequirementExternalId";
        private const string RequirementNameAttributeName = "RefRequirementName";
        private const string AttributesPropertySetName = "DPoW Attributes";

        static public string GetRequirementExternalSystem(this AssetType retType)
        {
            return GetStringValue(retType, RequirementExternalSystemAttributeName);
        }

        static public string GetRequirementExternalId(this AssetType retType)
        {
            return GetStringValue(retType, RequirementExternalIdAttributeName);
        }

        static public string GetRequirementName(this AssetType retType)
        {
            return GetStringValue(retType, RequirementNameAttributeName);
        }


        static public void SetRequirementExternalSystem(this AssetType retType, string value)
        {
            if (retType == null)
                return;
            SetStringValue(retType, value, RequirementExternalSystemAttributeName, "ExternalSystem of the asset type containing the related requirement.");
        }

        static public void SetRequirementExternalId(this AssetType retType, string value)
        {
            if (retType == null)
                return;
            SetStringValue(retType, value, RequirementExternalIdAttributeName, "ExternalId of the asset type containing the related requirement.");
        }

        static public void SetRequirementName(this AssetType retType, string value)
        {
            if (retType == null)
                return;
            SetStringValue(retType, value, RequirementNameAttributeName, "Name of the asset type containing the related requirement.");
        }

        static public void SetSubmittedAssetsCount(this AssetType retType, int value)
        {
            if (retType == null)
                return;
            SetIntegerValue(retType, value, SubmittedAssetsAttributeName, "Count of submitted assets under this AssetType.");
        }

        static public int GetSubmittedAssetsCount(this AssetType retType)
        {
            return GetIntegerValue(retType, SubmittedAssetsAttributeName);
        }


        static public void SetValidAssetsCount(this AssetType retType, int value)
        {
            if (retType == null)
                return;
            SetIntegerValue(retType, value, ValidAssetsAttributeName, "Count of submitted assets under this AssetType that satisfy requirements.");
        }

        static public int GetValidAssetsCount(this AssetType retType)
        {
            return GetIntegerValue(retType, ValidAssetsAttributeName);
        }

        private static string GetStringValue(AssetType retType, string AttributeName)
        {
            if (retType.Attributes == null)
                return "";

            var existingAttribute =
                retType.Attributes.FirstOrDefault(
                    a => a.Name == AttributeName && a.PropertySetName == AttributesPropertySetName);

            if (existingAttribute == null)
                return "";

            var stringValue = existingAttribute.Value as StringAttributeValue;
            return stringValue == null 
                ? @""
                : stringValue.Value;
        }

        private static int GetIntegerValue(AssetType retType, string AttributeName)
        {
            if (retType.Attributes == null)
                return 0;

            var existingAttribute =
                retType.Attributes.FirstOrDefault(
                    a => a.Name == AttributeName && a.PropertySetName == AttributesPropertySetName);

            if (existingAttribute == null)
                return 0;

            var integerValue = existingAttribute.Value as IntegerAttributeValue;
            return integerValue == null || !integerValue.Value.HasValue
                ? 0
                : integerValue.Value.Value;
        }

        private static void SetStringValue(AssetType retType, string value, string propertyName, string propertyDescription)
        {
            if (retType.Attributes == null)
                retType.Attributes = new List<Attribute>();

            var existingAttribute =
                retType.Attributes.FirstOrDefault(
                    a => a.Name == propertyName && a.PropertySetName == AttributesPropertySetName);

            if (existingAttribute != null)
            {
                existingAttribute.Value = new StringAttributeValue() { Value = value };
            }
            else
            {
                var matchingClassAttribute = new Attribute
                {
                    Name = propertyName,
                    PropertySetName = AttributesPropertySetName,
                    Description = propertyDescription,
                    Value = new StringAttributeValue() { Value = value },
                    Categories = new List<Category>() { dpowValidatedAttributeClass }
                };
                retType.Attributes.Add(matchingClassAttribute);
            }
        }

        private static Category dpowValidatedAttributeClass
        {
            get { return new Category() {Classification = @"DPoW", Code = "reference"}; }
        }

        private static void SetIntegerValue(AssetType retType, int value, string propertyName, string propertyDescription)
        {
            if (retType.Attributes == null)
                retType.Attributes = new List<Attribute>();

            var existingAttribute =
                retType.Attributes.FirstOrDefault(
                    a => a.Name == propertyName && a.PropertySetName == AttributesPropertySetName);

            if (existingAttribute != null)
            {
                existingAttribute.Value = new IntegerAttributeValue() {Value = value};
            }
            else
            {
                var matchingClassAttribute = new Attribute
                {
                    Name = propertyName,
                    PropertySetName = AttributesPropertySetName,
                    Description = propertyDescription,
                    Value = new IntegerAttributeValue() {Value = value},
                    Categories = new List<Category>() { dpowValidatedAttributeClass }
                };
                retType.Attributes.Add(matchingClassAttribute);
            }
        }

        public static IEnumerable<Category> GetMatchingCategories(this AssetType retType)
        {
            var cls = retType.GetMatchingClassifications().GetEnumerator();
            var codes = retType.GetMatchingCodes().GetEnumerator();

            while (cls.MoveNext() && codes.MoveNext())
            {
                yield return new Category()
                {
                    Classification = cls.Current,
                    Code = codes.Current
                };
            }
        }

        public static IEnumerable<Category> GetRequirementCategories(this AssetType retType)
        {
            var cls = retType.GetRequirementClassifications().GetEnumerator();
            var codes = retType.GetRequirementCodes().GetEnumerator();
            var descs = retType.GetRequirementDescs().GetEnumerator();

            while (cls.MoveNext() && codes.MoveNext() && descs.MoveNext())
            {
                yield return new Category()
                {
                    Classification = cls.Current,
                    Code = codes.Current,
                    Description = descs.Current
                };
            }
        }

        internal static IEnumerable<string> GetRequirementDescs(this AssetType retType)
        {
            return GetStringListFromCompound(retType, RequirementDescsAttributeName);
        }

        internal static IEnumerable<string> GetRequirementClassifications(this AssetType retType)
        {
            return GetStringListFromCompound(retType, RequirementCategoriesAttributeName);
        }

        internal static IEnumerable<string> GetRequirementCodes(this AssetType retType)
        {
            return GetStringListFromCompound(retType, RequirementCodesAttributeName);
        }
        
        internal static IEnumerable<string> GetMatchingClassifications(this AssetType retType)
        {
            return GetStringListFromCompound(retType, MatchingCategoriesAttributeName);
        }

        internal static IEnumerable<string> GetMatchingCodes(this AssetType retType)
        {
            return GetStringListFromCompound(retType, MatchingCodesAttributeName);
        }
        

        private static IEnumerable<string> GetStringListFromCompound(AssetType retType, string p)
        {
            if (retType.Attributes == null)
                return Enumerable.Empty<string>();

            var existingAttribute =
                retType.Attributes.FirstOrDefault(
                    a => a.Name == p && a.PropertySetName == AttributesPropertySetName);

            if (existingAttribute == null)
                return Enumerable.Empty<string>();

            var compoundStringValue = existingAttribute.Value as StringAttributeValue;
            return compoundStringValue == null
                ? Enumerable.Empty<string>()
                : compoundStringValue.Value.CompoundStringToList();
        }

        public static void SetMatchingCategories(this AssetType retType, IEnumerable<Category> matchingCategories)
        {
            var categories = matchingCategories as Category[] ?? matchingCategories.ToArray();
            retType.SetMatchingClassifications(categories.Select(x=>x.Classification));
            retType.SetMatchingCodes(categories.Select(x => x.Code));
        }

        public static void SetRequirementCategories(this AssetType retType, IEnumerable<Category> matchingCategories)
        {
            var categories = matchingCategories as Category[] ?? matchingCategories.ToArray();
            retType.SetRequirementClassifications(categories.Select(x => x.Classification));
            retType.SetRequirementCodes(categories.Select(x => x.Code));
            retType.SetRequirementDescs(categories.Select(x => x.Description));
        }

        private static void SetMatchingCodes(this AssetType retType, IEnumerable<string> matchingCategories)
        {
            const string description = "Comma separated classification names of the validation candidate that match a requirement.";
            SetListToCompoundAttribute(retType, matchingCategories, MatchingCodesAttributeName, description);
        }

        private static void SetRequirementDescs(this AssetType retType, IEnumerable<string> matchingCategories)
        {
            const string description = "Comma separated classification description applicable to the validation requirement.";
            SetListToCompoundAttribute(retType, matchingCategories, RequirementDescsAttributeName, description);
        }

        private static void SetRequirementCodes(this AssetType retType, IEnumerable<string> matchingCategories)
        {
            const string description = "Comma separated classification codes applicable to the validation requirement.";
            SetListToCompoundAttribute(retType, matchingCategories, RequirementCodesAttributeName, description);
        }

        private static void SetMatchingClassifications(this AssetType retType, IEnumerable<string> matchingCategories)
        {
            const string description = "Comma separated values of the validation candidate that match a classification requirement.";
            SetListToCompoundAttribute(retType, matchingCategories, MatchingCategoriesAttributeName, description);
        }

        private static void SetRequirementClassifications(this AssetType retType, IEnumerable<string> matchingCategories)
        {
            const string description = "Comma separated classification names applicable to the validation requirement.";
            SetListToCompoundAttribute(retType, matchingCategories, RequirementCategoriesAttributeName, description);
        }

        private static void SetListToCompoundAttribute(AssetType retType, IEnumerable<string> list, string propName,
            string description)
        {
            if (retType.Attributes == null)
                retType.Attributes = new List<Attribute>();

            var existingAttribute =
                retType.Attributes.FirstOrDefault(
                    a => a.Name == propName && a.PropertySetName == AttributesPropertySetName);

            var newValue = list.ListToCompoundString();

            if (existingAttribute != null)
            {
                existingAttribute.Value = new StringAttributeValue()
                {
                    Value = newValue
                };
            }
            else
            {
                var matchingClassAttribute = new Attribute
                {
                    Name = propName,
                    PropertySetName = AttributesPropertySetName,
                    Description = description,
                    Value = new StringAttributeValue() { Value = newValue },
                    Categories = new List<Category>() { dpowValidatedAttributeClass }
                };
                retType.Attributes.Add(matchingClassAttribute);
            }
        }
    }
}
