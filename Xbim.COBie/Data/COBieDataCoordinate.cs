using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Extensions;


using Xbim.Common.Geometry;
using Xbim.ModelGeometry.Scene;


namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Coordinate tab.
    /// </summary>
    public class COBieDataCoordinate : COBieData<COBieCoordinateRow>
    {
        /// <summary>
        /// Data Coordinate constructor
        /// </summary>
        /// <param name="context"></param>
        public COBieDataCoordinate(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Coordinate sheet
        /// </summary>
        public override COBieSheet<COBieCoordinateRow> Fill()
        {
            
            //get the conversion to the COBie units (metres or feet)

            double conversionFactor;
            var cobieUnits = Context.WorkBookUnits.LengthUnit.ToLowerInvariant();
            if (cobieUnits == "meters" || cobieUnits == "metres") conversionFactor = Model.ModelFactors.OneMetre;
            else if (cobieUnits == "millimeters" || cobieUnits == "millimetres") conversionFactor = Model.ModelFactors.OneMilliMetre;
            else if (cobieUnits == "feet" || cobieUnits == "foot") conversionFactor = Model.ModelFactors.OneFoot;
            else if (cobieUnits == "inch" || cobieUnits == "inches") conversionFactor = Model.ModelFactors.OneInch;
            else throw new Exception("The COBie units are incorrectly set, they should be 'meters' , 'feet', 'millimeters etc");

            XbimMatrix3D globalTransform = XbimMatrix3D.CreateScale(1 / conversionFactor);

            var coordinates = new COBieSheet<COBieCoordinateRow>(Constants.WORKSHEET_COORDINATE);
                ProgressIndicator.ReportMessage("Starting Coordinates...");


                //Create new sheet
                
                //Get buildings and spaces
            var ifcBuildingStoreys = Model.Instances.OfType<IfcBuildingStorey>();
            var ifcSpaces = Model.Instances.OfType<IfcSpace>().OrderBy(ifcSpace => ifcSpace.Name, new CompareIfcLabel());
            var ifcProducts = ifcBuildingStoreys.Union<IfcProduct>(ifcSpaces); //add spaces

                //get component products as shown in Component sheet
            var relAggregates = Model.Instances.OfType<IfcRelAggregates>();
            var relSpatial = Model.Instances.OfType<IfcRelContainedInSpatialStructure>();
            var ifcElements = ((from x in relAggregates
                                                        from y in x.RelatedObjects
                                                        where !Context.Exclude.ObjectType.Component.Contains(y.GetType())
                                                        select y).Union(from x in relSpatial
                                                                        from y in x.RelatedElements
                                                                        where !Context.Exclude.ObjectType.Component.Contains(y.GetType())
                                                                        select y)).OfType<IfcProduct>();  //.GroupBy(el => el.Name).Select(g => g.First())
                ifcProducts = ifcProducts.Union(ifcElements);

            var productList = ifcProducts as IList<IfcProduct> ?? ifcProducts.ToList();
            ProgressIndicator.Initialise("Creating Coordinates", productList.Count());

            var m3D = new Xbim3DModelContext(Model);
            
            foreach (var ifcProduct in productList)
                {

                    ProgressIndicator.IncrementAndUpdate();
                    //if no name to link the row name too skip it, as no way to link back to the parent object
                    //if (string.IsNullOrEmpty(ifcProduct.Name))
                    //    continue;

                var coordinate = new COBieCoordinateRow(coordinates)
                {
                    Name = (string.IsNullOrEmpty(ifcProduct.Name.ToString()))
                        ? DEFAULT_STRING
                        : ifcProduct.Name.ToString(),
                    CreatedBy = GetTelecomEmailAddress(ifcProduct.OwnerHistory),
                    CreatedOn = GetCreatedOnDateAsFmtString(ifcProduct.OwnerHistory)
                };

                // (ifcBuildingStorey == null || ifcBuildingStorey.Name.ToString() == "") ? "CoordinateName" : ifcBuildingStorey.Name.ToString();
                    
                    coordinate.RowName = coordinate.Name;

                IfcCartesianPoint ifcCartesianPointLower;
                IfcCartesianPoint ifcCartesianPointUpper;
                var transBox = new TransformedBoundingBox();
                    if (ifcProduct is IfcBuildingStorey)
                    {
                    XbimMatrix3D worldMatrix = ifcProduct.ObjectPlacement.ToMatrix3D();
                    ifcCartesianPointLower = new IfcCartesianPoint(worldMatrix.OffsetX, worldMatrix.OffsetY,
                        worldMatrix.OffsetZ);
                    //get the offset from the world coordinates system 0,0,0 point, i.e. origin point of this object in world space
                        coordinate.SheetName = "Floor";
                        coordinate.Category = "point";
                    ifcCartesianPointUpper = null;
                    }
                    else
                    {
                        if (ifcProduct is IfcSpace)
                            coordinate.SheetName = "Space";
                        else
                            coordinate.SheetName = "Component";

                        coordinate.Category = "box-lowerleft"; //and box-upperright, so two values required when we do this

                    var boundBox = XbimRect3D.Empty;
                    XbimMatrix3D transform = XbimMatrix3D.Identity;
                    foreach (var shapeInstance in m3D.ShapeInstancesOf(ifcProduct))
                    {
                        if (boundBox.IsEmpty)
                            boundBox = shapeInstance.BoundingBox;
                        else
                            boundBox.Union(shapeInstance.BoundingBox);
                        transform = shapeInstance.Transformation;
                    }
                    XbimMatrix3D m = globalTransform* transform;
                    transBox = new TransformedBoundingBox(boundBox, m);
                    //set points
                    ifcCartesianPointLower = new IfcCartesianPoint(transBox.MinPt);
                    ifcCartesianPointUpper = new IfcCartesianPoint(transBox.MaxPt);

                    }

                coordinate.CoordinateXAxis = string.Format("{0}", (double)ifcCartesianPointLower[0]);
                coordinate.CoordinateYAxis = string.Format("{0}", (double)ifcCartesianPointLower[1]);
                coordinate.CoordinateZAxis = string.Format("{0}", (double)ifcCartesianPointLower[2]);
                    coordinate.ExtSystem = GetExternalSystem(ifcProduct);
                    coordinate.ExtObject = ifcProduct.GetType().Name;
                    if (!string.IsNullOrEmpty(ifcProduct.GlobalId))
                    {
                        coordinate.ExtIdentifier = ifcProduct.GlobalId.ToString();
                    }
                coordinate.ClockwiseRotation = transBox.ClockwiseRotation.ToString("F4");
                coordinate.ElevationalRotation = transBox.ElevationalRotation.ToString("F4");
                coordinate.YawRotation = transBox.YawRotation.ToString("F4");

                    coordinates.AddRow(coordinate);
                    if (ifcCartesianPointUpper != null) //we need a second row for upper point
                    {
                    var coordinateUpper = new COBieCoordinateRow(coordinates);
                        coordinateUpper.Name = coordinate.Name;
                        coordinateUpper.CreatedBy = coordinate.CreatedBy;
                        coordinateUpper.CreatedOn = coordinate.CreatedOn;
                        coordinateUpper.RowName = coordinate.RowName;
                        coordinateUpper.SheetName = coordinate.SheetName;
                        coordinateUpper.Category = "box-upperright";
                    coordinateUpper.CoordinateXAxis = string.Format("{0}", (double)ifcCartesianPointUpper[0]);
                    coordinateUpper.CoordinateYAxis = string.Format("{0}", (double)ifcCartesianPointUpper[1]);
                    coordinateUpper.CoordinateZAxis = string.Format("{0}", (double)ifcCartesianPointUpper[2]);
                        coordinateUpper.ExtSystem = coordinate.ExtSystem;
                        coordinateUpper.ExtObject = coordinate.ExtObject;
                        coordinateUpper.ExtIdentifier = coordinate.ExtIdentifier;
                        coordinateUpper.ClockwiseRotation = coordinate.ClockwiseRotation;
                        coordinateUpper.ElevationalRotation = coordinate.ElevationalRotation;
                        coordinateUpper.YawRotation = coordinate.YawRotation;

                        coordinates.AddRow(coordinateUpper);
                    }

                }

            coordinates.OrderBy(s => s.Name);
            
                ProgressIndicator.Finalise();
                

            return coordinates;
        }

        #endregion
    }

    /// <summary>
    /// Structure to transform a bounding box values to world space
    /// </summary>
    public struct TransformedBoundingBox 
    {
        public TransformedBoundingBox(XbimRect3D boundBox, XbimMatrix3D matrix)
            : this()
	    {
            //Object space values
            
            MinPt = new XbimPoint3D(boundBox.X, boundBox.Y, boundBox.Z);
            MaxPt = new XbimPoint3D(boundBox.X + boundBox.SizeX, boundBox.Y + boundBox.SizeY, boundBox.Z + boundBox.SizeZ);
            //make assumption that the X direction will be the longer length hence the orientation will be along the x axis
            //transformed values, no longer a valid bounding box in the new space if any Pitch or Yaw
            MinPt = matrix.Transform(MinPt);
            MaxPt = matrix.Transform(MaxPt);

            
            //--------Calculate rotations from matrix-------
            //rotation around X,Y,Z axis
            double rotationZ, rotationY, rotationX;
            GetMatrixRotations(matrix, out rotationX, out rotationY, out rotationZ);
            
            //adjust Z to get clockwise rotation
            ClockwiseRotation = RadiansToDegrees(rotationZ * -1); //use * -1 to make a clockwise rotation
            ElevationalRotation = RadiansToDegrees(rotationY);
            YawRotation = RadiansToDegrees(rotationX);
            
	    }
        /// <summary>
        /// Get Euler angles from matrix
        /// derived for here i think!! http://khayyam.kaplinski.com/2011_06_01_archive.html
        /// </summary>
        /// <param name="m">XbimMatrix3D Matrix</param>
        /// <param name="rotationX">out parameter - X rotation in radians</param>
        /// <param name="rotationY">out parameter - Y rotation in radians</param>
        /// <param name="rotationZ">out parameter - Z rotation in radians</param>
        public static void GetMatrixRotations(XbimMatrix3D m, out double rotationX, out double rotationY, out double rotationZ)
        {
            double aX, aZ, aX2, aZ2;
            double aY = Math.Asin(m.M31);
            double aY2 = Math.PI - aY;
            if (Math.Abs(Math.Abs(m.M31) - 1) > 1e-9)
            {
                double c = Math.Cos(aY);
                double c2 = Math.Cos(aY2);
                double trX = m.M33 / c;
                double trY = -m.M32 / c;
                aX = Math.Atan2(trY, trX);
                aX2 = Math.Atan2(-m.M32 / c2, m.M33 / c2);
                trX = m.M11 / c;
                trY = -m.M21 / c;
                aZ = Math.Atan2(trY, trX);
                aZ2 = Math.Atan2(-m.M21 / c2, m.M11 / c2);
            }
            else
            {
                aX = aX2 = 0;
                double trX = m.M22;
                double trY = m.M12;
                aZ = aZ2 = Math.Atan2(trY, trX);
            }
            if ((aX * aX + aY * aY + aZ * aZ) < (aX2 * aX2 + aY2 * aY2 + aZ2 * aZ2))
            {
                rotationX = aX;
                rotationY = aY;
                rotationZ = aZ;
            }
            else
            {
                rotationX = aX2;
                rotationY = aY2;
                rotationZ = aZ2;
            }

        }
        

        /// <summary>
        /// Radians to Degrees
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double RadiansToDegrees (double value)
        {
            return value * (180 / Math.PI);
        }

        /// <summary>
        /// Degrees to Radians
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double DegreesToRadians(double value)
        {
            return value * (Math.PI / 180);
        }
        /// <summary>
        /// Minimum point, classed as origin point
        /// </summary>
        public XbimPoint3D MinPt { get; set; }
        /// <summary>
        /// Maximum point of the rectangle
        /// </summary>
        public XbimPoint3D MaxPt { get; set; }
        /// <summary>
        /// Clockwise rotation of the IfcProduct
        /// </summary>
        public double ClockwiseRotation { get; set; }
        /// <summary>
        /// Elevation rotation of the IfcProduct
        /// </summary>
        public double ElevationalRotation  { get; set; }
        /// <summary>
        /// Yaw rotation of the IfcProduct
        /// </summary>
        public double YawRotation { get; set; }

        
        
        
    }
}
