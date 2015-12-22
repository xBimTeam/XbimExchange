using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Common.Geometry;

#if DEBUG
using System.Diagnostics;
#endif

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Holds Geometry information for the space objects
    /// </summary>
    public struct SpaceInfo
    {
        public string Name { get; set; }
        public XbimRect3D Rectangle { get; set; }
        public XbimMatrix3D Matrix { get; set; }
    }
    
    /// <summary>
    /// Class to input data into excel worksheets for the the Component tab.
    /// </summary>
    public class COBieDataComponent : COBieData<COBieComponentRow>, IAttributeProvider
    {
        /// <summary>
        /// Data Component constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataComponent(COBieContext context)
            : base(context)
        {
            SpaceBoundingBoxInfo = new List<SpaceInfo>();
        }

        public List<SpaceInfo> SpaceBoundingBoxInfo { get; set; }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Component sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieComponentRow> Fill()
        {
#if DEBUG
            Stopwatch timer = new Stopwatch();
            timer.Start();
#endif
            ProgressIndicator.ReportMessage("Starting Components...");
            //Create new sheet
            COBieSheet<COBieComponentRow> components = new COBieSheet<COBieComponentRow>(Constants.WORKSHEET_COMPONENT);
         
            

            IEnumerable<IfcRelAggregates> relAggregates = Model.Instances.OfType<IfcRelAggregates>();
            IEnumerable<IfcRelContainedInSpatialStructure> relSpatial = Model.Instances.OfType<IfcRelContainedInSpatialStructure>();

            IEnumerable<IfcObject> ifcElements = ((from x in relAggregates
                                            from y in x.RelatedObjects
                                                   where !Context.Exclude.ObjectType.Component.Contains(y.GetType())
                                            select y).Union(from x in relSpatial
                                                            from y in x.RelatedElements
                                                            where !Context.Exclude.ObjectType.Component.Contains(y.GetType())
                                                            select y)).OfType<IfcObject>(); //.GroupBy(el => el.Name).Select(g => g.First())//.Distinct().ToList();
            
            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);
            //set up filters on COBieDataPropertySetValues for the SetAttributes only
            attributeBuilder.ExcludeAttributePropertyNames.AddRange(Context.Exclude.Component.AttributesEqualTo); //we do not want listed properties for the attribute sheet so filter them out
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Component.AttributesContain);//we do not want listed properties for the attribute sheet so filter them out
            attributeBuilder.RowParameters["Sheet"] = "Component";


            ProgressIndicator.Initialise("Creating Components", ifcElements.Count());

            foreach (var obj in ifcElements)
            {
                ProgressIndicator.IncrementAndUpdate();
                
                COBieComponentRow component = new COBieComponentRow(components);
                
                IfcElement el = obj as IfcElement;
                if (el == null)
                    continue;
                string name = el.Name.ToString();
                if (string.IsNullOrEmpty(name))
                {
                    name = "Name Unknown " + UnknownCount.ToString();
                    UnknownCount++;
                }
                //set allPropertyValues to this element
                allPropertyValues.SetAllPropertyValues(el); //set the internal filtered IfcPropertySingleValues List in allPropertyValues
                component.Name = name;

                string createBy = allPropertyValues.GetPropertySingleValueValue("COBieCreatedBy", false); //support for COBie Toolkit for Autodesk Revit
                component.CreatedBy = ValidateString(createBy) ? createBy : GetTelecomEmailAddress(el.OwnerHistory);
                string createdOn = allPropertyValues.GetPropertySingleValueValue("COBieCreatedOn", false);//support for COBie Toolkit for Autodesk Revit
                component.CreatedOn = ValidateString(createdOn) ?  createdOn : GetCreatedOnDateAsFmtString(el.OwnerHistory);
                
                component.TypeName = GetTypeName(el);
                component.Space = GetComponentRelatedSpace(el);
                string description = allPropertyValues.GetPropertySingleValueValue("COBieDescription", false);//support for COBie Toolkit for Autodesk Revit
                component.Description = ValidateString(description) ? description : GetComponentDescription(el);
                string extSystem = allPropertyValues.GetPropertySingleValueValue("COBieExtSystem", false);//support for COBie Toolkit for Autodesk Revit
                component.ExtSystem = ValidateString(extSystem) ? extSystem : GetExternalSystem(el);
                component.ExtObject = el.GetType().Name;
                component.ExtIdentifier = el.GlobalId;

                //set from PropertySingleValues filtered via candidateProperties
                //set the internal filtered IfcPropertySingleValues List in allPropertyValues to this element set above
                component.SerialNumber = allPropertyValues.GetPropertySingleValueValue("SerialNumber", false);
                component.InstallationDate = GetDateFromProperty(allPropertyValues, "InstallationDate");
                component.WarrantyStartDate = GetDateFromProperty(allPropertyValues, "WarrantyStartDate");
                component.TagNumber = allPropertyValues.GetPropertySingleValueValue("TagNumber", false);
                component.BarCode = allPropertyValues.GetPropertySingleValueValue("BarCode", false);
                component.AssetIdentifier = allPropertyValues.GetPropertySingleValueValue("AssetIdentifier", false);
                
                components.AddRow(component);

                //fill in the attribute information
                attributeBuilder.RowParameters["Name"] = component.Name;
                attributeBuilder.RowParameters["CreatedBy"] = component.CreatedBy;
                attributeBuilder.RowParameters["CreatedOn"] = component.CreatedOn;
                attributeBuilder.RowParameters["ExtSystem"] = component.ExtSystem;
                attributeBuilder.PopulateAttributesRows(el); //fill attribute sheet rows
            }

            components.OrderBy(s=>s.Name);

            ProgressIndicator.Finalise();
