using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xbim.IO.Esent;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBim
    {

        #region Fields
        private COBieProgress _progressStatus;
        #endregion

        #region Properties
        /// <summary>
        /// Context object holding Model, WorkBook etc...
        /// </summary>
        protected COBieXBimContext XBimContext { get; set; }

        /// <summary>
        /// Model from COBieXBimContext object
        /// </summary>
        public IfcStore Model
        {
            get { return XBimContext.Model; }
        } 
        /// <summary>
        /// World coordinate system from COBieXBimContext object
        /// </summary>
        public IIfcAxis2Placement3D WCS
        {
            get { return XBimContext.WCS; }
        }

        /// <summary>
        /// WorkBook from COBieXBimContext object
        /// </summary>
        public COBieWorkbook WorkBook
        {
            get { return XBimContext.WorkBook; }
        }

        /// <summary>
        /// Contacts from COBieXBimContext object
        /// </summary>
        public Dictionary<string, IIfcPersonAndOrganization> Contacts
        {
            get { return XBimContext.Contacts; }
        }

        protected COBieProgress ProgressIndicator
        {
            get
            {
                return _progressStatus;
            }
        }

        protected int TransBatchCount { get; set; }

        #endregion

        public COBieXBim(COBieXBimContext xBimContext)
        {
            XBimContext = xBimContext;
            _progressStatus = new COBieProgress(xBimContext);
            TransBatchCount = 100;
        }

        #region Methods

        /// <summary>
        /// Commit and restart the passed transaction
        /// </summary>
        /// <param name="trans">transaction to commit and restart start</param>
        /// <param name="number">number of instances in Model at start of transaction</param>
        protected void BumpTransaction(XbimReadWriteTransaction trans, long number)
        {
            if ((number % TransBatchCount) == 0)
            {
                trans.Commit();
                trans.Begin();
            }
        }
        /// <summary>
        /// Set a new owner history to the 
        /// </summary>
        /// <param name="ifcRoot">Object to add the owner history</param>
        /// <param name="externalSystem">Application used to modify/create</param>
        /// <param name="createdBy">IIfcPersonAndOrganization object</param>
        /// <param name="createdOn">Date the object was created on</param>
        protected void SetNewOwnerHistory(IIfcRoot ifcRoot, string externalSystem, IIfcPersonAndOrganization createdBy, string createdOn)
        { 
            ifcRoot.OwnerHistory = CreateOwnerHistory(null,  externalSystem, createdBy, createdOn);
        }

        /// <summary>
        /// Set an existing or create a new owner history if no match is found
        /// </summary>
        /// <param name="ifcRoot">Object to add the owner history</param>
        /// <param name="externalSystem">Application used to modify/create</param>
        /// <param name="createdBy">IIfcPersonAndOrganization object</param>
        /// <param name="createdOn">Date the object was created on</param>
        protected void SetOwnerHistory(IIfcRoot ifcRoot, string externalSystem, IIfcPersonAndOrganization createdBy, string createdOn)
        {
            IIfcTimeStamp? stamp = null;
            if (ValidateString(createdOn))
            {
                DateTime dateTime;
                if (DateTime.TryParse(createdOn, out dateTime))
                    stamp = IIfcTimeStamp.ToTimeStamp(dateTime);
            }
            var ifcOwnerHistory = Model.Instances.OfType<IIfcOwnerHistory>().Where(oh => (oh.CreationDate == stamp) &&
                                                                           (oh.OwningUser == createdBy) &&
                                                                           (((oh.OwningApplication != null) && (oh.OwningApplication.ApplicationFullName.ToString().ToLower() == externalSystem.ToLower()) )||
                                                                            ((oh.LastModifyingApplication != null) && (oh.LastModifyingApplication.ApplicationFullName.ToString().ToLower() == externalSystem.ToLower()))
                                                                           )
                                                                           ).FirstOrDefault();
            if (ifcOwnerHistory != null) 
                ifcRoot.OwnerHistory = ifcOwnerHistory;
            else
                ifcRoot.OwnerHistory = CreateOwnerHistory(ifcRoot, externalSystem, createdBy, createdOn);
        }
        /// <summary>
        /// Create the owner history
        /// </summary>
        /// <param name="ifcRoot">Object to add the owner history</param>
        /// <param name="externalSystem">Application used to modify/create</param>
        /// <param name="createdBy">IIfcPersonAndOrganization object</param>
        /// <param name="createdOn">Date the object was created on</param>
        protected IIfcOwnerHistory CreateOwnerHistory(IIfcRoot ifcRoot, string externalSystem, IIfcPersonAndOrganization createdBy, string createdOn)
        {
            IIfcApplication ifcApplication = null;
            IIfcOrganization ifcOrganization = null;
                
            if (ValidateString(externalSystem))
            {
                //ApplicationIdentifier is a required field so we need a value
                var applicationIdentifier = string.Empty;
                var externalSystemSplit = externalSystem.Split(' ');
                applicationIdentifier = externalSystemSplit.FirstOrDefault(); //assume first word in application can be used a identifier
                if (string.IsNullOrEmpty(applicationIdentifier))
                    applicationIdentifier = ""; //set a value as a required field

                //create an organization for the external system
                var orgName = externalSystem.Split(' ').FirstOrDefault();
                ifcOrganization = Model.Instances.OfType<IIfcOrganization>().Where(o => o.Name == orgName).FirstOrDefault();
                if (ifcOrganization == null)
                {
                    ifcOrganization = Model.Instances.New<IIfcOrganization>();
                    if (ValidateString(orgName))
                        ifcOrganization.Name = orgName;
                    else
                        ifcOrganization.Name = "Unknown"; //is not an optional field so fill with unknown value
                }
                ifcApplication = Model.Instances.OfType<IIfcApplication>().Where(app => app.ApplicationFullName == externalSystem).FirstOrDefault();
                if (ifcApplication == null)
                    ifcApplication = Model.Instances.New<IIfcApplication>(app =>
                    {
                        app.ApplicationFullName = externalSystem;
                        app.ApplicationDeveloper = ifcOrganization;
                        app.Version = new IIfcLabel("");
                        app.ApplicationIdentifier = new IIfcIdentifier(applicationIdentifier);
                    });

            }
            else
            { //Owner history OwningApplication is not an optional field so fill with unknown value
                ifcOrganization = Model.Instances.OfType<IIfcOrganization>().Where(o => o.Name == "").FirstOrDefault();
                if (ifcOrganization == null)
                {
                    ifcOrganization = Model.Instances.New<IIfcOrganization>();
                    ifcOrganization.Name = "Unknown"; //is not an optional field so fill with unknown value
                        
                }
                ifcApplication = Model.Instances.OfType<IIfcApplication>().Where(app => app.ApplicationFullName == "").FirstOrDefault();
                if (ifcApplication == null)
                    ifcApplication = Model.Instances.New<IIfcApplication>(app =>
                    {
                        app.ApplicationFullName = ""; //required field for all properties
                        app.ApplicationDeveloper = ifcOrganization;
                        app.Version = new IIfcLabel("");
                        app.ApplicationIdentifier = new IIfcIdentifier("");
                    });
            }

            IIfcTimeStamp stamp = new IIfcTimeStamp();
            if (ValidateString(createdOn))
            {
                DateTime dateTime;// = new DateTime(0, DateTimeKind.Utc);
                if (DateTime.TryParse(createdOn, out dateTime))
                {
                    stamp = IIfcTimeStamp.ToTimeStamp(dateTime);
                }
                else
                {
                    //try and get just the date part of the date time, as a conversion above failed, so assume time might be corrupt
                    var idx = createdOn.IndexOf("T");
                    if (idx > -1)
                    {
                        var date = createdOn.Substring(0, idx);
                        if (DateTime.TryParse(date, out dateTime))
                        {
                            stamp = IIfcTimeStamp.ToTimeStamp(dateTime);
                        }
                    }
                }
            }
            //if (createdBy == null)
            //{
            //    createdBy = Model.DefaultOwningUser;
            //}
            if ((ifcRoot != null) &&
                (ifcRoot.OwnerHistory != null)
                )
            {
                ifcRoot.OwnerHistory.OwningUser = createdBy;
                ifcRoot.OwnerHistory.OwningApplication = ifcApplication;
                ifcRoot.OwnerHistory.CreationDate = stamp;
                ifcRoot.OwnerHistory.ChangeAction = IIfcChangeActionEnum.MODIFIED;
                return ifcRoot.OwnerHistory;
            }
            else
                return Model.Instances.New<IIfcOwnerHistory>(oh =>
                                                {
                                                    oh.OwningUser = createdBy;
                                                    oh.OwningApplication = ifcApplication;
                                                    oh.LastModifyingApplication = ifcApplication;
                                                    oh.CreationDate = stamp;
                                                    oh.ChangeAction = IIfcChangeActionEnum.NOCHANGE;
                                                });

            
        }

        /// <summary>
        /// Set the user history to a IIfcRoot object
        /// </summary>
        /// <param name="ifcRoot">Object to set owner history</param>
        /// <param name="extSystem">External system string</param>
        /// <param name="createdBy">Email address of creator</param>
        /// <param name="createdOn">Created on date as string</param>
        protected void SetUserHistory(IIfcRoot ifcRoot, string extSystem, string createdBy, string createdOn)
        {
            if ((ValidateString(createdBy)) && (Contacts.ContainsKey(createdBy)))
                SetNewOwnerHistory(ifcRoot, extSystem, Contacts[createdBy], createdOn);
            else if (ValidateString(createdBy))
                SetNewOwnerHistory(ifcRoot, extSystem, SetEmailUser(createdBy), createdOn); //has a valid string, so lets pass string back, will be validated in XLS
            else
                SetNewOwnerHistory(ifcRoot, extSystem, SetEmailUser(Constants.DEFAULT_EMAIL), createdOn); //nothing in field so set to default
        }


        /// <summary>
        /// Set the IIfcPersonAndOrganization from using email only
        /// </summary>
        /// <param name="email">email</param>
        protected IIfcPersonAndOrganization SetEmailUser(string email)
        {
            if (!Contacts.ContainsKey(email)) //should filter on merge also unless Contacts is reset
            {
                var ifcPerson = Model.Instances.New<IIfcPerson>();
                var ifcOrganization = Model.Instances.New<IIfcOrganization>();
                var ifcPersonAndOrganization = Model.Instances.New<IIfcPersonAndOrganization>();
            
                Contacts.Add(email, ifcPersonAndOrganization); //build a list to reference for History objects

                //add email
                var ifcTelecomAddress = Model.Instances.New<IIfcTelecomAddress>();
                if (ValidateString(email))
                {
                    ifcTelecomAddress.ElectronicMailAddresses.Add(email); //add to existing collection
                }
                //add Company
                ifcOrganization.Name = "Unknown"; //is not an optional field so fill with unknown value

                var ifcPostalAddress = Model.Instances.New<IIfcPostalAddress>();
                //add Telecom Address
                ifcPerson.Addresses.Add(ifcTelecomAddress);//add to existing collection
                // Add blank postal address
                ifcPerson.Addresses.Add(ifcPostalAddress);//add to existing collection

                //add the person and the organization objects 
                ifcPersonAndOrganization.ThePerson = ifcPerson;
                ifcPersonAndOrganization.TheOrganization = ifcOrganization;
                return ifcPersonAndOrganization;
            }
            else
                return Contacts[email];
        }

        /// <summary>
        /// Check for empty, null of DEFAULT_STRING
        /// </summary>
        /// <param name="value">string to validate</param>
        /// <returns></returns>
        protected bool ValidateString(string value)
        {
            return ((!string.IsNullOrEmpty(value)) && (value != Constants.DEFAULT_STRING) && (value != "n\\a") && (value != "n\a")); //"n\a" cover miss types
        }

        /// <summary>
        /// Get the IIfcBuilding Object
        /// </summary>
        /// <returns>IIfcBuilding Object</returns>
        protected IIfcBuilding GetBuilding()
        {
            var ifcBuildings = Model.Instances.OfType<IIfcBuilding>();
            if ((ifcBuildings.Count() > 1) || (ifcBuildings.Count() == 0))
                throw new Exception(string.Format("COBieXBimSerialiser: Expecting one IIfcBuilding, Found {0}", ifcBuildings.Count().ToString()));
            return ifcBuildings.FirstOrDefault();
        }
        /// <summary>
        /// Get the IIfcSite Object
        /// </summary>
        /// <returns>IIfcSite Object</returns>
        protected IIfcSite GetSite()
        {
            var ifcSites = Model.Instances.OfType<IIfcSite>();
            if ((ifcSites.Count() > 1) || (ifcSites.Count() == 0))
                throw new Exception(string.Format("COBieXBimSerialiser: Expecting one IIfcSite, Found {0}", ifcSites.Count().ToString()));
            return ifcSites.FirstOrDefault();
        }
        /// <summary>
        /// Convert a String to a Double
        /// </summary>
        /// <param name="num">string to convert</param>
        /// <returns>double or null</returns>
        protected double? GetDoubleFromString(string num)
        {
            double test;
            if (double.TryParse(num, out test))
                return test;
            else
                return null;
        }

        protected IIfcBuildingStorey GetBuildingStory (string name)
        {
            var ifcBuildingStoreys = Model.Instances.OfType<IIfcBuildingStorey>().Where(bs => bs.Name == name);
            if ((ifcBuildingStoreys.Count() > 1) || (ifcBuildingStoreys.Count() == 0))
                throw new Exception(string.Format("COBieXBimSerialiser: Expecting one IIfcBuildingStorey with name of {1}, Found {0}", ifcBuildingStoreys.Count().ToString(), name));
            return ifcBuildingStoreys.FirstOrDefault();
        }

        /// <summary>
        /// Add Category via the IIfcRelAssociatesClassification object
        /// </summary>
        /// <param name="category">Category for this IIfcRoot Object</param>
        /// <param name="ifcRoot">IIfcRoot derived object to all category to</param>
        protected void AddCategory(string category, IIfcRoot ifcRoot)
        {
            if (ValidateString(category))
            {
                var itemReference = "";
                var name = "";
                var splitCategory = category.Split(':');
                //see if we have a split category name like "11-11: Assembly Facilities"
                if (splitCategory.Count() == 2)
                {
                    itemReference = splitCategory.First().Trim();
                    name = splitCategory.Last().Trim();
                }
                else
                    name = category.Trim();

            
                //check to see if we have a IIfcRelAssociatesClassification associated with this category
                var ifcRelAssociatesClassification = Model.Instances.OfType<IIfcRelAssociatesClassification>()
                                    .Where(rac => rac.RelatingClassification != null 
                                        && (rac.RelatingClassification is IIfcClassificationReference)
                                        && (   ((((IIfcClassificationReference)rac.RelatingClassification).ItemReference.ToString().ToLower() == itemReference.ToLower())
                                            &&  (((IIfcClassificationReference)rac.RelatingClassification).Name.ToString().ToLower() == name.ToLower())
                                               )
                                            || (((IIfcClassificationReference)rac.RelatingClassification).Location.ToString().ToLower() == category.ToLower())
                                            )
                                        )
                                    .FirstOrDefault();
                if (ifcRelAssociatesClassification == null)
                {
                    //check we have a IIfcClassificationReference holding the category
                    var ifcClassificationReference = Model.Instances.OfType<IIfcClassificationReference>()
                                    .Where(cr =>(  (cr.ItemReference.ToString().ToLower() == itemReference.ToLower())
                                                && (cr.Name.ToString().ToLower() == name.ToLower())
                                                )
                                            || (cr.Location.ToString().ToLower() == category.ToLower())
                                          )
                                    .FirstOrDefault();

                    if (ifcClassificationReference == null) //create if required
                        ifcClassificationReference = Model.Instances.New<IIfcClassificationReference>(ifcCR => { ifcCR.ItemReference = itemReference; ifcCR.Name = name; ifcCR.Location = category; });
                    //create new IIfcRelAssociatesClassification holding a IIfcClassificationReference with Location set to the category value
                    ifcRelAssociatesClassification = Model.Instances.New<IIfcRelAssociatesClassification>(ifcRAC => { ifcRAC.RelatingClassification = ifcClassificationReference; ifcRAC.RelatedObjects.Add(ifcRoot); });
                }
                else
                {
                    //we have a IIfcRelAssociatesClassification so just add this IIfcRoot object to the RelatedObjects collection
                    ifcRelAssociatesClassification.RelatedObjects.Add(ifcRoot);
                }
            }
        }

        /// <summary>
        /// Add GlobalId to the IIfcRoot Object
        /// </summary>
        /// <param name="extId">string representing the global Id</param>
        /// <param name="ifcRoot">IIfcRoot derived object to add GlobalId too</param>
        protected void AddGlobalId(string extId, IIfcRoot ifcRoot)
        {
            if (ValidateString(extId))
            {
                var id = new IIfcGloballyUniqueId(extId);
                ifcRoot.GlobalId = id;
            }
            else //if null passed then create a new Guid
            {
                ifcRoot.GlobalId = new IIfcGloballyUniqueId(Guid.NewGuid());
            }
        }

        /// <summary>
        /// Validate the string as a GlobalId
        /// </summary>
        /// <param name="extId">string representation of the GlobalId</param>
        /// <returns>bool</returns>
        public static bool ValidGlobalId(string extId)
        {

            Guid guid;
            var isGuid = IIfcGloballyUniqueId.TryConvertFromBase64(extId, out guid);
            if (!isGuid) return false;
            var guidStr = IIfcGloballyUniqueId.ConvertToBase64(guid);
            if (extId != guidStr) return false;
            else return true;
        }

        /// <summary>
        /// Set or change a Property Set description value
        /// </summary>
        /// <param name="obj">Object holding the property</param>
        /// <param name="pSetName">Property set name</param>
        /// <param name="pSetDescription">Property set description</param>
        private static void SetPropertySetDescription(IIfcObject obj, string pSetName, string pSetDescription)
        {
            if (!string.IsNullOrEmpty(pSetDescription))
            {
            var ifcRelDefinesByProperties = obj.IsDefinedByProperties.FirstOrDefault(r => r.RelatingPropertyDefinition.Name == pSetName);
            if (ifcRelDefinesByProperties != null)
                ifcRelDefinesByProperties.RelatingPropertyDefinition.Description = pSetDescription;
            }
        }

        /// <summary>
        /// Add a property single value
        /// </summary>
        /// <param name="ifcObject">Object to add property too</param>
        /// <param name="pSetName">Property set name to add property single value too</param>
        /// <param name="pSetDescription">Property set description or null to leave unaltered</param>
        /// <param name="propertyName">Property single value name</param>
        /// <param name="propertyDescription">Property single value description or null to not set</param>
        /// <param name="value">IIfcValue select type to set on property single value</param>
        protected IIfcPropertySingleValue AddPropertySingleValue(IIfcObject ifcObject, string pSetName, string pSetDescription, string propertyName, string propertyDescription, IIfcValue value)
        {
            if (string.IsNullOrEmpty(propertyName)) //required value on IIfcPropertySingleValue
                if ((value != null) && (!string.IsNullOrEmpty(value.ToString())))
                    propertyName = value.ToString();
                else
                    propertyName = "NoName";
            
            var ifcPropertySingleValue = ifcObject.SetPropertySingleValue(pSetName, propertyName, value);
            if (!string.IsNullOrEmpty(propertyDescription))
                ifcPropertySingleValue.Description = propertyDescription; //description to property Single Value
            //set description for the property set, nice to have but optional
            if (!string.IsNullOrEmpty(pSetDescription))
            SetPropertySetDescription(ifcObject, pSetName, pSetDescription);
            return ifcPropertySingleValue;
        }

        /// <summary>
        /// Add a property set or use existing if it exists
        /// </summary>
        /// <param name="ifcObject">IIfcObject to add property set too</param>
        /// <param name="pSetName">name of the property set</param>
        protected IIfcPropertySet AddPropertySet(IIfcObject ifcObject, string pSetName, string pSetDescription)
        {
            var ifcPropertySet = ifcObject.GetPropertySet(pSetName);
            if (ifcPropertySet == null)
            {
                ifcPropertySet = Model.Instances.New<IIfcPropertySet>();
                ifcPropertySet.Name = pSetName;
                ifcPropertySet.Description = pSetDescription;
                //set relationship
                var ifcRelDefinesByProperties = Model.Instances.New<IIfcRelDefinesByProperties>();
                ifcRelDefinesByProperties.RelatingPropertyDefinition = ifcPropertySet;
                ifcRelDefinesByProperties.RelatedObjects.Add(ifcObject);
            }
            return ifcPropertySet;
        }

        /// <summary>
        /// Add a property set or use existing if it exists
        /// </summary>
        /// <param name="ifcObject">IIfcObject to add property set too</param>
        /// <param name="pSetName">name of the property set</param>
        protected IIfcPropertySet AddPropertySet(IIfcTypeObject ifcObject, string pSetName, string pSetDescription)
        {
            var ifcPropertySet = ifcObject.GetPropertySet(pSetName);
            if (ifcPropertySet == null)
            {
                ifcPropertySet = Model.Instances.New<IIfcPropertySet>();
                ifcPropertySet.Name = pSetName;
                ifcPropertySet.Description = pSetDescription;
                //add to type object property set list
                ifcObject.AddPropertySet(ifcPropertySet);
            }
            return ifcPropertySet;
        }
        /// <summary>
        /// Add a property single value or use existing if it exists
        /// </summary>
        /// <param name="ifcPropertySet">IIfcPropertySet object</param>
        /// <param name="propertyName">Single Value Property Name</param>
        /// <param name="propertyDescription">Single Value Property Description</param>
        /// <param name="value">IIfcValue object</param>
        protected IIfcPropertySingleValue AddPropertySingleValue(IIfcPropertySet ifcPropertySet, string propertyName, string propertyDescription, IIfcValue value, IIfcUnit unit)
        {
            //see if we have this property single value, if so overwrite the value
            var ifcPropertySingleValue = ifcPropertySet.HasProperties.Where<IIfcPropertySingleValue>(p => p.Name == propertyName).FirstOrDefault();
            if (ifcPropertySingleValue == null)
            {
                if (string.IsNullOrEmpty(propertyName))//required value on IIfcPropertySingleValue
                {
                    if ((value != null) && (!string.IsNullOrEmpty(value.ToString())))
                        propertyName = value.ToString();
                    else
                        propertyName = "";
                    
                }
                ifcPropertySingleValue = Model.Instances.New<IIfcPropertySingleValue>(psv => { psv.Name = propertyName; psv.Description = propertyDescription;  });
            }
            ifcPropertySingleValue.NominalValue = value;
            ifcPropertySingleValue.Unit = unit;
            //add to property set
            ifcPropertySet.HasProperties.Add(ifcPropertySingleValue);
            return ifcPropertySingleValue;
        }

        /// <summary>
        /// Add a new Property Enumerated Value or use existing
        /// </summary>
        /// <param name="ifcPropertySet">IIfcPropertySet object</param>
        /// <param name="propertyName">Property Enumerated Value Name</param>
        /// <param name="propertyDescription">Property Enumerated Value Description</param>
        /// <param name="values">Property Enumerated Value list of values</param>
        /// <param name="enumValues">Property Enumerated Value possible enumeration values</param>
        /// <param name="unit">Unit for the enumValues values</param>
        /// <returns></returns>
        protected IIfcPropertyEnumeratedValue AddPropertyEnumeratedValue(IIfcPropertySet ifcPropertySet, string propertyName, string propertyDescription, IIfcValue[] values, IIfcValue[] enumValues, IIfcUnit unit)
        {
            var ifcPropertyEnumeratedValue = ifcPropertySet.HasProperties.Where<IIfcPropertyEnumeratedValue>(p => p.Name == propertyName).FirstOrDefault();

            if (ifcPropertyEnumeratedValue != null)
            {
                ifcPropertyEnumeratedValue.EnumerationValues.Clear();
                ifcPropertyEnumeratedValue.EnumerationReference.EnumerationValues.Clear();
            }
            else
            {
                ifcPropertyEnumeratedValue = Model.Instances.New<IIfcPropertyEnumeratedValue>();
                ifcPropertyEnumeratedValue.EnumerationReference = Model.Instances.New<IIfcPropertyEnumeration>();
                ifcPropertyEnumeratedValue.EnumerationReference.Name = "";
            }
            //fill values
            if (string.IsNullOrEmpty(propertyName))
                ifcPropertyEnumeratedValue.Name = "";
            else
                ifcPropertyEnumeratedValue.Name = propertyName;
            
            
            if (!string.IsNullOrEmpty(propertyDescription))
            {
                 ifcPropertyEnumeratedValue.Description = propertyDescription;
            }
           
            foreach (var ifcValue in values)
            {
                ifcPropertyEnumeratedValue.EnumerationValues.Add(ifcValue);
            }
            foreach (var ifcValue in enumValues)
            {
                ifcPropertyEnumeratedValue.EnumerationReference.EnumerationValues.Add(ifcValue);
            }
            //add unit
            if (unit != null)
            {
                ifcPropertyEnumeratedValue.EnumerationReference.Unit = unit;
            }

            //add to property set
            ifcPropertySet.HasProperties.Add(ifcPropertyEnumeratedValue);
            return ifcPropertyEnumeratedValue;
        }
        /// <summary>
        /// Add a new Property Table Value or use existing
        /// </summary>
        /// <param name="ifcPropertySet"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyDescription"></param>
        /// <param name="values">delimited string of paired values(xxx:xxx)(xxx:xxx)....(xxx:xxx)</param>
        /// <param name="expression">string</param>
        /// <param name="unit">delimited string of paired unit values xxx:xxx</param>
        /// <returns>IIfcPropertyTableValue</returns>
        protected IIfcPropertyTableValue AddPropertyTableValue(IIfcPropertySet ifcPropertySet, string propertyName, string propertyDescription, string values, string expression, string unit)
        {
            var ifcPropertyTableValue = ifcPropertySet.HasProperties.Where<IIfcPropertyTableValue>(p => p.Name == propertyName).FirstOrDefault();

            if (ifcPropertyTableValue != null)
            {
                ifcPropertyTableValue.DefiningValues.Clear();
                ifcPropertyTableValue.DefinedValues.Clear();
            }
            else
            {
                ifcPropertyTableValue = Model.Instances.New<IIfcPropertyTableValue>();
            }

            //fill values
            if (string.IsNullOrEmpty(propertyName))
                ifcPropertyTableValue.Name = "";
            else
                ifcPropertyTableValue.Name = propertyName;
            
            if (!string.IsNullOrEmpty(expression))
            {
                ifcPropertyTableValue.Expression = expression;
            }

            if (!string.IsNullOrEmpty(propertyDescription))
            {
                ifcPropertyTableValue.Description = propertyDescription;
            }
            
            values = values.Replace("(", ""); //remove the start brackets
            var valueArray = values.Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in valueArray)
            {
                var ifcValues = GetValueArray(item);
                ifcPropertyTableValue.DefiningValues.Add(ifcValues.First());
                ifcPropertyTableValue.DefinedValues.Add(ifcValues.Last());
            }
            //add unit
            if (ValidateString(unit))
            {
                var strValues = unit.Split(':');
                var definingUnit = GetIIfcUnit(strValues.First());
                var definedUnit = GetIIfcUnit(strValues.Last());

                if (definingUnit != null)
                    ifcPropertyTableValue.DefiningUnit = definingUnit;

                if (definedUnit != null)
                    ifcPropertyTableValue.DefinedUnit = definedUnit;
            }
            

            //add to property set
            ifcPropertySet.HasProperties.Add(ifcPropertyTableValue);
            return ifcPropertyTableValue;
        }

        /// <summary>
        /// create a IIfcValue array from a delimited string, either "," or ":" delimited
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>IIfcValue[]</returns>
        public IIfcValue[] GetValueArray(string value)
        {
            var splitKey = GetSplitChar(value);
            double number;
            IIfcValue[] ifcValues;
            var strValues = value.Split(splitKey);
            ifcValues = new IIfcValue[strValues.Length];
            for (var i = 0; i < strValues.Length; i++)
            {
                var str = strValues[i].Trim();//.Replace(" ", ""); //enumeration so remove spaces, now assume user entered correctly
                if (double.TryParse(str, out number))
                    ifcValues[i] = new IIfcReal((double)number);
                else
                    ifcValues[i] = new IIfcLabel(str);
            }
            return ifcValues;
        }

        /// <summary>
        /// Convert a string into an IIfcUnit
        /// </summary>
        /// <param name="unit">unit</param>
        /// <returns>IIfcUnit or null on fail</returns>
        protected IIfcUnit GetIIfcUnit(string unit)
        {
            IIfcUnit ifcUnit = null;
            if (ValidateString(unit))
            {
                ifcUnit = GetDurationUnit(unit); //see if time unit
                //see if we can convert to a IIfcSIUnit
                if (ifcUnit == null)
                    ifcUnit = GetSIUnit(unit);
                //OK set as a user defined
                if (ifcUnit == null)
                    ifcUnit = SetContextDependentUnit(unit);
            }
            return ifcUnit;
        }
        /// <summary>
        /// Get the most likely character which splits the line
        /// </summary>
        /// <param name="value">delimited string</param>
        /// <returns>split character</returns>
        public static char GetSplitChar(string value)
        {
            //swapped these on 28 Nov 2012, might cause problem, should see in testing
            var splitKey = ':'; //contained within names so give "," a higher likelihood
            if (value.Contains(",")) 
                splitKey = ',';
            return splitKey;
        }

        /// <summary>
        /// Split the string 
        /// </summary>
        /// <param name="str">string to split via a ":" or ","</param>
        /// <returns>string array</returns>
        public List<string> SplitTheString(string str)
        {
            if (ValidateString(str))
            {
                var splitKey = GetSplitChar(str);
                return SplitString(str, splitKey);
            }
            else
                return new List<string>();
            
        }

        /// <summary>
        /// Set or change a Property Set description value
        /// </summary>
        /// <param name="obj">IIfcTypeObject holding the property</param>
        /// <param name="pSetName">Property set name</param>
        /// <param name="pSetDescription">Property set description</param>
        private void SetPropertySetDescription(IIfcTypeObject obj, string pSetName, string pSetDescription)
        {
            if (!string.IsNullOrEmpty(pSetDescription))
            {
                var ifcPropertySet = obj.HasPropertySets.OfType<IIfcPropertySet>().Where(r => r.Name == pSetName).FirstOrDefault();
                if (ifcPropertySet != null)
                    ifcPropertySet.Description = pSetDescription;
            }
        }

        /// <summary>
        /// Add a property single value
        /// </summary>
        /// <param name="IIfcTypeObject">Object to add property too</param>
        /// <param name="pSetName">Property set name to add property single value too</param>
        /// <param name="pSetDescription">Property set description or null to leave unaltered</param>
        /// <param name="propertyName">Property single value name</param>
        /// <param name="propertyDescription">Property single value description or null to not set</param>
        /// <param name="value">IIfcValue select type to set on property single value</param>
        protected IIfcPropertySingleValue AddPropertySingleValue(IIfcTypeObject ifcObject, string pSetName, string pSetDescription, string propertyName, string propertyDescription, IIfcValue value)
        {
            if (string.IsNullOrEmpty(propertyName))//required value on IIfcPropertySingleValue
            {
                if ((value != null) && (!string.IsNullOrEmpty(value.ToString())))
                    propertyName = value.ToString();
                else
                    propertyName = "";

            }
            var ifcPropertySingleValue = ifcObject.SetPropertySingleValue(pSetName, propertyName, value);
            if (!string.IsNullOrEmpty(propertyDescription))
                ifcPropertySingleValue.Description = propertyDescription; //description to property Single Value
            //set description for the property set, nice to have but optional
            if (!string.IsNullOrEmpty(pSetDescription))
                SetPropertySetDescription(ifcObject, pSetName, pSetDescription);
            return ifcPropertySingleValue;
        }

        /// <summary>
        /// Get the Duration unit, default year on no match
        /// </summary>
        /// <param name="requiredUnit">string of year, month or week</param>
        protected IIfcConversionBasedUnit GetDurationUnit(string requiredUnit)
        {
            switch (requiredUnit.ToLower())
            {
                case "year":
                    return XBimContext.IIfcConversionBasedUnitYear;
                    
                case "month":
                    return XBimContext.IIfcConversionBasedUnitMonth;
                   
                case "week":
                    return XBimContext.IIfcConversionBasedUnitWeek;

                case "minute":
                    return XBimContext.IIfcConversionBasedUnitMinute;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Create a user defined unit via IIfcContextDependentUnit
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        protected IIfcContextDependentUnit SetContextDependentUnit(string unitName)
        {
            return Model.Instances.New<IIfcContextDependentUnit>(cdu => 
            {
                cdu.Name = unitName;
                cdu.UnitType = IfcUnitEnum.USERDEFINED;
                cdu.Dimensions = XBimContext.DimensionalExponentSingleUnit;
            });
        }

        protected IIfcSIUnit GetSIUnit(string value)
        {
            IfcSIUnitName? returnUnit;
            IfcSIPrefix? returnPrefix;
            IIfcSIUnit ifcSIUnit = null;

            if (GetUnitEnumerations(value, out returnUnit, out returnPrefix))
            {
                IEnumerable<IIfcSIUnit> ifcSIUnits;
                if (returnPrefix != null)
                {
                    ifcSIUnits = Model.Instances.Where<IIfcSIUnit>(siu => (siu.Name == (IfcSIUnitName)returnUnit) && (siu.Prefix == (IfcSIPrefix)returnPrefix));
                }
                else
                {
                    ifcSIUnits = Model.Instances.Where<IIfcSIUnit>(siu => siu.Name == (IfcSIUnitName)returnUnit && siu.Prefix == null);
                }
                
                if (ifcSIUnits.Any())
                {
                    ifcSIUnit = ifcSIUnits.FirstOrDefault();
                }
                else
                {
                    var IfcUnitEnum = MappingUnitType((IfcSIUnitName)returnUnit); //get the unit type for the IfcSIUnitName
                    ifcSIUnit = Model.Instances.New<IIfcSIUnit>(si =>
                                            {
                                                si.UnitType = IfcUnitEnum;
                                                si.Name = (IfcSIUnitName)returnUnit;
                                            });
                    if (returnPrefix != null)
                    {
                        ifcSIUnit.Prefix = (IfcSIPrefix)returnPrefix;
                    }
                }
            }
            return ifcSIUnit;
        }
        
        private IfcUnitEnum MappingUnitType (IfcSIUnitName ifcSIUnitName)
        {
            switch (ifcSIUnitName)
            {
                case IfcSIUnitName.AMPERE:
                    return IfcUnitEnum.ELECTRICCURRENTUNIT;
                case IfcSIUnitName.BECQUEREL:
                    return IfcUnitEnum.RADIOACTIVITYUNIT;
                case IfcSIUnitName.CANDELA:
                    return IfcUnitEnum.LUMINOUSINTENSITYUNIT;
                case IfcSIUnitName.COULOMB:
                    return IfcUnitEnum.ELECTRICCHARGEUNIT;
                case IfcSIUnitName.CUBIC_METRE:
                    return IfcUnitEnum.VOLUMEUNIT;
                case IfcSIUnitName.DEGREE_CELSIUS:
                    return IfcUnitEnum.THERMODYNAMICTEMPERATUREUNIT;
                case IfcSIUnitName.FARAD:
                    return IfcUnitEnum.ELECTRICCAPACITANCEUNIT;
                case IfcSIUnitName.GRAM:
                    return IfcUnitEnum.MASSUNIT;
                case IfcSIUnitName.GRAY:
                    return IfcUnitEnum.ABSORBEDDOSEUNIT;
                case IfcSIUnitName.HENRY:
                    return IfcUnitEnum.INDUCTANCEUNIT;
                case IfcSIUnitName.HERTZ:
                    return IfcUnitEnum.FREQUENCYUNIT;
                case IfcSIUnitName.JOULE:
                    return IfcUnitEnum.ENERGYUNIT;
                case IfcSIUnitName.KELVIN:
                    return IfcUnitEnum.THERMODYNAMICTEMPERATUREUNIT;
                case IfcSIUnitName.LUMEN:
                    return IfcUnitEnum.LUMINOUSFLUXUNIT;
                case IfcSIUnitName.LUX:
                    return IfcUnitEnum.ILLUMINANCEUNIT;
                case IfcSIUnitName.METRE:
                    return IfcUnitEnum.LENGTHUNIT;
                case IfcSIUnitName.MOLE:
                    return IfcUnitEnum.AMOUNTOFSUBSTANCEUNIT;
                case IfcSIUnitName.NEWTON:
                    return IfcUnitEnum.FORCEUNIT;
                case IfcSIUnitName.OHM:
                    return IfcUnitEnum.ELECTRICRESISTANCEUNIT;
                case IfcSIUnitName.PASCAL:
                    return IfcUnitEnum.PRESSUREUNIT;
                case IfcSIUnitName.RADIAN:
                    return IfcUnitEnum.PLANEANGLEUNIT;
                case IfcSIUnitName.SECOND:
                    return IfcUnitEnum.TIMEUNIT;
                case IfcSIUnitName.SIEMENS:
                    return IfcUnitEnum.ELECTRICCONDUCTANCEUNIT;
                case IfcSIUnitName.SIEVERT:
                    return IfcUnitEnum.RADIOACTIVITYUNIT;
                case IfcSIUnitName.SQUARE_METRE:
                    return IfcUnitEnum.AREAUNIT;
                case IfcSIUnitName.STERADIAN:
                    return IfcUnitEnum.SOLIDANGLEUNIT;
                case IfcSIUnitName.TESLA:
                    return IfcUnitEnum.MAGNETICFLUXDENSITYUNIT;
                case IfcSIUnitName.VOLT:
                    return IfcUnitEnum.ELECTRICVOLTAGEUNIT;
                case IfcSIUnitName.WATT:
                    return IfcUnitEnum.POWERUNIT;
                case IfcSIUnitName.WEBER:
                    return IfcUnitEnum.MAGNETICFLUXUNIT;
                default:
                    return IfcUnitEnum.USERDEFINED;
                   
            }
        

        }
        /// <summary>
        /// See if a string can be converted to the IfcSIUnitName / IfcSIPrefix combination
        /// </summary>
        /// <param name="value">string to evaluate</param>
        /// <param name="returnUnit">IfcSIUnitName? object to pass found value out</param>
        /// <param name="returnPrefix">IfcSIPrefix? object to pass found value out</param>
        /// <returns>bool, success or failed</returns>
        protected bool GetUnitEnumerations(string value, out IfcSIUnitName? returnUnit, out IfcSIPrefix? returnPrefix)
        {
            value = value.ToLower();
            if (value.Contains("meter")) value = value.Replace("meter", "metre");
            //remove the last letter if 's', but DEGREE_CELSIUS in IfcSIUnitName so do not do removal for CELSIUS
            if ((!value.Contains("celsius")) && (value.Last() == 's'))
            {
                value = value.Substring(0, (value.Length - 1));
            }


            var sqText = "";
            var baseUnit = "";
            var testPrefix = "";

            var prefixUnit = "";
            var unitName = "";

            var ifcSIUnitNames = Enum.GetNames(typeof(IfcSIUnitName));
            var ifcSIPrefixs = Enum.GetNames(typeof(IfcSIPrefix));
            //check for "_" ifcSIUnitName held in the value
            foreach (var name in ifcSIUnitNames)
            {
                if (name.Contains("_"))
                {
                    //see COBieData.GetUnitName method for the setting of the names via the enum's
                    var split = name.Split('_');   //see if _ delimited value such as SQUARE_METRE
                    if (split.Length > 1) sqText = split.First().ToLower();
                    baseUnit = split.Last().ToLower();
                    if (value.Contains(sqText) && value.Contains(baseUnit))
                    {
                        unitName = name; //we used an underscore to set the name
                        break;
                    }
                    else
                    {
                        sqText = "";
                        baseUnit = "";
                    }
                }

            }
            //see if we had prefixes in the string name
            foreach (var prefix in ifcSIPrefixs)
            {
                testPrefix = sqText + prefix; //add the front end of any "_" unit name from above, or default will be ""
                if ((value.Length >= testPrefix.Length) && (testPrefix.ToLower() == value.Substring(0, testPrefix.Length))) //see if the front end matched
                {
                    prefixUnit = prefix;
                    break;
                }

            }
            if (string.IsNullOrEmpty(unitName)) //no "_" unit name was used so lets test for the rest
            {
                foreach (var name in ifcSIUnitNames)
                {
                    if ((!name.Contains("_")) && //skip under scores
                        value.Contains(name.ToLower()) && //holds the text, but two value that conflict are STERADIAN  and RADIAN  so
                        (name.Length == (value.Length - prefixUnit.Length)) //check length by adding any prefix(default ""), we know no underscore so no need to add sqText value
                        )
                    {
                        unitName = name;
                        break;
                    }
                }
            }

            //convert the strings to the enumeration type
            IfcSIUnitName ifcSIUnitName;
            if (Enum.TryParse(unitName.ToUpper(), out ifcSIUnitName))
                returnUnit = ifcSIUnitName;
            else
                returnUnit = null; //we need a value


            IfcSIPrefix ifcSIPrefix;
            if (Enum.TryParse(prefixUnit.ToUpper(), out ifcSIPrefix))
                returnPrefix = ifcSIPrefix;
            else
                returnPrefix = null;
            return (returnUnit != null);
        }

        

        /// <summary>
        /// Determined the sheet the IIfcRoot will have come from using the object type
        /// </summary>
        /// <param name="ifcItem">object which inherits from IIfcRoot </param>
        /// <returns>string holding sheet name</returns>
        public IIfcRoot GetRootObject(string sheetName, string rowName)
        {
            //ensure we have worksheet name format 
            sheetName = sheetName.ToLower().Trim();
            sheetName = char.ToUpper(sheetName[0]) + sheetName.Substring(1);
            var objName = rowName;
            rowName = rowName.ToLower().Trim();
            switch (sheetName)
            {
                case Constants.WORKSHEET_TYPE:
                    var ifcTypeObject = Model.Instances.Where<IIfcTypeObject>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                    //if no Type Object create one to maintain relationship
                    if (ifcTypeObject == null)
                    {
                        ifcTypeObject = Model.Instances.New<IIfcBuildingElementProxyType>();
                        ifcTypeObject.Name = objName;
                    }
                    return ifcTypeObject;
                case Constants.WORKSHEET_COMPONENT:
                    var ifcElement = Model.Instances.Where<IIfcElement>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                    //if no Element Object create one to maintain relationship
                    if (ifcElement == null)
                    {
                        ifcElement = Model.Instances.New<IIfcVirtualElement>();
                        ifcElement.Name = objName;
                    }
                    return ifcElement;
                case Constants.WORKSHEET_JOB:
                    return Model.Instances.Where<IIfcProcess>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_ASSEMBLY:
                    return Model.Instances.Where<IIfcRelDecomposes>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_CONNECTION:
                    return Model.Instances.Where<IIfcRelConnectsElements>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_FACILITY:
                    IIfcRoot ifcRoot = Model.Instances.Where<IIfcSite>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                    if (ifcRoot == null)
                        ifcRoot = Model.Instances.Where<IIfcBuilding>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                    if (ifcRoot == null)
                        ifcRoot = Model.Instances.Where<IIfcProject>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
	                return ifcRoot;
                case Constants.WORKSHEET_FLOOR:
                    return Model.Instances.Where<IIfcBuildingStorey>(to => to.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_RESOURCE:
                    return Model.Instances.Where<IIfcConstructionEquipmentResource>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_SPACE:
                    return Model.Instances.Where<IIfcSpace>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_SPARE:
                    return Model.Instances.Where<IIfcConstructionProductResource>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_SYSTEM:
                    return Model.Instances.Where<IIfcGroup>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                case Constants.WORKSHEET_ZONE:
                    return Model.Instances.Where<IIfcZone>(obj => obj.Name.ToString().ToLower().Trim() == rowName).FirstOrDefault();
                default:
                    return null;
                    
            }
        }

        /// <summary>
        /// Get the ActorSelect fro the given email
        /// </summary>
        /// <param name="email">string</param>
        /// <returns>IIfcActorSelect object</returns>
        public IIfcActorSelect GetActorSelect(string email)
        {
            //srl the following fixes errors in the logic  and are more performant
            var ifcActorSelect = (Model.Instances.OfType<IIfcPersonAndOrganization>().FirstOrDefault<IIfcActorSelect>(a => a.HasEmail(email)) ??
                                  Model.Instances.OfType<IIfcOrganization>().FirstOrDefault<IIfcActorSelect>(a => a.HasEmail(email))) ??
                                 Model.Instances.OfType<IIfcPerson>().FirstOrDefault<IIfcActorSelect>(a => a.HasEmail(email));
            return ifcActorSelect;
        }


        /// <summary>
        /// Join string list into a delimited string, but escape any character that is within an added string which is also the delimited character
        /// </summary>
        /// <param name="strings">list of strings</param>
        /// <param name="splitChar">delimited character</param>
        /// <returns></returns>
        public static string JoinStrings (char splitChar, List<string> strings)
        {
            var sb = new StringBuilder();
            var split_Char = splitChar.ToString();
            var i = 1;
            foreach (var str in strings)
            {
                sb.Append(str.Replace(split_Char, "\\" + split_Char));
                if (i < strings.Count)
                {
                    sb.Append(" ");//place space around delimiter
                    sb.Append(splitChar);
                    sb.Append(" ");
                }
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Split a string but also remove any escape characters used to preserve the delimited character within a string
        /// </summary>
        /// <param name="str">string to split</param>
        /// <param name="splitChar">Delimited character to use</param>
        /// <returns></returns>
        public static List<string> SplitString(string str, char splitChar)
        {
            var strings = new List<string>();
            var sb = new StringBuilder();
            var isEscaped = false;
            foreach (var c in str)
            {
                if (c == '\\')
                {
                    isEscaped = true;
                }
                else if ((c == splitChar) && !isEscaped)
                {
                    strings.Add(sb.ToString().Trim());
                    sb.Clear();
                    isEscaped = false;
                }
                else
                {
                    sb.Append(c);
                    isEscaped = false;
                }
            }
            //pick up last string
            if (sb.Length > 0)
            {
                strings.Add(sb.ToString().Trim());
            }

            return strings;
        }

        /// <summary>
        /// get name without the thickness value
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string with (???) removed</returns>
        public string GetMaterialName(string str)
        {
            var end = GetLastMatchChar(str, '(');
            if (end < 0) //no match return original
                return str;
 
            //check we have a number in the brackets
            var isNum = GetLayerThickness(str);
            if (isNum == null) //no number so return original
                return str;
            
            return str.Substring(0, end).Trim();
        }
        /// <summary>
        /// Get any value held in ()
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>double value or null</returns>
        public  double? GetLayerThickness(string str)
        {
            var start = GetLastMatchChar(str, '(');
            var end = GetLastMatchChar(str, ')');
            if ((start < 0) || (end < 0)) //failed to find bracket pair
            {
                return null;
            }
            str = str.Substring((start + 1), (end - start - 1));

            double value;
            if (double.TryParse(str, out value))
                return value;
            else
                return null;
        }

        /// <summary>
        /// find the string matching from the back of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public int GetLastMatchChar(string str, char match)
        {
            var arr = str.ToCharArray();
            Array.Reverse(arr);
            var newStr = new string(arr);

            var end = newStr.IndexOf(match);
            if (end >= 0) //bracket found, if not will return -1
            {
                end = str.Length - end - 1; //less one more zero based, as length is not zero based
            }
            return end;
        }

        /// <summary>
        /// Check that a IIfcRoot object name exists in the model on a merge
        /// </summary>
        /// <typeparam name="T">object derived from IIfcRoot </typeparam>
        /// <param name="name">string name to compare with name property of objects in model</param>
        /// <returns>bool</returns>
        protected bool CheckIfExistOnMerge<T>(string name) where T : IIfcRoot
        {
            if (XBimContext.IsMerge)
            {
                if (ValidateString(name)) //we have a primary key to check
                {
                    var testName = name.ToLower().Trim();
                    var testObj = Model.Instances.Where<T>(bs => bs.Name.ToString().ToLower().Trim() == testName).FirstOrDefault();
                    if (testObj != null)
                    {
#if DEBUG
                        Console.WriteLine("{0} : {1} exists so skip on merge", testObj.GetType().Name, name);
#endif
                        return true; //we have it so no need to create
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check that a IIfcRoot object name exists in the model on a merge
        /// </summary>
        /// <typeparam name="T">object derived from IIfcRoot </typeparam>
        /// <param name="name">string name to compare with name property of objects in model</param>
        /// <returns>the object</returns>
        protected IEnumerable<T> CheckIfObjExistOnMerge<T>(string name) where T : IIfcRoot
        {
            if (XBimContext.IsMerge)
            {
                if (ValidateString(name)) //we have a primary key to check
                {
                    var testName = name.ToLower().Trim();
                    var testObjs = Model.Instances.Where<T>(bs => bs.Name.ToString().ToLower().Trim() == testName);
                    return testObjs;
                }
            }
            return Enumerable.Empty<T>();
        }

        
        #endregion

    }
}
