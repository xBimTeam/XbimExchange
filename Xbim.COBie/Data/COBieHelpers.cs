using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common.Geometry;
using Xbim.Ifc;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBie.Data
{
    internal class COBieHelpers
    {
        private static IEnumerable<IfcRelSpaceBoundary> ifcRelSpaceBoundarys = null; 
        /// <summary>
                        /// Get Space name which holds the passed in IfcElement
                        /// </summary>
                        /// <param name="el">Element to extract space name from</param>
                        /// <returns>string</returns>
        internal static string GetComponentRelatedSpace(IfcElement el, IfcStore Model, IEnumerable<SpaceInfo> SpaceBoundingBoxInfo, COBieContext Context)
        {
            string value = "";
            List<string> names = new List<string>();

            if (el != null)//&& el.ContainedInStructure.Count() > 0
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
                ifcRelSpaceBoundarys = Model.FederatedInstances.OfType<IfcRelSpaceBoundary>().Where(rsb => (rsb.RelatedBuildingElement != null)).ToList();
            }
            var ifcSpaces = ifcRelSpaceBoundarys.Where(rsb => rsb.RelatedBuildingElement == el).Select(rsb => rsb.RelatingSpace);
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
                value = GetSpaceHoldingElement(el, Model, SpaceBoundingBoxInfo, ifcSpaces, Context);
            }


            return string.IsNullOrEmpty(value) ? Constants.DEFAULT_STRING : value;
        }

        internal static void EnsureSpaceList(IEnumerable<IfcSpace> ifcSpaces, IfcStore Model)
        {
            if (ifcSpaces == null)
            {
                ifcSpaces = Model.FederatedInstances.OfType<IfcSpace>().ToList();
            }
        }
        /// <summary>
        /// Get the space name holding the element
        /// </summary>
        /// <param name="el">IfcElement to get containing space for</param>
        /// <returns>Space name</returns>
        internal static string GetSpaceHoldingElement(IfcElement el, IfcStore Model, IEnumerable<SpaceInfo> SpaceBoundingBoxInfo, IEnumerable<IfcSpace> ifcSpaces, COBieContext Context)
        {
            string spaceName = string.Empty;
            int ifcSpacesId = Model.Metadata.ExpressTypeId(typeof(IfcSpace));

            using (var geomStore = Model.GeometryStore)
            {
                using (var geomReader = geomStore.BeginRead())
                {
                    // see if we have space information, if not fill information list
                    // if SpaceBoundingBoxInfo is not populated then prepare it
                    if (!SpaceBoundingBoxInfo.Any())
                    {
                        EnsureSpaceList(ifcSpaces, Model);
                        SpaceBoundingBoxInfo =
                            geomReader.ShapeInstances.Where(x => x.IfcTypeId == ifcSpacesId).Select(bb =>
                                new SpaceInfo()
                                {
                                    Rectangle = bb.BoundingBox,
                                    Matrix = bb.Transformation,
                                    Name =
                                        ifcSpaces.Where(sp => (sp.EntityLabel == bb.IfcProductLabel))
                                            .Select(sp => sp.Name.ToString())
                                            .FirstOrDefault()
                                }
                                ).ToList();
                    }

                    //only if we have any space information
                    if (SpaceBoundingBoxInfo.Any())
                    {
                        var t1 = geomReader.ShapeInstancesOfEntity(el).FirstOrDefault();
                        if (t1 == null)
                            return string.Empty;
                        var t2 = t1.BoundingBox;



                        var elBoundBox = t1.BoundingBox;
                        var elWorldMatrix = t1.Transformation;
                        //Get object space top and bottom points of the bounding box
                        var elBoxPts = new List<XbimPoint3D>
                        {
                            new XbimPoint3D(elBoundBox.X, elBoundBox.Y, elBoundBox.Z),
                            new XbimPoint3D(elBoundBox.X + elBoundBox.SizeX, elBoundBox.Y + elBoundBox.SizeY,
                                elBoundBox.Z + elBoundBox.SizeZ),
                            elBoundBox.Centroid()
                        };

                        //convert points of the bounding box to WCS
                        IEnumerable<XbimPoint3D> elBoxPtsWCS = elBoxPts.Select(pt => elWorldMatrix.Transform(pt));
                        //see if we hit any spaces
                        spaceName = GetSpaceFromPoints(elBoxPtsWCS, SpaceBoundingBoxInfo);
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
                            spaceName = GetSpaceFromPoints(elBoxPtsWCS, SpaceBoundingBoxInfo);
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

                            spaceName = GetSpaceFromClosestPoints(elBoxPtsWCS, tol, SpaceBoundingBoxInfo);
                        }
                    }
                }
            }
            return spaceName;
        }

        /// <summary>
        /// Closet point on a axially aligned bounding boxes (usually in object space)
        /// REF: 3D Math Primer for Graphics and Game Development. page 720.
        /// </summary>
        /// <param name="pt">Point to get closet point on bounding box from</param>
        /// <param name="boundBox">Bounding Box as Rect3D</param>
        /// <returns>Point3D (note: if return point == pt point then point inside box</returns>
        internal static XbimPoint3D ClosetPointOnBoundingBox(XbimPoint3D pt, XbimRect3D boundBox)
        {

            var minPt = new XbimPoint3D(boundBox.X, boundBox.Y, boundBox.Z);
            var maxPt = new XbimPoint3D(boundBox.X + boundBox.SizeX, boundBox.Y + boundBox.SizeY, boundBox.Z + boundBox.SizeZ);
            double x = 0;
            double y = 0;
            double z = 0;
            if (pt.X < minPt.X)
                x = minPt.X;
            else if (pt.X > maxPt.X)
                x = maxPt.X;

            if (pt.Y < minPt.Y)
                y = minPt.Y;
            else if (pt.Y > maxPt.Y)
                y = maxPt.Y;

            if (pt.Z < minPt.Z)
                z = minPt.Z;
            else if (pt.Z > maxPt.Z)
                z = maxPt.Z;
            XbimPoint3D retPt = new XbimPoint3D(x, y, z);
            return retPt;
        }


        /// <summary>
        /// distance point is from the axially aligned bounding boxes (usually in object space)
        /// </summary>
        /// <param name="pt">Point to get closet point on bounding box from</param>
        /// <param name="boundBox">Bounding Box as Rect3D</param>
        /// <returns>Distance from the box edge</returns>
        internal static double DistanceFromSpace(XbimPoint3D pt, XbimRect3D boundBox)
        {
            XbimPoint3D hitPt = ClosetPointOnBoundingBox(pt, boundBox);
            XbimVector3D vect = XbimPoint3D.Subtract(pt, hitPt);
            return vect.Length;
        }
        /// <summary>
        /// Get the space name if any of the points are within the space
        /// </summary>
        /// <param name="PtsWCS">list of points</param>
        /// <param name="hitTotarance">Distance the point is outside the space but considered still usable to reference the space</param>
        /// <returns>Space name</returns>
        internal static string GetSpaceFromClosestPoints(IEnumerable<XbimPoint3D> PtsWCS, double hitTotarance, IEnumerable<SpaceInfo> SpaceBoundingBoxInfo)
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
        /// Get the space name if any of the points are within the space
        /// </summary>
        /// <param name="PtsWCS">list of points</param>
        /// <returns>Space name</returns>
        internal static string GetSpaceFromPoints(IEnumerable<XbimPoint3D> PtsWCS, IEnumerable<SpaceInfo> SpaceBoundingBoxInfo)
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
                if (hitPts.Any() &&
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
    }
}
