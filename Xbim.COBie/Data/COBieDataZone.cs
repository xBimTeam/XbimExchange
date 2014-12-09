using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.XbimExtensions;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Zone tab.
    /// </summary>
    public class COBieDataZone : COBieData<COBieZoneRow>, IAttributeProvider
    {
        /// <summary>
        /// Data Zone constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataZone(COBieContext context) : base(context)
        { }

        #region methods

        /// <summary>
        /// Fill sheet rows for Zone sheet
        /// </summary>
        /// <returns>COBieSheet<COBieZoneRow></returns>
        public override COBieSheet<COBieZoneRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Zones...");

            //Create new sheet
            COBieSheet<COBieZoneRow> zones = new COBieSheet<COBieZoneRow>(Constants.WORKSHEET_ZONE);

           
            // get all IfcBuildingStory objects from IFC file
            IEnumerable<IfcZone> ifcZones = Model.Instances.OfType<IfcZone>();

            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);
            
            //list of attributes to exclude form attribute sheet
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Zone.AttributesContain);
            attributeBuilder.RowParameters["Sheet"] = "Zone";
            
            //Also check to see if we have any zones within the spaces
            IEnumerable<IfcSpace> ifcSpaces = Model.Instances.OfType<IfcSpace>();//.OrderBy(ifcSpace => ifcSpace.Name, new CompareIfcLabel());

            ProgressIndicator.Initialise("Creating Zones", ifcZones.Count() + ifcSpaces.Count());

            foreach (IfcZone zn in ifcZones)
            {
                ProgressIndicator.IncrementAndUpdate();
                // create zone for each space found
                Dictionary<String, COBieZoneRow> ExistingZones = new Dictionary<string, COBieZoneRow>();
                IEnumerable<IfcSpace> spaces = (zn.IsGroupedBy == null) ? Enumerable.Empty<IfcSpace>() : zn.IsGroupedBy.RelatedObjects.OfType<IfcSpace>();
                foreach (IfcSpace sp in spaces)
                {
                    COBieZoneRow zone;
                    if (ExistingZones.ContainsKey(zn.Name.ToString()))
                    {
                        zone = ExistingZones[zn.Name.ToString()];
                        zone.SpaceNames += ","+sp.Name.ToString();
                    }
                    else
                    {
                        zone = new COBieZoneRow(zones);
                        ExistingZones[zn.Name.ToString()] = zone;

                        //set allPropertyValues to this element
                        allPropertyValues.SetAllPropertyValues(zn); //set the internal filtered IfcPropertySingleValues List in allPropertyValues

                        zone.Name = zn.Name.ToString();

                        string createBy = allPropertyValues.GetPropertySingleValueValue("COBieCreatedBy", false); //support for COBie Toolkit for Autodesk Revit
                        zone.CreatedBy = ValidateString(createBy) ? createBy : GetTelecomEmailAddress(zn.OwnerHistory);
                        string createdOn = allPropertyValues.GetPropertySingleValueValue("COBieCreatedOn", false);//support for COBie Toolkit for Autodesk Revit
                        zone.CreatedOn = ValidateString(createdOn) ? createdOn : GetCreatedOnDateAsFmtString(zn.OwnerHistory);

                        zone.Category = GetCategory(zn);

                        zone.SpaceNames = sp.Name;

                        string extSystem = allPropertyValues.GetPropertySingleValueValue("COBieExtSystem", false);//support for COBie Toolkit for Autodesk Revit
                        zone.ExtSystem = ValidateString(extSystem) ? extSystem : GetExternalSystem(zn);
                        zone.ExtObject = zn.GetType().Name;
                        zone.ExtIdentifier = zn.GlobalId;
                        string description = allPropertyValues.GetPropertySingleValueValue("COBieDescription", false);//support for COBie Toolkit for Autodesk Revit
                        if (ValidateString(extSystem))
                            zone.Description = extSystem;
                        else
                            zone.Description = (string.IsNullOrEmpty(zn.Description)) ? zn.Name.ToString() : zn.Description.ToString(); //if IsNullOrEmpty on Description then output Name
                        zones.AddRow(zone);

                        //fill in the attribute information
                        attributeBuilder.RowParameters["Name"] = zone.Name;
                        attributeBuilder.RowParameters["CreatedBy"] = zone.CreatedBy;
                        attributeBuilder.RowParameters["CreatedOn"] = zone.CreatedOn;
                        attributeBuilder.RowParameters["ExtSystem"] = zone.ExtSystem;
                        attributeBuilder.PopulateAttributesRows(zn); //fill attribute sheet rows//pass data from this sheet info as Dictionary
                    }
                }

            }

            COBieDataPropertySetValues allSpacePropertyValues = new COBieDataPropertySetValues(); //get all property sets and associated properties in one go

            Dictionary<String, COBieZoneRow> myExistingZones = new Dictionary<string, COBieZoneRow>();

            foreach (IfcSpace sp in ifcSpaces)
            {
                ProgressIndicator.IncrementAndUpdate();
                allSpacePropertyValues.SetAllPropertyValues(sp); //set the space as the current object for the properties get glass

                IEnumerable<IfcPropertySingleValue> spProperties = Enumerable.Empty<IfcPropertySingleValue>();
                foreach (KeyValuePair<IfcPropertySet, IEnumerable<IfcSimpleProperty>> item in allSpacePropertyValues.MapPsetToProps)
                {
                    IfcPropertySet pset = item.Key;
                    spProperties = item.Value.Where(p => p.Name.ToString().Contains("ZoneName")).OfType<IfcPropertySingleValue>();


                    //if we have no ifcZones or "ZoneName" properties, and the DepartmentsUsedAsZones flag is true then list departments as zones
                    if ((!spProperties.Any()) && (!ifcZones.Any()) && (Context.DepartmentsUsedAsZones == true))
                    {
                        spProperties = item.Value.Where(p => p.Name == "Department").OfType<IfcPropertySingleValue>();
                    }

                    foreach (IfcPropertySingleValue spProp in spProperties)
                    {
                        COBieZoneRow zone;
                        if (myExistingZones.ContainsKey(spProp.NominalValue.ToString()))
                        {
                            zone = myExistingZones[spProp.NominalValue.ToString()];
                            zone.SpaceNames += "," + sp.Name;
                        } else {
                            zone = new COBieZoneRow(zones);
                            zone.Name = spProp.NominalValue.ToString();
                            myExistingZones[spProp.NominalValue.ToString()] = zone;

                            zone.CreatedBy = GetTelecomEmailAddress(sp.OwnerHistory);
                            zone.CreatedOn = GetCreatedOnDateAsFmtString(sp.OwnerHistory);

                            zone.Category = spProp.Name;
                            zone.SpaceNames = sp.Name;

                            zone.ExtSystem = GetExternalSystem(pset);
                            zone.ExtObject = spProp.GetType().Name;
                            zone.ExtIdentifier = pset.GlobalId.ToString(); //IfcPropertySingleValue has no GlobalId so set to the holding IfcPropertySet
                            
                            zone.Description = (string.IsNullOrEmpty(spProp.NominalValue.ToString())) ? DEFAULT_STRING : spProp.NominalValue.ToString(); ;

                            zones.AddRow(zone);
                        }
                    }
                }
                //spProperties = spProperties.OrderBy(p => p.Name.ToString(), new CompareString()); //consolidate test, Concat as looping spaces then sort then dump to COBieZoneRow foreach   
            }

            zones.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();

            return zones;
        }
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
