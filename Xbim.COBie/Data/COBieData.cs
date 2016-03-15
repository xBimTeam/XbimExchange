using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.ProductExtension;
using System.Globalization;
using Xbim.Common;
using Xbim.COBie.Resources;
using Xbim.Ifc2x3.ApprovalResource;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.Ifc2x3.IO;
using Xbim.Ifc2x3.MaterialResource;


namespace Xbim.COBie.Data
{
    /// <summary>
    /// Base class for the input of data into the Excel worksheets
    /// </summary>
    public abstract class COBieData<T> where T : COBieRow
    {

        protected const string DEFAULT_STRING = Constants.DEFAULT_STRING;
        protected const string DEFAULT_NUMERIC = Constants.DEFAULT_NUMERIC;

        protected COBieContext Context { get; set; }
        private readonly COBieProgress _progressStatus;
        protected int UnknownCount { get; set; } //use for unnamed items as an index

        
        //private static Dictionary<long, string> _eMails = new Dictionary<long, string>();

        protected COBieData()
        { }


        protected COBieData(COBieContext context)
        {
            Context = context;
            _progressStatus = new COBieProgress(context);
            UnknownCount = 1;
        }

        protected XbimModel Model
        {
            get
            {
                return Context.Model;
            }
        }

        protected COBieProgress ProgressIndicator
        {
            get
            {
                return _progressStatus;
            }
        }


        

        #region Methods

        public abstract COBieSheet<T> Fill();

        /// <summary>
        /// Extract the Created On date from the passed entity
        /// </summary>
        /// <param name="rootEntity">Entity to extract the Create On date</param>
        /// <returns></returns>
        protected string GetCreatedOnDateAsFmtString(IfcOwnerHistory ownerHistory, bool requiresTime = true)
        {
            var date = GetCreatedOnDate(ownerHistory);
            //return default date of now
            return date ?? (requiresTime ? Context.RunDateTime : Context.RunDate); //if we don't have a date then use the context date or datetime
        }

        public static string GetCreatedOnDate(IfcOwnerHistory ownerHistory)
        {
            if (ownerHistory != null)
            {
                var createdOnTStamp = (int)ownerHistory.CreationDate;
                if (createdOnTStamp != 0) //assume not set, but could it be 1970/1/1 00:00:00!!!
                {
                    //to remove trailing decimal seconds use a set format string as "o" option is to long.

                    //We have a day light saving problem with the comparison with other COBie Export Programs. if we convert to local time we get a match
                    //but if the time stamp is Coordinated Universal Time (UTC), then daylight time should be ignored. see http://msdn.microsoft.com/en-us/library/bb546099.aspx
                    //IfcTimeStamp.ToDateTime(CreatedOnTStamp).ToLocalTime()...; //test to see if corrects 1 hour difference, and yes it did, but should we?

                    return IfcTimeStamp.ToDateTime(createdOnTStamp).ToString(Constants.DATETIME_FORMAT);
                    
                }
            }
            return null;
        }


        /// <summary>
        /// Gets the name of the application that is linked with the supplied item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetExternalSystem(IfcOwnerHistory ifcOwnerHistory)
        {
            var appName = "";

            if (ifcOwnerHistory != null)
            {
                if (ifcOwnerHistory.LastModifyingApplication != null)
                    appName = ifcOwnerHistory.LastModifyingApplication.ApplicationFullName;
                if ((string.IsNullOrEmpty(appName)) &&
                    (ifcOwnerHistory.OwningApplication != null)
                    )
                    appName = ifcOwnerHistory.OwningApplication.ApplicationFullName; 
            }

            return string.IsNullOrEmpty(appName) ? DEFAULT_STRING : appName;
        }

        /// <summary>
        /// Gets the name of the application that is linked with the supplied item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetExternalSystem(IfcRoot item)
        {
            return GetExternalSystem(item.OwnerHistory);
        }

        //fields for GetMaterialOwnerHistory function
        private List<IfcRelAssociatesMaterial> _ifcRelAssociatesMaterials;
        private List<IfcMaterialLayerSetUsage> _ifcMaterialLayerSetUsages;