#if DEBUG
            timer.Stop();
            Console.WriteLine(String.Format("Time to generate Component data = {0} seconds", timer.Elapsed.TotalSeconds.ToString("F3")));
#endif
           
            
            return components;
        }

        
        
        /// <summary>
        /// Get Formatted Start Date
        /// </summary>
        /// <param name="allPropertyValues"></param>
        /// <returns></returns>
        private string GetDateFromProperty(COBieDataPropertySetValues allPropertyValues, string propertyName)
        {
            string startData = "";
            IfcPropertySingleValue ifcPropertySingleValue = allPropertyValues.GetPropertySingleValue(propertyName);
            if ((ifcPropertySingleValue != null) && (ifcPropertySingleValue.NominalValue != null))
                startData = ifcPropertySingleValue.NominalValue.ToString();

            DateTime frmDate;
            if (DateTime.TryParse(startData, out frmDate))
                startData = frmDate.ToString(Constants.DATE_FORMAT);
            else if (string.IsNullOrEmpty(startData))
                startData = Constants.DEFAULT_STRING;//Context.RunDate;
            
            return startData;
        }

        //Fields for the GetComponentRelatedSpace function
        List<IfcRelSpaceBoundary> ifcRelSpaceBoundarys = null;
        
        /// <summary>
        /// Get Space name which holds the passed in IfcElement
        /// </summary>
        /// <param name="el">Element to extract space name from</param>
        /// <returns>string</returns>
        internal string GetComponentRelatedSpace(IfcElement el)
        {
            string value = "";
            List<string> names = new List<string>();
                    
            if (el != null )//&& el.ContainedInStructure.Count() > 0
            {
                var owningObjects = el.ContainedInStructure.Select(cis => cis.RelatingStructure).OfType<IfcSpace>(); //only one or zero held in ContainedInStructure
                foreach (IfcSpace item in owningObjects)
                {
                    if (item.Name != null)
                        names.Add(item.Name);
                }
            }

            //check for the element as a containing item of a space
            if (ifcRelSpaceBoundarys == null)
            {
                ifcRelSpaceBoundarys = Model.Instances.OfType<IfcRelSpaceBoundary>().Where(rsb => (rsb.RelatedBuildingElement != null)).ToList();
            }
            IEnumerable<IfcSpace> ifcSpaces = ifcRelSpaceBoundarys.Where(rsb => rsb.RelatedBuildingElement == el).Select(rsb => rsb.RelatingSpace);
            foreach (IfcSpace item in ifcSpaces)
            {
                if ((item.Name != null) && (!names.Contains(item.Name)))
                    names.Add(item.Name);
            }

            if (names.Count > 0) //check we have some values
            {
                value = string.Join(", ", names);
            }
            
            //if no space is saved with the element then get from the geometry 
            if (string.IsNullOrEmpty(value))
            { 
                value = GetSpaceHoldingElement(el);
            }
            

            return string.IsNullOrEmpty(value) ? Constants.DEFAULT_STRING : value;
        }

        //Fields for the GetComponentRelatedSpace function
        List<IfcSpace> ifcSpaces = null;

        /// <summary>
        /// Get the space name holding the element
        /// </summary>
        /// <param name="el">IfcElement to get containing space for</param>
        /// <returns>Space name</returns>
        internal string GetSpaceHoldingElement(IfcElement el)
        {
            //see if we have space information, if not fill information list
            if (SpaceBoundingBoxInfo.Count == 0)
            {
                if (ifcSpaces == null)
                {
                    ifcSpaces = Model.Instances.OfType<IfcSpace>().ToList();
                }

                //get Geometry for spaces 
             SpaceBoundingBoxInfo = Model.GetGeometryData(XbimGeometryType.BoundingBox)
                .Where(bb => bb.IfcTypeId == Model.Metadata.ExpressTypeId(typeof(IfcSpace)))
                .Select(bb => new SpaceInfo
                {
                    Rectangle = XbimRect3D.FromArray(bb.ShapeData),
                    Matrix = XbimMatrix3D.FromArray(bb.DataArray2),
                    Name = ifcSpaces.Where(sp => (sp.EntityLabel == bb.IfcProductLabel)).Select(sp => sp.Name.ToString()).FirstOrDefault()
                }).ToList();
            }

            
            string spaceName = string.Empty;
            //only if we have any space information
            if (SpaceBoundingBoxInfo.Any())
            {
                //find the IfcElement Bounding Box and To WCS Matrix
                XbimGeometryData elGeoData = Model.GetGeometryData(el, XbimGeometryType.BoundingBox).FirstOrDefault();
                //check to see if we have any geometry within the file
                if (elGeoData == null)
                    return string.Empty; //No geometry

                XbimRect3D elBoundBox = XbimRect3D.FromArray(elGeoData.ShapeData);
                XbimMatrix3D elWorldMatrix = XbimMatrix3D.FromArray(elGeoData.DataArray2);
                //Get object space top and bottom points of the bounding box
                List<XbimPoint3D> elBoxPts = new List<XbimPoint3D>();
                elBoxPts.Add(new XbimPoint3D(elBoundBox.X, elBoundBox.Y, elBoundBox.Z));
                elBoxPts.Add(new XbimPoint3D(elBoundBox.X + elBoundBox.SizeX, elBoundBox.Y + elBoundBox.SizeY, elBoundBox.Z + elBoundBox.SizeZ));
                elBoxPts.Add(elBoundBox.Centroid());

                //convert points of the bounding box to WCS
                IEnumerable<XbimPoint3D> elBoxPtsWCS = elBoxPts.Select(pt => elWorldMatrix.Transform(pt));
                //see if we hit any spaces
                spaceName = GetSpaceFromPoints(elBoxPtsWCS);
                //if we failed to get space on min points then use the remaining corner points
                if (string.IsNullOrEmpty(spaceName))
                {
                    XbimPoint3D elMinPt = elBoxPts[0];
                    XbimPoint3D elMaxPt = elBoxPts[1];
                    //elBoxPts.Clear(); //already tested points in list so clear them

                    //Extra testing on remaining corner points on the top and bottom plains
                    elBoxPts.Add(new XbimPoint3D(elMaxPt.X, elMaxPt.Y, elMinPt.Z));
                    elBoxPts.Add(new XbimPoint3D(elMaxPt.X, elMinPt.Y, elMinPt.Z));
                    elBoxPts.Add(new XbimPoint3D(elMinPt.X, elMaxPt.Y, elMinPt.Z));
                    elBoxPts.Add(new XbimPoint3D((elMaxPt.X - elMinPt.X) / 2.0, (elMaxPt.Y - elMinPt.Y) / 2.0, elMinPt.Z)); //centre face point

                    elBoxPts.Add(new XbimPoint3D(elMinPt.X, elMinPt.Y, elMaxPt.Z));
                    elBoxPts.Add(new XbimPoint3D(elMaxPt.X, elMinPt.Y, elMaxPt.Z));
                    elBoxPts.Add(new XbimPoint3D(elMinPt.X, elMaxPt.Y, elMaxPt.Z));
                    elBoxPts.Add(new XbimPoint3D((elMaxPt.X - elMinPt.X) / 2.0, (elMaxPt.Y - elMinPt.Y) / 2.0, elMaxPt.Z)); //centre face point
                    //convert points of the bounding box to WCS
                    elBoxPtsWCS = elBoxPts.Select(pt => elWorldMatrix.Transform(pt));
                    //see if we hit any spaces
                    spaceName = GetSpaceFromPoints(elBoxPtsWCS);
                }
                if (string.IsNullOrEmpty(spaceName))
                {
                    //Get tolerance size from element, 1% of smallest side size
                    double tol = elBoundBox.SizeX * 0.001;
                    if ((elBoundBox.SizeY * 0.001) < tol)
                        tol = elBoundBox.SizeY * 0.001;
                    if ((elBoundBox.SizeZ * 0.001) < tol)
                        tol = elBoundBox.SizeZ * 0.001;
                    if ((tol == 0.0) && //if tol 0.0
                        ((Context.WorkBookUnits.LengthUnit.Equals("meters", StringComparison.OrdinalIgnoreCase)) ||
                         (Context.WorkBookUnits.LengthUnit.Equals("metres", StringComparison.OrdinalIgnoreCase))
                        )
                       )
                        tol = 0.001;

                    spaceName = GetSpaceFromClosestPoints(elBoxPtsWCS, tol);
                }
            }
            return spaceName;
        }

        /// <summary>
        /// Get the space name if any of the points are within the space
        /// </summary>
        /// <param name="PtsWCS">list of points</param>
        /// <returns>Space name</returns>
        internal string GetSpaceFromPoints(IEnumerable<XbimPoint3D> PtsWCS)
        {
            //holder for space names, could be more then one so a list is used
            List<string> spaceNames = new List<string>();

            foreach (SpaceInfo spGeoData in SpaceBoundingBoxInfo)
            {
                //get each space bounding box and To WCS Matrix
                XbimRect3D spBoundBox = spGeoData.Rectangle;
                XbimMatrix3D spWorldMatrix = spGeoData.Matrix;
                String spName = spGeoData.Name;
                //we need to transform the element max and min points back into the spaces Object Space so we can test on Bounding Box rectangle
                spWorldMatrix.Invert();
                IEnumerable<XbimPoint3D> elBoxPtsOCS = PtsWCS.Select(pt => spWorldMatrix.Transform(pt));
                //check if element space object points are contained fully within the space bounding box rectangle
                IEnumerable<XbimPoint3D> hitPts = elBoxPtsOCS.Where(pt => spBoundBox.Contains(pt));
                if ( hitPts.Any() &&
                    (hitPts.Count() == elBoxPtsOCS.Count())
                    )
                {
                    spaceNames.Add(spName);
                    break; //all element point are contained in one space so kill loop
                }
                else if (hitPts.Any())//one or more point is contained in space and continue in case we have an element over several spaces
                {
                    if (!spaceNames.Contains(spName))
                        spaceNames.Add(spName);
                }
            }
            if (spaceNames.Count > 0)
                return string.Join(", ", spaceNames);
            else
                return string.Empty;
        }

        /// <summary>
        /// Get the space name if any of the points are within the space
        /// </summary>
        /// <param name="PtsWCS">list of points</param>
        /// <param name="hitTotarance">Distance the point is outside the space but considered still usable to reference the space</param>
        /// <returns>Space name</returns>
        internal string GetSpaceFromClosestPoints(IEnumerable<XbimPoint3D> PtsWCS, double hitTotarance)
        {
            //holder for space names, could be more then one so a list is used
            List<string> spaceNames = new List<string>();

            foreach (SpaceInfo spGeoData in SpaceBoundingBoxInfo)
            {
                //get each space bounding box and To WCS Matrix
                XbimRect3D spBoundBox = spGeoData.Rectangle;
                XbimMatrix3D spWorldMatrix = spGeoData.Matrix;
                String spName = spGeoData.Name;
                //we need to transform the element max and min points back into the spaces Object Space so we can test on Bounding Box rectangle
                spWorldMatrix.Invert();
                IEnumerable<XbimPoint3D> elBoxPtsOCS = PtsWCS.Select(pt => spWorldMatrix.Transform(pt));
                //check if element space object points are contained fully within the space bounding box rectangle
                IEnumerable<double> hitPts = elBoxPtsOCS.Select(pt => DistanceFromSpace(pt, spBoundBox)).Where(d => d <= hitTotarance);
                if (hitPts.Any())//one or more point is contained in space and continue in case we have an element over several spaces
                {
                    if (!spaceNames.Contains(spName))
                        spaceNames.Add(spName);
                }
            }
            if (spaceNames.Count > 0)
                return string.Join(", ", spaceNames);
            else
                return string.Empty;
        }

        /// <summary>
        /// Closet point on a axially aligned bounding boxes (usually in object space)
        /// REF: 3D Math Primer for Graphics and Game Development. page 720.
        /// </summary>
        /// <param name="pt">Point to get closet point on bounding box from</param>
        /// <param name="boundBox">Bounding Box as Rect3D</param>
        /// <returns>Point3D (note: if return point == pt point then point inside box</returns>
        internal XbimPoint3D ClosetPointOnBoundingBox(XbimPoint3D pt, XbimRect3D boundBox)
        {
            XbimPoint3D retPt = new XbimPoint3D();
            XbimPoint3D MinPt = new XbimPoint3D(boundBox.X, boundBox.Y, boundBox.Z);
            XbimPoint3D MaxPt = new XbimPoint3D(boundBox.X + boundBox.SizeX, boundBox.Y + boundBox.SizeY, boundBox.Z + boundBox.SizeZ);
            
            if ( pt.X < MinPt.X ) 
                retPt.X = MinPt.X;
            else if ( pt.X > MaxPt.X) 
                retPt.X = MaxPt.X;

            if (pt.Y < MinPt.Y)
                retPt.Y = MinPt.Y;
            else if (pt.Y > MaxPt.Y)
                retPt.Y = MaxPt.Y; 
            
            if (pt.Z < MinPt.Z)
                retPt.Z = MinPt.Z;
            else if (pt.Z > MaxPt.Z)
                retPt.Z = MaxPt.Z;

            return retPt;
        }

        /// <summary>
        /// distance point is from the axially aligned bounding boxes (usually in object space)
        /// </summary>
        /// <param name="pt">Point to get closet point on bounding box from</param>
        /// <param name="boundBox">Bounding Box as Rect3D</param>
        /// <returns>Distance from the box edge</returns>
        internal double DistanceFromSpace(XbimPoint3D pt, XbimRect3D boundBox)
        {
            XbimPoint3D hitPt = ClosetPointOnBoundingBox(pt, boundBox);
            XbimVector3D vect = XbimPoint3D.Subtract(pt, hitPt);
            return vect.Length;
        }
        

        /// <summary>
        /// Get Description for passed in IfcElement
        /// </summary>
        /// <param name="el">Element holding description</param>
        /// <returns>string</returns>
        internal string GetComponentDescription(IfcElement el)
        {
            if (el != null)
            {
                if (!string.IsNullOrEmpty(el.Description)) return el.Description;
                else if (!string.IsNullOrEmpty(el.Name)) return el.Name;
            }
            return DEFAULT_STRING;
        }
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
