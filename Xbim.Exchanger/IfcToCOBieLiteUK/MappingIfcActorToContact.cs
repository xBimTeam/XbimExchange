using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.IO;
using Xbim.XbimExtensions.SelectTypes;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    class MappingIfcActorToContact : XbimMappings<XbimModel, List<Facility>, string, IfcActorSelect, Contact>
    {
        protected override Contact Mapping(IfcActorSelect actor, Contact target)
        {
            var helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            var personAndOrganization = actor as IfcPersonAndOrganization;
            var person = actor as IfcPerson;
            var organisation = actor as IfcOrganization;
            if (personAndOrganization != null)
            {
                ConvertOrganisation(target, personAndOrganization.TheOrganization, helper);
                ConvertPerson(target, personAndOrganization.ThePerson, helper);

            }
            else if (person != null)
                ConvertPerson(target, person, helper);
            else if (organisation != null)
                ConvertOrganisation(target, organisation, helper);

            ////Attributes
            //AttributeType[] ifcAttributes = helper.GetAttributes(actor);
            //if (ifcAttributes != null && ifcAttributes.Length > 0)
            //    ContactAttributes = new AttributeCollectionType { Attribute = ifcAttributes };
            return target;
        }


        private void ConvertOrganisation(Contact target, IfcOrganization ifcOrganization, CoBieLiteUkHelper helper)
        {
            if (ifcOrganization.Addresses != null)
            {
                var telecom = ifcOrganization.Addresses.OfType<IfcTelecomAddress>();
                var postal = ifcOrganization.Addresses.OfType<IfcPostalAddress>();
                var ifcTelecomAddresses = telecom as IfcTelecomAddress[] ?? telecom.ToArray();
                if (ifcTelecomAddresses.Any())
                {
                    var emailAddresses = string.Join(";", ifcTelecomAddresses.SelectMany(t => t.ElectronicMailAddresses));
                    if (!string.IsNullOrWhiteSpace(emailAddresses))
                        target.Email = emailAddresses;
                    var phoneNums = string.Join(";", ifcTelecomAddresses.SelectMany(t => t.TelephoneNumbers));
                    if (!string.IsNullOrWhiteSpace(phoneNums))
                        target.Phone = phoneNums;
                    //var url = string.Join(";", ifcTelecomAddresses.Where(p => p.WWWHomePageURL.HasValue).SelectMany(p => p.WWWHomePageURL.ToString()));
                    //if (!string.IsNullOrWhiteSpace(url))
                    //    URL= url;

                }

                var ifcPostalAddresses = postal as IfcPostalAddress[] ?? postal.ToArray();
                if (ifcPostalAddresses.Any())
                {
                    var deptNames = string.Join(";", ifcPostalAddresses.Where(p => p.InternalLocation.HasValue).SelectMany(p => p.InternalLocation.ToString()));
                    if (!string.IsNullOrWhiteSpace(deptNames))
                        target.Department = deptNames;
                    var streetNames = string.Join(";", ifcPostalAddresses.Where(p => p.AddressLines != null).SelectMany(p => p.AddressLines.ToString()));
                    if (!string.IsNullOrWhiteSpace(streetNames))
                        target.Street = streetNames;
                    var postalBox = string.Join(";", ifcPostalAddresses.Where(p => p.PostalBox.HasValue).SelectMany(p => p.PostalBox.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalBox))
                        target.PostalBox = postalBox;
                    var town = string.Join(";", ifcPostalAddresses.Where(p => p.Town.HasValue).SelectMany(p => p.Town.ToString()));
                    if (!string.IsNullOrWhiteSpace(town))
                        target.Town = town;
                    var region = string.Join(";", ifcPostalAddresses.Where(p => p.Region.HasValue).SelectMany(p => p.Region.ToString()));
                    if (!string.IsNullOrWhiteSpace(region))
                        target.StateRegion = region;
                    var postalCode = string.Join(";", ifcPostalAddresses.Where(p => p.PostalCode.HasValue).SelectMany(p => p.PostalCode.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalCode))
                        target.PostalCode = postalCode;
                }


            }
            if (ifcOrganization.Roles != null)
            {
                var roles = ifcOrganization.Roles;
                if (roles.Any())
                {
                    target.Categories = new List<Category>(roles.Count);
                    foreach (var role in roles)
                        target.Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                }
            }

            target.Company = ifcOrganization.Name;
            //if (string.IsNullOrWhiteSpace(target.Email))
            //{
            //    if (!string.IsNullOrWhiteSpace(target.FamilyName))
            //        target.Email = string.Format("{0}{1}@undefined.email", ifcOrganization..FamilyName, ifcOrganization.EntityLabel);
            //}

        }

        private void ConvertPerson(Contact target, IfcPerson ifcPerson, CoBieLiteUkHelper helper)
        {
            target.FamilyName = ifcPerson.FamilyName;
            target.GivenName = ifcPerson.GivenName;

            if (ifcPerson.Addresses != null)
            {
                var telecom = ifcPerson.Addresses.OfType<IfcTelecomAddress>();
                var postal = ifcPerson.Addresses.OfType<IfcPostalAddress>();
                var ifcTelecomAddresses = telecom as IfcTelecomAddress[] ?? telecom.ToArray();
                if (ifcTelecomAddresses.Any())
                {
                    var emailAddresses = string.Join(";", ifcTelecomAddresses.Where(t => t.ElectronicMailAddresses != null).SelectMany(t => t.ElectronicMailAddresses));
                    if (!string.IsNullOrWhiteSpace(emailAddresses))
                        target.Email = string.Join(";", emailAddresses, target.Email ?? "");
                    var phoneNums = string.Join(";", ifcTelecomAddresses.Where(t => t.TelephoneNumbers != null).SelectMany(t => t.TelephoneNumbers));
                    if (!string.IsNullOrWhiteSpace(phoneNums))
                        target.Phone = string.Join(";", phoneNums, target.Phone ?? "");
                    //var url = string.Join(";", ifcTelecomAddresses.Where(p => p.WWWHomePageURL.HasValue).SelectMany(p => p.WWWHomePageURL.ToString()));
                    //if (!string.IsNullOrWhiteSpace(url))
                    //    ContactURL = string.Join(";", url, ContactURL ?? ""); ;
                }

                var ifcPostalAddresses = postal as IfcPostalAddress[] ?? postal.ToArray();
                if (ifcPostalAddresses.Any())
                {
                    var deptNames = string.Join(";", ifcPostalAddresses.Where(p => p.InternalLocation.HasValue).SelectMany(p => p.InternalLocation.ToString()));
                    if (!string.IsNullOrWhiteSpace(deptNames))
                        target.Department = string.Join(";", deptNames, target.Department ?? "");
                    var streetNames = string.Join(";", ifcPostalAddresses.Where(p => p.AddressLines != null).SelectMany(p => p.AddressLines.ToString()));
                    if (!string.IsNullOrWhiteSpace(streetNames))
                        target.Street = string.Join(";", streetNames, target.Street ?? "");
                    var postalBox = string.Join(";", ifcPostalAddresses.Where(p => p.PostalBox.HasValue).SelectMany(p => p.PostalBox.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalBox))
                        target.PostalBox = string.Join(";", postalBox, target.PostalBox ?? "");
                    var town = string.Join(";", ifcPostalAddresses.Where(p => p.Town.HasValue).SelectMany(p => p.Town.ToString()));
                    if (!string.IsNullOrWhiteSpace(town))
                        target.Town = string.Join(";", town, target.Town ?? "");
                    var region = string.Join(";", ifcPostalAddresses.Where(p => p.Region.HasValue).SelectMany(p => p.Region.ToString()));
                    if (!string.IsNullOrWhiteSpace(region))
                        target.StateRegion = string.Join(";", region, target.StateRegion ?? "");
                    var postalCode = string.Join(";", ifcPostalAddresses.Where(p => p.PostalCode.HasValue).SelectMany(p => p.PostalCode.ToString()));
                    if (!string.IsNullOrWhiteSpace(postalCode))
                        target.PostalCode = string.Join(";", postalCode, target.PostalCode ?? "");
                }
            }
            if (ifcPerson.Roles != null)
            {
                var roles = ifcPerson.Roles;
                if (roles.Any())
                {
                    target.Categories = new List<Category>(roles.Count);
                    foreach (var role in roles)
                        target.Categories.Add(new Category { Classification = "Role", Code = role.Role.ToString(), Description = role.Description });
                }

            }

        }
    }
}