        /// <summary>
        /// Get the IfcRelAssociatesMaterial object from the passed IfcMaterialLayerSet 
        /// </summary>
        /// <param name="ifcMaterialLayerSet">IfcMaterialLayerSet object</param>
        /// <returns>IfcOwnerHistory object or null if none found</returns>
        protected IfcOwnerHistory GetMaterialOwnerHistory(IfcMaterialLayerSet ifcMaterialLayerSet)
        {

            if (_ifcRelAssociatesMaterials == null)
            {
                _ifcRelAssociatesMaterials = Model.FederatedInstances.OfType<IfcRelAssociatesMaterial>().ToList();
                _ifcMaterialLayerSetUsages = Model.FederatedInstances.OfType<IfcMaterialLayerSetUsage>().ToList();
            }

            var ifcMaterialLayerSetUsage = _ifcMaterialLayerSetUsages.FirstOrDefault(mlsu => mlsu.ForLayerSet == ifcMaterialLayerSet);
            
            IfcRelAssociatesMaterial ifcRelAssociatesMaterial = null;
            if (ifcMaterialLayerSetUsage != null)
                ifcRelAssociatesMaterial = _ifcRelAssociatesMaterials.FirstOrDefault(ram => (ram.RelatingMaterial is IfcMaterialLayerSetUsage) && ((IfcMaterialLayerSetUsage) ram.RelatingMaterial == ifcMaterialLayerSetUsage));
                
            if (ifcRelAssociatesMaterial == null)
                ifcRelAssociatesMaterial = _ifcRelAssociatesMaterials.FirstOrDefault(ram => (ram.RelatingMaterial is IfcMaterialLayerSet) && ((IfcMaterialLayerSet) ram.RelatingMaterial == ifcMaterialLayerSet));

            if (ifcRelAssociatesMaterial == null)
                ifcRelAssociatesMaterial = _ifcRelAssociatesMaterials.FirstOrDefault(ram => ifcMaterialLayerSet.MaterialLayers.Contains(ram.RelatingMaterial));

            if (ifcRelAssociatesMaterial != null)
                return ifcRelAssociatesMaterial.OwnerHistory;
            else
                return null;
        }

        /// <summary>
        /// Extract the email address lists for the owner of the IfcOwnerHistory passed
        /// </summary>
        /// <param name="ifcOwnerHistory">Entity to extract the email addresses for</param>
        /// <returns>string of comma delimited addresses</returns>
        protected string GetTelecomTelephoneNumber(IfcPersonAndOrganization ifcPersonAndOrganization)
        {
            var telephoneNo = "";
            var ifcOrganization = ifcPersonAndOrganization.TheOrganization;
            var ifcPerson = ifcPersonAndOrganization.ThePerson;
                
            if (ifcPerson.Addresses != null)
            {
                telephoneNo = ifcPerson.Addresses
                    .OfType<IfcTelecomAddress>()
                    .Select(address => address.TelephoneNumbers)
                    .Where(item => item != null)
                    .SelectMany(em => em)
                    .FirstOrDefault(em => !string.IsNullOrEmpty(em));

                if (string.IsNullOrEmpty(telephoneNo))
                {
                    if (ifcOrganization.Addresses != null)
                    {
                        telephoneNo = ifcOrganization
                            .Addresses.OfType<IfcTelecomAddress>()
                            .Select(address => address.TelephoneNumbers)
                            .Where(item => item != null)
                            .SelectMany(em => em)
                            .FirstOrDefault(em => !string.IsNullOrEmpty(em));
                    }
                } 
            }
            
            //if still no email lets make one up
            if (string.IsNullOrEmpty(telephoneNo)) telephoneNo = DEFAULT_STRING;
            

            return telephoneNo;
        }

        /// <summary>
        /// Clear the email dictionary for next file
        /// </summary>
        public void ClearEMails()
        {
            Context.EMails.Clear();
        }



