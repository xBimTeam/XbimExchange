using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.GeometricConstraintResource;
using Xbim.Ifc2x3.RepresentationResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.Extensions;
using Xbim.COBie.Data;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.Common.Geometry;


namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimCoordinate : COBieXBim
    {

        public COBieXBimCoordinate(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        #region Methods
        /// <summary>
        /// Create and setup Bounding Box's
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieCoordinateRow to read data from</param>
        public void SerialiseCoordinate(COBieSheet<COBieCoordinateRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Coordinate"))
            {

                try
                {
                    int count = 1;
                    ProgressIndicator.ReportMessage("Starting Coordinates...");
                    ProgressIndicator.Initialise("Creating Coordinates", cOBieSheet.RowCount);
                    var rows = cOBieSheet.Rows.OrderBy(a => a.SheetName == "Component").ThenBy(a => a.SheetName == "Space").ThenBy(a => a.SheetName == "Floor"); //order into Floor,Space,Component, needed to build object placements

                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        COBieCoordinateRow row = rows.ElementAt(i);// cOBieSheet[i];
                        COBieCoordinateRow rowNext = null;
                        BumpTransaction(trans, count);
                        count++;
                        
                        //do floor placement point
                        if ((ValidateString(row.Category)) &&  
                            (row.Category.ToLower() == "point")
                            )
                        {
                            ProgressIndicator.IncrementAndUpdate();
                        
                            AddFloorPlacement(row);
                            continue; //work done, next loop please
                        }
                        //do bounding box items
                        if ((ValidateString(row.Category)) && 
                            (row.Category.Contains("box-"))
                            )
                        {    
                            i++; //set to get next row

                            if (i < cOBieSheet.RowCount) //get next row if still in range
                                rowNext = rows.ElementAt(i);//cOBieSheet[i];

                            if ((rowNext != null) &&
                                (ValidateString(rowNext.Category)) && 
                                (rowNext.Category.Contains("box-")) &&
                                (ValidateString(row.SheetName)) && 
                                //(ValidateString(row.RowName)) &&
                                (ValidateString(rowNext.SheetName)) &&
                                //(ValidateString(rowNext.RowName)) &&
                                (row.SheetName == rowNext.SheetName) &&
                                (row.RowName == rowNext.RowName)
                                )
                            {
                                ProgressIndicator.IncrementAndUpdate();
                                
                                AddBoundingBoxAsExtrudedAreaSolid(row, rowNext);
                                
                                ProgressIndicator.IncrementAndUpdate(); //two row processed here
                        
//#if DEBUG
//                                Console.WriteLine("{0} : {1} == {2} : {3} ", row.SheetName, row.RowName, rowNext.SheetName, rowNext.RowName);
//#endif                           
                                continue;
                            }
                            else
                            {
#if DEBUG
                                if (rowNext == null)
                                    Console.WriteLine("Failed to find pair {0} : {1} != {2} : {3} ", row.SheetName, row.RowName, "Null", "Null");
                                else
                                    Console.WriteLine("Failed to find pair {0} : {1} != {2} : {3} ", row.SheetName, row.RowName, rowNext.SheetName, rowNext.RowName);
#endif
                                i--; //set back in case next is point, as two box points failed
                            }
                        }
                       
                    }
                    ProgressIndicator.Finalise();
                    
                    trans.Commit();

                }
                catch (Exception)
                {
                    //TODO: Catch with logger?
                    throw;
                }

            }
        }

        /// <summary>
        /// Add floor placement point
        /// </summary>
        /// <param name="row">COBieCoordinateRow holding the data</param>
        private void AddFloorPlacement(COBieCoordinateRow row)
        {
            IfcBuildingStorey ifcBuildingStorey = null;
            if (ValidateString(row.ExtIdentifier))
            {
                IfcGloballyUniqueId id = new IfcGloballyUniqueId(row.ExtIdentifier);
                ifcBuildingStorey = Model.Instances.Where<IfcBuildingStorey>(bs => bs.GlobalId == id).FirstOrDefault();
            }

            if ((ifcBuildingStorey == null) && (ValidateString(row.RowName)))
            {
                ifcBuildingStorey = Model.Instances.Where<IfcBuildingStorey>(bs => bs.Name == row.RowName).FirstOrDefault();
            }

            if (ifcBuildingStorey != null)
            {
                IfcProduct placementRelToIfcProduct = ifcBuildingStorey.SpatialStructuralElementParent as IfcProduct;
                IfcLocalPlacement objectPlacement = CalcObjectPlacement(row, placementRelToIfcProduct);
                if (objectPlacement != null)
                {
                    //using statement will set the Model.OwnerHistoryAddObject to IfcRoot.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
                    //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
                    using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcBuildingStorey.OwnerHistory))
                    {
                        ifcBuildingStorey.ObjectPlacement = objectPlacement;
                    }
                }
            }
        }

        /// <summary>
        /// Add space placement
        /// </summary>
        /// <param name="row">COBieCoordinateRow holding the data for one corner</param>
        /// <param name="rowNext">COBieCoordinateRow holding the data for the other corner</param>
        private void AddBoundingBoxAsExtrudedAreaSolid(COBieCoordinateRow row, COBieCoordinateRow rowNext)
        {
            if (row.SheetName.ToLower() == "floor")
            {
                IfcBuildingStorey ifcBuildingStorey = null;
                if (ValidateString(row.ExtIdentifier))
                {
                    IfcGloballyUniqueId id = new IfcGloballyUniqueId(row.ExtIdentifier);
                    ifcBuildingStorey = Model.Instances.Where<IfcBuildingStorey>(bs => bs.GlobalId == id).FirstOrDefault();
                }

                if ((ifcBuildingStorey == null) && (ValidateString(row.RowName)))
                {
                    ifcBuildingStorey = Model.Instances.Where<IfcBuildingStorey>(bs => bs.Name == row.RowName).FirstOrDefault();
                }

                if (ifcBuildingStorey != null)
                {
                    //using statement will set the Model.OwnerHistoryAddObject to IfcRoot.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
                    //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
                    using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcBuildingStorey.OwnerHistory))
                    {
                        IfcProduct placementRelToIfcProduct = ifcBuildingStorey.SpatialStructuralElementParent as IfcProduct;
                        AddExtrudedRectangle(row, rowNext, ifcBuildingStorey, placementRelToIfcProduct);
                    }
                }
            } 
            if (row.SheetName.ToLower() == "space")
            {
                IfcSpace ifcSpace = null;
                if (ValidateString(row.ExtIdentifier))
                {
                    IfcGloballyUniqueId id = new IfcGloballyUniqueId(row.ExtIdentifier);
                    ifcSpace = Model.Instances.Where<IfcSpace>(bs => bs.GlobalId == id).FirstOrDefault();
                }
                if ((ifcSpace == null) && (ValidateString(row.RowName)))
                {
                    ifcSpace = Model.Instances.Where<IfcSpace>(bs => bs.Name == row.RowName).FirstOrDefault();
                }
                if ((ifcSpace == null) && (ValidateString(row.RowName)))
                {
                    IEnumerable<IfcSpace> ifcSpaces = Model.Instances.Where<IfcSpace>(bs => bs.Description == row.RowName);
                    //check we have one, if >1 then no match
                    if ((ifcSpaces.Any()) && (ifcSpaces.Count() == 1))
                        ifcSpace = ifcSpaces.FirstOrDefault();
                }

                if (ifcSpace != null)
                {
                    if (ifcSpace.Representation != null) //check it has no graphics attached, if it has then skip
                        return;
                    //using statement will set the Model.OwnerHistoryAddObject to IfcRoot.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
                    //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
                    using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcSpace.OwnerHistory))
                    {
                        IfcProduct placementRelToIfcProduct = ifcSpace.SpatialStructuralElementParent as IfcProduct;

                        AddExtrudedRectangle(row, rowNext, ifcSpace, placementRelToIfcProduct);
                    }
                }

            }

            if (row.SheetName.ToLower() == "component")
            {
                IfcElement ifcElement = null;
                if (ValidateString(row.ExtIdentifier))
                {
                    IfcGloballyUniqueId id = new IfcGloballyUniqueId(row.ExtIdentifier);
                    ifcElement = Model.Instances.Where<IfcElement>(bs => bs.GlobalId == id).FirstOrDefault();
                }
                if ((ifcElement == null) && (ValidateString(row.RowName)))
                {
                    ifcElement = Model.Instances.Where<IfcElement>(bs => bs.Name == row.RowName).FirstOrDefault();
                }

                if ((ifcElement == null) && (ValidateString(row.RowName)))
                {
                    IEnumerable<IfcElement> ifcElements = Model.Instances.Where<IfcElement>(bs => bs.Description == row.RowName);
                    //check we have one, if >1 then no match
                    if ((ifcElements.Any()) && (ifcElements.Count() == 1))
                        ifcElement = ifcElements.FirstOrDefault();
                }

                if (ifcElement != null)
                {
                    if (ifcElement.Representation != null) //check it has no graphics attached, if it has then skip
                        return;

                    //using statement will set the Model.OwnerHistoryAddObject to IfcRoot.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
                    //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
                    using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcElement.OwnerHistory))
                    {
                        IfcProduct placementRelToIfcProduct = ifcElement.ContainedInStructure as IfcProduct;
                        IfcRelContainedInSpatialStructure ifcRelContainedInSpatialStructure = Model.Instances.OfType<IfcRelContainedInSpatialStructure>().Where(rciss => rciss.RelatedElements.Contains(ifcElement)).FirstOrDefault();
                        if ((ifcRelContainedInSpatialStructure != null) &&
                            (ifcRelContainedInSpatialStructure.RelatingStructure != null)
                            )
                        {
                            placementRelToIfcProduct = ifcRelContainedInSpatialStructure.RelatingStructure as IfcProduct;
                            AddExtrudedRectangle(row, rowNext, ifcElement, placementRelToIfcProduct);
                        }
                        else
                        {
#if DEBUG
                            Console.WriteLine("COBieXBimCoordinate.AddBoundingBoxAsExtrudedAreaSolid: Cannot find Parent object placement");
#endif                        
                        }
                    }
                    
                }
                else
                {
#if DEBUG
                    Console.WriteLine("COBieXBimCoordinate.AddBoundingBoxAsExtrudedAreaSolid: Cannot find object to relate points too");
#endif
                }
            }
            
        }

        /// <summary>
        /// Add a Bounding Box extrusion onto the ifcProduct
        /// </summary>
        /// <param name="row">COBieCoordinateRow holding the data for one corner</param>
        /// <param name="rowNext">COBieCoordinateRow holding the data for the other corner</param>
        /// <param name="placementRelToIfcProduct">Product which is parent of ifcProduct passed product to add extrusion onto</param>
        /// <param name="ifcProduct">IfcProduct to add the extrusion onto</param>
        private void AddExtrudedRectangle(COBieCoordinateRow row, COBieCoordinateRow rowNext, IfcProduct ifcProduct, IfcProduct placementRelToIfcProduct)
        {
            if (ifcProduct != null)
            {
                COBieCoordinateRow lowerLeftRow, upperRightRow;
                if (row.Category.ToLower() == "box-lowerleft")
                {
                    lowerLeftRow = row;
                    upperRightRow = rowNext;
                }
                else
                {
                    lowerLeftRow = rowNext;
                    upperRightRow = row;
                }
                IfcLocalPlacement objectPlacement = CalcObjectPlacement(lowerLeftRow, placementRelToIfcProduct);
                if (objectPlacement != null)
                {
                    //set the object placement for the space
                    ifcProduct.ObjectPlacement = objectPlacement;

                    //get matrix to the space placement
                    XbimMatrix3D matrix3D = ConvertMatrix3D(objectPlacement);
                    //invert matrix so we can convert row points back to the object space
                    matrix3D.Invert();
                    //lets get the points from the two rows
                    XbimPoint3D lowpt, highpt;
                    if ((GetPointFromRow(upperRightRow, out highpt)) &&
                         (GetPointFromRow(lowerLeftRow, out lowpt))
                        )
                    {
                        //transform the points back to object space
                        lowpt = matrix3D.Transform(lowpt);
                        highpt = matrix3D.Transform(highpt);
                        //in object space so we can use Rect3D as this will be aligned with coordinates systems X and Y
                        XbimRect3D bBox = new XbimRect3D();
                        bBox.Location = lowpt;
                        bBox.Union(highpt);
                        if ((double.NaN.CompareTo(bBox.SizeX) != 0) && (double.NaN.CompareTo(bBox.SizeY) != 0))
                        {
                            XbimPoint3D ctrPt = new XbimPoint3D(bBox.X + (bBox.SizeX / 2.0), bBox.Y + (bBox.SizeY / 2.0), bBox.Z + (bBox.SizeZ / 2.0));

                            //Create IfcRectangleProfileDef
                            IfcCartesianPoint IfcCartesianPointCtr = Model.Instances.New<IfcCartesianPoint>(cp => { cp.X = ctrPt.X; cp.Y = ctrPt.Y; cp.Z = 0.0; }); //centre point of 2D box
                            IfcDirection IfcDirectionXDir = Model.Instances.New<IfcDirection>(d => { d.X = 1.0; d.Y = 0; d.Z = 0.0; }); //default to X direction
                            IfcAxis2Placement2D ifcAxis2Placement2DCtr = Model.Instances.New<IfcAxis2Placement2D>(a2p => { a2p.Location = IfcCartesianPointCtr; a2p.RefDirection = IfcDirectionXDir; });
                            IfcRectangleProfileDef ifcRectangleProfileDef = Model.Instances.New<IfcRectangleProfileDef>(rpd => { rpd.ProfileType = IfcProfileTypeEnum.AREA; rpd.ProfileName = row.RowName; rpd.Position = ifcAxis2Placement2DCtr; rpd.XDim = bBox.SizeX; rpd.YDim = bBox.SizeY; });

                            //Create IfcExtrudedAreaSolid
                            IfcDirection IfcDirectionAxis = Model.Instances.New<IfcDirection>(d => { d.X = 0.0; d.Y = 0; d.Z = 1.0; }); //default to Z direction
                            IfcDirection IfcDirectionRefDir = Model.Instances.New<IfcDirection>(d => { d.X = 1.0; d.Y = 0; d.Z = 0.0; }); //default to X direction
                            IfcCartesianPoint IfcCartesianPointPosition = Model.Instances.New<IfcCartesianPoint>(cp => { cp.X = 0.0; cp.Y = 0.0; cp.Z = 0.0; }); //centre point of 2D box
                            IfcAxis2Placement3D ifcAxis2Placement3DPosition = Model.Instances.New<IfcAxis2Placement3D>(a2p3D => { a2p3D.Location = IfcCartesianPointPosition; a2p3D.Axis = IfcDirectionAxis; a2p3D.RefDirection = IfcDirectionRefDir; });
                            IfcDirection IfcDirectionExtDir = Model.Instances.New<IfcDirection>(d => { d.X = 0.0; d.Y = 0; d.Z = 1.0; }); //default to Z direction
                            IfcExtrudedAreaSolid ifcExtrudedAreaSolid = Model.Instances.New<IfcExtrudedAreaSolid>(eas => { eas.SweptArea = ifcRectangleProfileDef; eas.Position = ifcAxis2Placement3DPosition; eas.ExtrudedDirection = IfcDirectionExtDir; eas.Depth = bBox.SizeZ; });

                            //Create IfcShapeRepresentation
                            IfcShapeRepresentation ifcShapeRepresentation = Model.Instances.New<IfcShapeRepresentation>(sr => { sr.ContextOfItems = Model.IfcProject.ModelContext(); sr.RepresentationIdentifier = "Body"; sr.RepresentationType = "SweptSolid"; });
                            ifcShapeRepresentation.Items.Add(ifcExtrudedAreaSolid);

                            //create IfcProductDefinitionShape
                            IfcProductDefinitionShape ifcProductDefinitionShape = Model.Instances.New<IfcProductDefinitionShape>(pds => { pds.Name = row.Name; pds.Description = row.SheetName; });
                            ifcProductDefinitionShape.Representations.Add(ifcShapeRepresentation);

                            //Link to the IfcProduct
                            ifcProduct.Representation = ifcProductDefinitionShape;
                        }
                        else
                        {
#if DEBUG
                            Console.WriteLine("Failed to calculate box size for {0}", row.Name);
#endif
                        }
                    }
                    
                }
                else
                {
#if DEBUG
                    Console.WriteLine("Failed to add Object placement for {0}", row.Name);
#endif
                }

            }
        }

        /// <summary>
        /// Calculate the ObjectPlacment for an IfcProduct from row data and the parent object
        /// </summary>
        /// <param name="row">COBieCoordinateRow holding the data</param>
        /// <param name="placementRelToIfcProduct">IfcProduct that the ObjectPlacment relates too, i.e. the parent of the ifcProduct ObjectPlacment we are calculating</param>
        /// <returns></returns>
        private IfcLocalPlacement CalcObjectPlacement(COBieCoordinateRow row, IfcProduct placementRelToIfcProduct)
        {
            XbimPoint3D locationPt;
            bool havePoint = GetPointFromRow(row, out locationPt);
            if (havePoint)
            {
                if ((placementRelToIfcProduct != null) && (placementRelToIfcProduct.ObjectPlacement is IfcLocalPlacement))
                {
                    //TEST, change the building position to see if the same point comes out in Excel sheet, it should be, and in test was.
                    //((IfcAxis2Placement3D)((IfcLocalPlacement)placementRelToIfcProduct.ObjectPlacement).RelativePlacement).SetNewLocation(10.0, 10.0, 0.0);
                    IfcLocalPlacement placementRelTo = (IfcLocalPlacement)placementRelToIfcProduct.ObjectPlacement;
                    XbimMatrix3D matrix3D = ConvertMatrix3D(placementRelTo);
                    
                    //we want to take off the translations and rotations caused by IfcLocalPlacement of the parent objects as we will add these to the new IfcLocalPlacement for this floor
                    matrix3D.Invert(); //so invert matrix to remove the translations to give the origin for the next IfcLocalPlacement
                    locationPt = matrix3D.Transform(locationPt); //get the point with relation to the last IfcLocalPlacement i.e the parent element
                    
                    //Get the WCS matrix values
                    double rotX, rotY, rotZ;
                    if (!(double.TryParse(row.YawRotation, out rotX) && (double.NaN.CompareTo(rotX) != 0)))
                        rotX = 0.0;

                    if (!(double.TryParse(row.ElevationalRotation, out rotY) && (double.NaN.CompareTo(rotY) != 0)))
                        rotY = 0.0;

                    if (double.TryParse(row.ClockwiseRotation, out rotZ) && (double.NaN.CompareTo(rotZ) != 0))
                        rotZ = rotZ * -1; //convert back from clockwise to anti clockwise
                    else
                        rotZ = 0.0;

                    //apply the WCS rotation from COBie Coordinates stored values
                    XbimMatrix3D matrixNewRot3D = new XbimMatrix3D();
                    if (rotX != 0.0)
                        matrixNewRot3D.RotateAroundXAxis(TransformedBoundingBox.DegreesToRadians(rotX));
                    if (rotY != 0.0)
                        matrixNewRot3D.RotateAroundYAxis(TransformedBoundingBox.DegreesToRadians(rotY));
                    if (rotZ != 0.0)
                        matrixNewRot3D.RotateAroundZAxis(TransformedBoundingBox.DegreesToRadians(rotZ));
                    
                    //remove any displacement from the matrix which moved/rotated us to the object space
                    matrix3D.OffsetX = 0.0F;
                    matrix3D.OffsetY = 0.0F;
                    matrix3D.OffsetZ = 0.0F;
                    
                    //remove the matrix that got use to the object space from the WCS location of this object
                    XbimMatrix3D matrixRot3D = matrixNewRot3D * matrix3D;
                    
                    //get the rotation vectors to place in the new IfcAxis2Placement3D for the new IfcLocalPlacement for this object
                    XbimVector3D ucsAxisX = matrixRot3D.Transform(new XbimVector3D(1, 0, 0));
                    XbimVector3D ucsAxisZ = matrixRot3D.Transform(new XbimVector3D(0, 0, 1));
                    ucsAxisX.Normalize();
                    ucsAxisZ.Normalize();

                    //create the new IfcAxis2Placement3D 
                    IfcAxis2Placement3D relativePlacemant = Model.Instances.New<IfcAxis2Placement3D>();
                    relativePlacemant.SetNewDirectionOf_XZ(ucsAxisX.X, ucsAxisX.Y, ucsAxisX.Z, ucsAxisZ.X, ucsAxisZ.Y, ucsAxisZ.Z);
                    relativePlacemant.SetNewLocation(locationPt.X, locationPt.Y, locationPt.Z);

                    //Set up IfcLocalPlacement
                    IfcLocalPlacement objectPlacement = Model.Instances.New<IfcLocalPlacement>();
                    objectPlacement.PlacementRelTo = placementRelTo;
                    objectPlacement.RelativePlacement = relativePlacemant;

                    return objectPlacement;
                }
            }
            return null;
           
        }

        

        private bool GetPointFromRow(COBieCoordinateRow row, out XbimPoint3D point)
        {
            double x, y, z;
            if ((double.TryParse(row.CoordinateXAxis, out x)) &&
                (double.TryParse(row.CoordinateYAxis, out y)) &&
                (double.TryParse(row.CoordinateZAxis, out z))
                )
            {
                point = new XbimPoint3D(x, y, z);
                return true;
            }
            else
            {
                point = new XbimPoint3D();
                return false;
            }
        }

        /// <summary>
        /// Builds a windows Matrix3D from an ObjectPlacement
        /// Conversion fo c++ function CartesianTransform::ConvertMatrix3D from CartesianTransform.cpp
        /// </summary>
        /// <param name="objPlacement">IfcObjectPlacement object</param>
        /// <returns>Matrix3D</returns>
		protected XbimMatrix3D ConvertMatrix3D(IfcObjectPlacement objPlacement)
		{
			if(objPlacement is IfcLocalPlacement)
			{
				IfcLocalPlacement locPlacement = (IfcLocalPlacement)objPlacement;
				if (locPlacement.RelativePlacement is IfcAxis2Placement3D)
				{
					IfcAxis2Placement3D axis3D = (IfcAxis2Placement3D)locPlacement.RelativePlacement;
                    XbimVector3D ucsXAxis = new XbimVector3D(axis3D.RefDirection.DirectionRatios[0], axis3D.RefDirection.DirectionRatios[1], axis3D.RefDirection.DirectionRatios[2]);
                    XbimVector3D ucsZAxis = new XbimVector3D(axis3D.Axis.DirectionRatios[0], axis3D.Axis.DirectionRatios[1], axis3D.Axis.DirectionRatios[2]);
					ucsXAxis.Normalize();
					ucsZAxis.Normalize();
                    XbimVector3D ucsYAxis = XbimVector3D.CrossProduct(ucsZAxis, ucsXAxis);
					ucsYAxis.Normalize();
					XbimPoint3D ucsCentre = axis3D.Location.XbimPoint3D();

                    XbimMatrix3D ucsTowcs = new XbimMatrix3D(ucsXAxis.X, ucsXAxis.Y, ucsXAxis.Z, 0,
						ucsYAxis.X, ucsYAxis.Y, ucsYAxis.Z, 0,
						ucsZAxis.X, ucsZAxis.Y, ucsZAxis.Z, 0,
						ucsCentre.X, ucsCentre.Y, ucsCentre.Z , 1);
					if (locPlacement.PlacementRelTo != null)
					{
                        return XbimMatrix3D.Multiply(ucsTowcs, ConvertMatrix3D(locPlacement.PlacementRelTo));
					}
					else
						return ucsTowcs;

				}
				else //must be 2D
				{
                    throw new NotImplementedException("Support for Placements other than 3D not implemented");
				}

			}
			else //probably a Grid
			{
                throw new NotImplementedException("Support for Placements other than Local not implemented");
			}
        }
        #endregion
    }
}
