using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBieLite
{
    public static class SpatialStructureElementExtensions
    {

        /// <summary>
        ///   Adds the element to the set of elements which are contained in this spatialstructure
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
}