        /// <summary>
        /// Extract the email address lists for the owner of the IfcOwnerHistory passed
        /// </summary>
        /// <param name="ifcOwnerHistory">Entity to extract the email addresses for</param>
        /// <returns>string of comma delimited addresses</returns>
        protected string GetTelecomEmailAddress(IfcOwnerHistory ifcOwnerHistory)
        {
            if ((ifcOwnerHistory != null) &&
                (ifcOwnerHistory.OwningUser != null) &&
                (ifcOwnerHistory.OwningUser.ThePerson != null)
                )
            {
                var ifcPerson = ifcOwnerHistory.OwningUser.ThePerson;
                if (Context.EMails.ContainsKey(ifcPerson.EntityLabel))
                {
                    return Context.EMails[ifcPerson.EntityLabel];
                }
                else
                {
                    var ifcOrganization = ifcOwnerHistory.OwningUser.TheOrganization;
                    var email = GetEmail(ifcOrganization, ifcPerson);
                    //save to the email directory for quick retrieval
                    Context.EMails.Add(ifcPerson.EntityLabel, email);
                    return email;
                }
            }
            else if (Context.EMails.Count == 1) //if only one then no contact are probably set up so use as default
                return Context.EMails.First().Value;
            else
                return Constants.DEFAULT_EMAIL;
        }
        /// <summary>
        /// Extract the email address lists for the owner of the IfcOwnerHistory passed
        /// </summary>
        /// <param name="ifcOwnerHistory">Entity to extract the email addresses for</param>
        /// <returns>string of comma delimited addresses</returns>
        protected string GetTelecomEmailAddress(IfcPersonAndOrganization ifcPersonAndOrganization)
        {
            if (ifcPersonAndOrganization != null)
            {
                var ifcPerson = ifcPersonAndOrganization.ThePerson;
                if (Context.EMails.ContainsKey(ifcPerson.EntityLabel))
                {
                    return Context.EMails[ifcPerson.EntityLabel];
                }
                else
                {
                    var ifcOrganization = ifcPersonAndOrganization.TheOrganization;
                    var email = GetEmail(ifcOrganization, ifcPerson);
                    Context.EMails.Add(ifcPerson.EntityLabel, email);
                    return email;
                }
            }
            else
                return Constants.DEFAULT_EMAIL;
        }



        /// <summary>
        /// Get email address from IfcPerson 
        /// </summary>
        /// <param name="ifcOrganization"></param>
        /// <param name="ifcPerson"></param>
        /// <returns></returns>
        public static string GetEmail( IfcOrganization ifcOrganization, IfcPerson ifcPerson)
        {
            var email = "";
            var emails = Enumerable.Empty<IfcLabel>();
            if ((ifcPerson != null) && (ifcPerson.Addresses != null))
            {
                emails = ifcPerson
                    .Addresses.OfType<IfcTelecomAddress>()
                    .Select(address => address.ElectronicMailAddresses)
                    .Where(item => item != null).SelectMany(em => em)
                    .Where(em => !string.IsNullOrEmpty(em))
                    .ToList();
                
            }
            if (!emails.Any())
            {
                if ((ifcOrganization != null) && (ifcOrganization.Addresses != null))
                {
                    emails = ifcOrganization.Addresses.OfType<IfcTelecomAddress>().Select(address => address.ElectronicMailAddresses).Where(item => item != null).SelectMany(em => em).Where(em => !string.IsNullOrEmpty(em));
                }
            }


            //if still no email lets make one up
            if (emails.Any())
            {
                email = string.Join(" : ", emails);
            }
            else
            {
                var first = "";
                var lastName = "";
                var organization = "";
                if (ifcPerson != null)
                {
                    first = ifcPerson.GivenName.ToString();
                    lastName = ifcPerson.FamilyName.ToString();
                }
                if (ifcOrganization != null)
                    organization = ifcOrganization.Name.ToString();
                var domType = "";
                if (!string.IsNullOrEmpty(first))
                {
                    var split = first.Split('.');
                    if (split.Length > 1) first = split[0]; //assume first
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    var split = lastName.Split('.');
                    if (split.Length > 1) lastName = split.Last(); //assume last
                }
                

                if (!string.IsNullOrEmpty(organization))
                {
                    var split = organization.Split('.');
                    var index = 1;
                    foreach (var item in split)
                    {
                        
                        if (index == 1)
                            organization = item; //first item always
                        else if (index < split.Length) //all the way up to the last item
                            organization += "." + item;
                        else
                            domType = "." + item; //last item assume domain type
                        index++;
                    }
                    
                }
                
                email += (string.IsNullOrEmpty(first)) ? "unknown" : first;
                email += ".";
                email += (string.IsNullOrEmpty(lastName)) ? "unknown" : lastName;
                email += "@";
                email += (string.IsNullOrEmpty(organization)) ? "unknown" : organization;
                email += (string.IsNullOrEmpty(domType)) ? ".com" : domType;
                email = email.Replace(" ", ""); //remove any spaces
            }

            return email;
        }

