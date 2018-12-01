using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBie
{
    internal static class IfcAxis2Placement3DExtensions
    {
        public static void SetNewLocation(this IfcAxis2Placement3D axis3, double x, double y, double z)
        {
            IModel model = axis3.Model;
            IfcCartesianPoint location = model.Instances.New<IfcCartesianPoint>();
            location.X = x;
            location.Y = y;
            location.Z = z;
            axis3.Location = location;
        }


        /// <summary>
        ///   Sets new directions of the axes. Direction vectors are automaticaly normalized.
        /// </summary>
        /// <param name = "axis3"></param>
        /// <param name = "xAxisDirectionX"></param>
        /// <param name = "xAxisDirectionY"></param>
        /// <param name = "xAxisDirectionZ"></param>
        /// <param name = "zAxisDirectionX"></param>
        /// <param name = "zAxisDirectionY"></param>
        /// <param name = "zAxisDirectionZ"></param>
        public static void SetNewDirectionOf_XZ(this IfcAxis2Placement3D axis3, double xAxisDirectionX,
                                                double xAxisDirectionY, double xAxisDirectionZ, double zAxisDirectionX,
                                                double zAxisDirectionY, double zAxisDirectionZ)
        {
            IModel model = axis3.Model;
            IfcDirection zDirection = model.Instances.New<IfcDirection>();
            zDirection.DirectionRatios[0] = zAxisDirectionX;
            zDirection.DirectionRatios[1] = zAxisDirectionY;
            zDirection.DirectionRatios[2] = zAxisDirectionZ;
            zDirection.Normalise();
            axis3.Axis = zDirection;

            IfcDirection xDirection = model.Instances.New<IfcDirection>();
            xDirection.DirectionRatios[0] = xAxisDirectionX;
            xDirection.DirectionRatios[1] = xAxisDirectionY;
            xDirection.DirectionRatios[2] = xAxisDirectionZ;
            xDirection.Normalise();
            axis3.RefDirection = xDirection;
        }
    }

    internal static class ActorExtensions
    {
        public static bool HasEmail(this IfcActorSelect actor, string emailAddress)
        {
            var personOrg = actor as IfcPersonAndOrganization;
            if (personOrg != null)
                return personOrg.HasEmail(emailAddress);
            var person = actor as IfcPerson;
            if (person != null)
                return person.HasEmail(emailAddress);
            var organisation = actor as IfcOrganization;
            if (organisation != null)
                return organisation.HasEmail(emailAddress);
            return false;
        }

        public static bool HasEmail(this IfcPerson ifcPerson, string emailAddress)
        {
            if (ifcPerson.Addresses != null)
            {
                return ifcPerson.Addresses.OfType<IfcTelecomAddress>().Select(address => address.ElectronicMailAddresses)
                    .Where(item => item != null).SelectMany(em => em)
                    .FirstOrDefault(em => string.Compare(emailAddress, em, true) == 0) != null;
            }
            return false;
        }
    }

    public static class SpatialStructureElementExtensions
    {
        /// <summary>
        ///   Returns  the first spatial structural element that this decomposes
        /// </summary>
        /// <param name = "se"></param>
        /// <returns></returns>
        public static IfcSpatialStructureElement GetContainingStructuralElement(this IfcSpatialStructureElement se)
        {
            IModel model = se.Model;
            IEnumerable<IfcRelContainedInSpatialStructure> rels =
                model.Instances.Where<IfcRelContainedInSpatialStructure>(r => r.RelatedElements.Contains(se));
            return rels.Select(r => r.RelatingStructure).FirstOrDefault();
            // return  se).Instances.Where<RelContainedInSpatialStructure>(r => r.RelatedElements.Contains(se)).Select(r=>r.RelatingStructure).FirstOrDefault(.ModelOf;
        }

        /// <summary>
        ///   Returns  the spatial structural elements that this decomposes
        /// </summary>
        /// <param name = "se"></param>
        /// <returns></returns>
        public static IEnumerable<IfcSpatialStructureElement> GetContainingStructuralElements(
            this IfcSpatialStructureElement se)
        {
            IModel model = se.Model;
            IEnumerable<IfcRelContainedInSpatialStructure> rels =
                model.Instances.Where<IfcRelContainedInSpatialStructure>(r => r.RelatedElements.Contains(se));
            return rels.Select(r => r.RelatingStructure);
            // return  se).Instances.Where<RelContainedInSpatialStructure>(r => r.RelatedElements.Contains(se)).Select(r=>r.RelatingStructure).FirstOrDefault(.ModelOf;
        }


        /// <summary>
        ///   Adds the  element to the set of  elements which are contained in this spatialstructure
        /// </summary>
        /// <param name = "se"></param>
        /// <param name = "prod"></param>
        public static void AddElement(this IfcSpatialStructureElement se, IfcProduct prod)
        {
            if (prod == null) return;

            IEnumerable<IfcRelContainedInSpatialStructure> relatedElements = se.ContainsElements;
            if (relatedElements.Count() == 0) //none defined create the relationship
            {
                IfcRelContainedInSpatialStructure relSe =
                    se.Model.Instances.New<IfcRelContainedInSpatialStructure>();
                relSe.RelatingStructure = se;
                relSe.RelatedElements.Add(prod);
            }
            else
            {
                relatedElements.First().RelatedElements.Add(prod);
            }
        }
        /// <summary>
        ///   Adds specified IfcSpatialStructureElement to the decomposition of this spatial structure element.
        /// </summary>
        /// <param name = "se"></param>
        /// <param name = "child">Child spatial structure element.</param>
        public static void AddToSpatialDecomposition(this IfcSpatialStructureElement se,
                                                     IfcSpatialStructureElement child)
        {
            IEnumerable<IfcRelDecomposes> decomposition = se.IsDecomposedBy;
            if (decomposition.Count() == 0) //none defined create the relationship
            {
                IfcRelAggregates relSub = se.Model.Instances.New<IfcRelAggregates>();
                relSub.RelatingObject = se;
                relSub.RelatedObjects.Add(child);
            }
            else
            {
                decomposition.First().RelatedObjects.Add(child);
            }
        }

    }

    public static class GroupExtension
    {

        public static void AddObjectToGroup(this IfcGroup gr, IfcObjectDefinition obj)
        {
            IModel model = gr.Model;

            IfcRelAssignsToGroup relation = gr.IsGroupedBy ?? model.Instances.New<IfcRelAssignsToGroup>(rel => rel.RelatingGroup = gr);
            relation.RelatedObjects.Add(obj);
        }
    }

    public static class ModelExtensions
    {
        public static IfcProject GetProject(this IModel model)
        {
            return model.Instances.OfType<IfcProject>().FirstOrDefault();
        }
    }

}
