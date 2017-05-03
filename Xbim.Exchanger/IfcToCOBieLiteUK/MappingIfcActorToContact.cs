using System.Collections.Generic;
using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;
using XbimExchanger.CobieHelpers;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    public class ContactComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact x, Contact y)
        {
            return x.Email.Equals(y.Email);
        }

        public int GetHashCode(Contact obj)
        {
            return obj.Email.GetHashCode();
        }
    }

    internal class MappingIfcActorToContact : XbimMappings<IModel, List<Facility>, string, IIfcActorSelect, Contact>
    {
        protected override Contact Mapping(IIfcActorSelect actor, Contact target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            var personAndOrganization = actor as IIfcPersonAndOrganization;
            var person = actor as IIfcPerson;
            var organisation = actor as IIfcOrganization;
            if (personAndOrganization != null)
            {
                ConvertOrganisation(target, personAndOrganization.TheOrganization);
                ConvertPerson(target, personAndOrganization.ThePerson);

            }
            else if (person != null)
                ConvertPerson(target, person);
            else if (organisation != null)
                ConvertOrganisation(target, organisation);

            if (string.IsNullOrWhiteSpace(target.Email))
            {
               target.Email = ContactFunctions.DefaultUniqueEmail(actor);
            }
            target.CreatedBy = helper.GetCreatedBy(actor, true);
            target.CreatedOn = helper.GetCreatedOn(actor);
            if (target.Categories == null || !target.Categories.Any())
            {
                target.Categories = CoBieLiteUkHelper.UnknownCategory;
            }
            ////Attributes
            //AttributeType[] ifcAttributes = helper.GetAttributes(actor);
            //if (ifcAttributes != null && ifcAttributes.Length > 0)
            //    ContactAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
            //Documents no link available IfcActorSelect not inherited from IfcRoot
            return target;
        }
        
        public override Contact CreateTargetObject()
        {
            return new Contact();
        }


        internal static void ConvertOrganisation(Contact target, IIfcOrganization ifcOrganization)
        {
            // specific fields (different from ifcperson)
            target.Company = ifcOrganization.Name;

            // ========================= postal address
            //
            string department;
            string street;
            string postalBox;
            string town;
            string stateRegion;
            string postalCode;

            var c = ContactFunctions.GetPostal(ifcOrganization.Addresses,
                out department,
                out street,
                out postalBox,
                out town,
                out stateRegion,
                out postalCode
            );
            if (c > 0)
            {
                if (department != null) target.Department = department;
                if (street != null) target.Street = street;
                if (postalBox != null) target.PostalBox = postalBox;
                if (town != null) target.Town = town;
                if (stateRegion != null) target.StateRegion = stateRegion;
                if (postalCode != null) target.PostalCode = postalCode;
            }

            // ========================= telecom address
            //

            string email;
            string phone;

            var c2 = ContactFunctions.GetTelecom(ifcOrganization.Addresses,
                out email,
                out phone
            );
            if (c2 > 0)
            {
                if (email != null) target.Email = email;
                if (phone != null) target.Phone = phone;
            }

            // ========================= roles are used for classification
            //
            if (ifcOrganization.Roles == null)
                return;
            var roles = ifcOrganization.Roles;
            if (!roles.Any())
                return;
            if (target.Categories == null)
                target.Categories = new List<Category>(roles.Count());
            foreach (var role in roles)
                target.Categories.Add(new Category
                {
                    Classification = "Role",
                    Code = role.Role.ToString(),
                    Description = role.Description
                });
        }

        internal static void ConvertPerson(Contact target, IIfcPerson ifcPerson)
        {
            // specific fields (different from ifcorganisation)
            target.FamilyName = ifcPerson.FamilyName;
            target.GivenName = ifcPerson.GivenName;

            // ========================= postal address
            //
            string department;
            string street;
            string postalBox;
            string town;
            string stateRegion;
            string postalCode;

            var c = ContactFunctions.GetPostal(ifcPerson.Addresses,
                out department,
                out street,
                out postalBox,
                out town,
                out stateRegion,
                out postalCode
            );
            if (c > 0)
            {
                if (department != null) target.Department = department;
                if (street != null) target.Street = street;
                if (postalBox != null) target.PostalBox = postalBox;
                if (town != null) target.Town = town;
                if (stateRegion != null) target.StateRegion = stateRegion;
                if (postalCode != null) target.PostalCode = postalCode;
            }

            // ========================= telecom address
            //

            string email;
            string phone;

            var c2 = ContactFunctions.GetTelecom(ifcPerson.Addresses,
                out email,
                out phone
            );
            if (c2 > 0)
            {
                if (email != null) target.Email = email;
                if (phone != null) target.Phone = phone;
            }

            // ========================= roles are used for classification
            //
            if (ifcPerson.Roles == null)
                return;
            var roles = ifcPerson.Roles;
            if (!roles.Any())
                return;
            if (target.Categories == null)
                target.Categories = new List<Category>(roles.Count());
            foreach (var role in roles)
                target.Categories.Add(new Category
                {
                    Classification = "Role",
                    Code = role.Role.ToString(),
                    Description = role.Description
                });
        }


    }
}