        /// <summary>
        /// Converts string to formatted string and if it fails then passes 0 back, mainly to check that we always have a number returned
        /// </summary>
        /// <param name="num">string to convert</param>
        /// <returns>string converted to a formatted string using "F2" as formatter</returns>
        protected string ConvertNumberOrDefault(string num)
        {
            double temp;

            if (double.TryParse(num, out temp))
            {
                return temp.ToString(); // two decimal places
            }
            else
            {
                return DEFAULT_NUMERIC; 
            }

        }

        /// <summary>
        /// Get the category from the IfcRelAssociatesClassification / IfcClassificationReference objects
        /// </summary>
        /// <param name="obj">IfcObjectDefinition object</param>
        /// <returns></returns>
        public string GetCategoryClassification(IfcObjectDefinition obj)
        {
            //Try by relationship first
            var ifcRAC = obj.HasAssociations.OfType<IfcRelAssociatesClassification>().FirstOrDefault();
            IfcClassificationReference ifcCR = null;
            if (ifcRAC != null)
                ifcCR = ifcRAC.RelatingClassification as IfcClassificationReference;

            if (ifcCR != null)
            {
                var conCatChar = " : ";
                if ((Context.TemplateFileName != null) && (Context.TemplateFileName.Contains("COBie-US"))) //change for US format
                    conCatChar = ": "; 
                //holders for first and last part of category
                string itemReference = ifcCR.ItemReference;
                if (!string.IsNullOrEmpty(itemReference))
                    itemReference = itemReference.Trim();
                
                string name = ifcCR.Name;
                if (!string.IsNullOrEmpty(name))
                    name = name.Trim();

                //need to use split as sometime the whole category is stored in both ItemReference and Name
                //We split here as sometimes the whole category(13-15 11 34 11: Office) is place in itemReference and Name
                if ((!string.IsNullOrEmpty(name)) &&
                    (!string.IsNullOrEmpty(itemReference))
                    ) 
                {
                    itemReference = itemReference.Split(':').First().Trim();
                    var nameSplit = name.Split(':');
                    //just in case we have more than one ":"in name
                    if (nameSplit.First().Trim().Equals(itemReference, StringComparison.OrdinalIgnoreCase))
                    {
                        for (var i = 0; i < nameSplit.Count(); i++)
                        {
                            //skip first item
                            if (i == 1) name = nameSplit[i].Trim();
                            if (i > 1) name += conCatChar + nameSplit[i].Trim(); //add back the second, third... ": "
                        }
                    }
                    else
                        name = nameSplit.Last().Trim();
                }

                //Return the Category
                if ((!string.IsNullOrEmpty(itemReference)) &&
                    (!string.IsNullOrEmpty(name)) &&
                    (!itemReference.Equals(name, StringComparison.OrdinalIgnoreCase))
                    )
                    return itemReference + conCatChar + name;
                else if (!string.IsNullOrEmpty(itemReference))
                    return itemReference;
                else if (!string.IsNullOrEmpty(name))
                    return name;
                else if (!string.IsNullOrEmpty(ifcCR.Location))
                    return ifcCR.Location;
                else if ((ifcCR.ReferencedSource != null) && 
                         (!string.IsNullOrEmpty(ifcCR.ReferencedSource.Name))
                        )
                    return ifcCR.ReferencedSource.Name;
            }
            return null;
        }

        /// <summary>
        /// Get Category method
        /// </summary>
        /// <param name="obj">Object to try and extract method from</param>
        /// <returns></returns>
        public string GetCategory(IfcObject obj)
        {
            var categoryRef = GetCategoryClassification(obj);
            if (!string.IsNullOrEmpty(categoryRef))
            {
                return categoryRef;
            }
            //Try by PropertySet as fallback
            var query = from pSet in obj.PropertySets
                        from props in pSet.HasProperties
                        where props.Name.ToString() == "OmniClass Table 13 Category" || props.Name.ToString() == "Category Code" || props.Name.ToString() == "Omniclass Title"
                        select props.ToString().TrimEnd();
            var val = query.FirstOrDefault();

            if (!String.IsNullOrEmpty(val))
            {
                return val;
            }
            return Constants.DEFAULT_STRING;
        }

        

        /// <summary>
        /// Extract the unit name
        /// </summary>
        /// <param name="ifcUnit">ifcUnit object to get unit name from</param>
        /// <returns>string holding unit name</returns>
        public static string GetUnitName(IfcUnit ifcUnit)
        {
            var value = "";
            var sqText = "";

            var unit = ifcUnit as IfcSIUnit;
            if (unit != null)
            {
                var prefixUnit = (unit.Prefix != null) ? unit.Prefix.ToString() : "";
                value = unit.Name.ToString();                                             //see IfcSIUnitName

                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains("_"))
                    {
                        var split = value.Split('_');
                        if (split.Length > 1) sqText = split.First(); //see if _ delimited value such as SQUARE_METRE
                        value = sqText + prefixUnit + split.Last(); //combine to give full unit name 
                    }
                    else
                        value = prefixUnit + value; //combine to give length name
                }

            }
            else if (ifcUnit is IfcConversionBasedUnit)
            {
                var IfcConversionBasedUnit = ifcUnit as IfcConversionBasedUnit;
                value = (IfcConversionBasedUnit.Name != null) ? IfcConversionBasedUnit.Name.ToString() : "";

                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains("_"))
                    {
                        var split = value.Split('_');
                        if (split.Length > 1) sqText = split.First(); //see if _ delimited value such as SQUARE_METRE
                        value = sqText + split.Last(); //combine to give full unit name 
                    }
                }
            }
            else if (ifcUnit is IfcContextDependentUnit)
            {
                var ifcContextDependentUnit = ifcUnit as IfcContextDependentUnit;
                value = ifcContextDependentUnit.Name;
                if (string.IsNullOrEmpty(value)) //fall back to UnitType enumeration
                    value = ifcContextDependentUnit.UnitType.ToString();
            }
            else if (ifcUnit is IfcDerivedUnit)
            {
                var ifcDerivedUnit = ifcUnit as IfcDerivedUnit;
                value = ifcDerivedUnit.UnitType.ToString();
                if ((string.IsNullOrEmpty(value)) && (ifcDerivedUnit.UserDefinedType != null)) //fall back to user defined
                    value = ifcDerivedUnit.UserDefinedType;
            }
            else if (ifcUnit is IfcMonetaryUnit)
            {
                value = GetMonetaryUnitName(ifcUnit as IfcMonetaryUnit);
                return (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value; //don't want to lower case so return here
            }
            value = (string.IsNullOrEmpty(value)) ? DEFAULT_STRING : value.ToLower();

            //check for unit spelling on meter/metre
            if (value.Contains("metre") || value.Contains("meter"))
            {
                var culturemetre = ErrorDescription.meter;
                if (!string.IsNullOrEmpty(culturemetre))
                    if (value.Contains("metre"))
                        value = value.Replace("metre", culturemetre);
                    else
                        value = value.Replace("meter", culturemetre);
                    
            }
            return value;
        }

        /// <summary>
        /// Get Monetary Unit
        /// </summary>
        /// <param name="ifcMonetaryUnit">IfcMonetaryUnit object</param>
        /// <returns>string holding the Monetary Unit</returns>
        private static string GetMonetaryUnitName(IfcMonetaryUnit ifcMonetaryUnit)
        {
            var value = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
               .Where(c => new RegionInfo(c.LCID).ISOCurrencySymbol == ifcMonetaryUnit.Currency.ToString())
               .Select(c => new RegionInfo(c.LCID).CurrencyEnglishName)
               .FirstOrDefault();
            //TODO: Convert currency to match pick list
            //convert to pick list hard coded for now
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Contains("Dollar"))
                    value = "Dollars";
                else if (value.Contains("Euro"))
                    value = "Euros";
                else if (value.Contains("Pound"))
                    value = "Pounds";
                else
                    value = DEFAULT_STRING;
            }
            else
                value = DEFAULT_STRING;
            return value;
        }

        /// <summary>
        /// Determined the sheet the IfcRoot will have come from using the object type
        /// </summary>
        /// <param name="ifcItem">object which inherits from IfcRoot </param>
        /// <returns>string holding sheet name</returns>
        public string GetSheetByObjectType(Type ifcItem)
        {
            
            var value = DEFAULT_STRING;
            if (ifcItem.IsSubclassOf(typeof(IfcTypeObject))) value = Constants.WORKSHEET_TYPE;
            else if (ifcItem == typeof(IfcTypeObject)) value = Constants.WORKSHEET_TYPE;
            else if (ifcItem.IsSubclassOf(typeof(IfcElement))) value = Constants.WORKSHEET_COMPONENT;
            else if (ifcItem.IsSubclassOf(typeof(IfcProcess))) value = Constants.WORKSHEET_JOB;
            else if (ifcItem.IsSubclassOf(typeof(IfcRelDecomposes))) value = Constants.WORKSHEET_ASSEMBLY;
            else if (ifcItem.IsSubclassOf(typeof(IfcRelConnects))) value = Constants.WORKSHEET_CONNECTION;
            else if (ifcItem == typeof(IfcDocumentInformation)) value = Constants.WORKSHEET_DOCUMENT;
            else if (ifcItem == typeof(IfcOrganization)) value = Constants.WORKSHEET_CONTACT;
            else if (ifcItem == typeof(IfcPerson)) value = Constants.WORKSHEET_CONTACT;
            else if (ifcItem == typeof(IfcPersonAndOrganization)) value = Constants.WORKSHEET_CONTACT;
            else if (ifcItem == typeof(IfcSite)) value = Constants.WORKSHEET_FACILITY;
            else if (ifcItem == typeof(IfcBuilding)) value = Constants.WORKSHEET_FACILITY;
            else if (ifcItem == typeof(IfcProject)) value = Constants.WORKSHEET_FACILITY;
            else if (ifcItem == typeof(IfcBuildingStorey)) value = Constants.WORKSHEET_FLOOR;
            else if (ifcItem == typeof(IfcApproval)) value = Constants.WORKSHEET_ISSUE;
            else if (ifcItem == typeof(IfcConstructionEquipmentResource)) value = Constants.WORKSHEET_RESOURCE;
            else if (ifcItem == typeof(IfcSpace)) value = Constants.WORKSHEET_SPACE;
            else if (ifcItem == typeof(IfcConstructionProductResource)) value = Constants.WORKSHEET_SPARE;
            else if (ifcItem == typeof(IfcSystem)) value = Constants.WORKSHEET_SYSTEM;
            else if (ifcItem == typeof(IfcZone)) value = Constants.WORKSHEET_ZONE;
            //Impact and attributes are off property sets so not here for now
            //more sheets as tests date becomes available
            return value;
        }

        
        /// <summary>
        /// Get the associated Type for a IfcObject, so a Door can be of type "Door Type A"
        /// </summary>
        /// <param name="obj">IfcObject to get associated type information from</param>
        /// <returns>string holding the type information</returns>
        protected string GetTypeName(IfcObject obj)
        {
            var value = "";
            var elType = obj.IsDefinedBy.OfType<IfcRelDefinesByType>().FirstOrDefault();
            if ((elType != null) && (elType.RelatingType.Name != null))
                value = elType.RelatingType.Name.ToString();
            if ((string.IsNullOrEmpty(value)) && (obj.ObjectType != null))
                value = obj.ObjectType.ToString();
            return (string.IsNullOrEmpty(value)) ?  DEFAULT_STRING : value;
            //var elType = obj.IsDefinedBy.OfType<IfcRelDefinesByType>().FirstOrDefault();
            //return (elType != null) ? elType.RelatingType.Name.ToString() : DEFAULT_STRING;
        }

        /// <summary>
        /// Check if a string represents a date time
        /// </summary>
        /// <param name="date">string holding date</param>
        /// <returns>bool</returns>
        public bool IsDate (string date)
        {
            DateTime test;
            return DateTime.TryParse(date, out test);
        }

        /// <summary>
        /// Test string for email address format
        /// </summary>
        /// <param name="email">string holding email address</param>
        /// <returns>bool</returns>
        public bool IsEmailAddress(string email)
        {
            try
            {
                if (email == Constants.DEFAULT_STRING) return false; //false
                var address = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                // Do nothing
            }
            return false;
        }

        public bool IsNumeric(string num)
        {
            double test;
            return double.TryParse(num, out test);
        }
        /// <summary>
        /// Get the global units used for this building
        /// </summary>
        /// <param name="model">model object</param>
        /// <param name="wBookUnits">GlobalUnits to place units into</param>
        /// <returns>return passed wBookUnits(GlobalUnits) with units added</returns>
        public static GlobalUnits GetGlobalUnits(IModel model, GlobalUnits wBookUnits )
        {
            var linearUnit = "";
            var areaUnit = "";
            var volumeUnit = "";
            var moneyUnit = "";
            foreach (var ifcUnitAssignment in model.Instances.OfType<IfcUnitAssignment>()) //loop all IfcUnitAssignment
            {
                foreach (var ifcUnit in ifcUnitAssignment.Units) //loop the UnitSet
                {
                    var ifcNamedUnit = ifcUnit as IfcNamedUnit;
                    if (ifcNamedUnit != null)
                    {
                        if ((ifcNamedUnit.UnitType == IfcUnitEnum.LENGTHUNIT) && string.IsNullOrEmpty(linearUnit)) //we want length units until we have value
                        {
                            linearUnit = GetUnitName(ifcUnit);
                            if ( ( !((linearUnit.Contains("feet")) || (linearUnit.Contains("foot"))) )
                                 && (linearUnit.Last() != 's')
                                )
                                linearUnit = linearUnit + "s";
                        }


                        if ((ifcNamedUnit.UnitType == IfcUnitEnum.AREAUNIT) && string.IsNullOrEmpty(areaUnit)) //we want area units until we have value
                        {
                            areaUnit = GetUnitName(ifcUnit);
                            if ( ( !((areaUnit.Contains("feet")) || (areaUnit.Contains("foot")) ) )
                                 && (areaUnit.Last() != 's')
                                )
                                areaUnit = areaUnit + "s";

                        }


                        if ((ifcNamedUnit.UnitType == IfcUnitEnum.VOLUMEUNIT) && string.IsNullOrEmpty(volumeUnit)) //we want volume units until we have value
                        {
                            volumeUnit = GetUnitName(ifcUnit);
                            if ( ( !((volumeUnit.Contains("feet")) || (volumeUnit.Contains("foot"))) )
                                 && (volumeUnit.Last() != 's')
                                )
                                volumeUnit = volumeUnit + "s";
                        }
                    }
                    //get the money unit
                    if ((ifcUnit is IfcMonetaryUnit) && string.IsNullOrEmpty(moneyUnit))
                    {
                        moneyUnit = GetUnitName(ifcUnit);
                        if (moneyUnit.Last() != 's')
                            moneyUnit = moneyUnit + "s";
                    }

                }
            }

            //ensure we have a value on each unit type, if not then default
            linearUnit = string.IsNullOrEmpty(linearUnit) ? DEFAULT_STRING : linearUnit;
            areaUnit = string.IsNullOrEmpty(areaUnit) ? DEFAULT_STRING : areaUnit;
            volumeUnit = string.IsNullOrEmpty(volumeUnit) ? DEFAULT_STRING : volumeUnit;
            moneyUnit = string.IsNullOrEmpty(moneyUnit) ? DEFAULT_STRING : moneyUnit;

            //save values for retrieval by other sheets
            wBookUnits.LengthUnit = linearUnit;
            wBookUnits.AreaUnit = areaUnit;
            wBookUnits.VolumeUnit = volumeUnit;
            wBookUnits.MoneyUnit = moneyUnit;

            return wBookUnits;

        }

        /// <summary>
        /// Get the EnumerationValues from a IfcPropertyEnumeratedValue property
        /// </summary>
        /// <param name="ifcValues">IEnumerable of IfcValue</param>
        /// <returns>delimited string of values</returns>
        public static string GetEnumerationValues(IEnumerable<IfcValue> ifcValues)
        {
            var EnumValues = new List<string>();
            foreach (var item in ifcValues)
            {
                EnumValues.Add(item.Value.ToString());
            }
            return string.Join(" : ", EnumValues);
        }

        /// <summary>
        /// Get the IfcPropertySingleValue value and unit associated with the value
        /// </summary>
        /// <param name="propertyList">List of IfcSimpleProperty</param>
        /// <param name="name">property name we want to extract</param>
        /// <returns></returns>
        public Interval GetPropertyValue(List<IfcSimpleProperty> propertyList, string name)
        {
            var result = new Interval() { Value = DEFAULT_STRING, Unit = DEFAULT_STRING };
            var ifcPSValue = propertyList.OfType<IfcPropertySingleValue>().Where(psv => psv.Name == name).FirstOrDefault();
            if (ifcPSValue != null)
            {
                result.Value = (!string.IsNullOrEmpty(ifcPSValue.NominalValue.ToString())) ? ifcPSValue.NominalValue.Value.ToString() : DEFAULT_STRING;
                result.Unit = (ifcPSValue.Unit != null) ? GetUnitName(ifcPSValue.Unit) : DEFAULT_STRING;
            }
            return result;
        }

        /// <summary>
        /// Get the IfcPropertyEnumeratedValue value and unit associated with the value
        /// </summary>
        /// <param name="propertyList">List of IfcSimpleProperty</param>
        /// <param name="name">property name we want to extract</param>
        /// <returns></returns>
        public Interval GetPropertyEnumValue(List<IfcSimpleProperty> propertyList, string name)
        {
            var result = new Interval() { Value = DEFAULT_STRING, Unit = DEFAULT_STRING };
            var ifcPEValue = propertyList.OfType<IfcPropertyEnumeratedValue>().Where(psv => psv.Name == name).FirstOrDefault();
            if (ifcPEValue != null)
            {
                if (ifcPEValue.EnumerationValues != null)
                {
                    result.Value = GetEnumerationValues(ifcPEValue.EnumerationValues);
                }
                //get  the unit and all possible values held in the Enumeration
                if (ifcPEValue.EnumerationReference != null)
                {
                    if (ifcPEValue.EnumerationReference.Unit != null)
                    {
                        result.Unit = GetUnitName(ifcPEValue.EnumerationReference.Unit);
                    }
                    //string EnumValuesHeld = GetEnumerationValues(ifcPEValue.EnumerationReference.EnumerationValues);
                }
                
            }
            return result;
        }

        /// <summary>
        /// Check for empty, null of DEFAULT_STRING
        /// </summary>
        /// <param name="value">string to validate</param>
        /// <returns></returns>
        protected bool ValidateString(string value)
        {
            return (!((value == Constants.DEFAULT_STRING) || (string.IsNullOrEmpty(value)) || (value == "n\\a") || (value == "n\a"))); //"n\a" cover miss types
            //return ((!string.IsNullOrEmpty(value)) && (value != Constants.DEFAULT_STRING) && (value != "n\\a") && (value != "n\a")); //"n\a" cover miss types
        }


        #endregion

    }

    #region IComparer Classes
    /// <summary>
    /// ICompare class for IfcLabels, used to order by 
    /// </summary>
    public class CompareIfcLabel : IComparer<IfcLabel?>
    {
        public int Compare(IfcLabel? x, IfcLabel? y)
        {
            return string.Compare(x.ToString(), y.ToString(), true); //ignore case set to true
        }
    }

    /// <summary>
    /// ICompare class for String, used to order by 
    /// </summary>
    public class CompareString : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, true); //ignore case set to true
        }
    }

 

    #endregion
}
