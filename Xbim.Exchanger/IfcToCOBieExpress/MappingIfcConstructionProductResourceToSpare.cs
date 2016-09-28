using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.CobieExpress;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieExpress
{
    internal class MappingIfcConstructionProductResourceToSpare : XbimMappings<IfcStore, IModel, int, IIfcConstructionProductResource, CobieSpare>
    {
        /// <summary>
        /// Helper
        /// </summary>
        private COBieExpressHelper Helper
        { get; set; }

        public IIfcTypeObject ParentObject
        { get; set; }
        /// <summary>
        /// List of created documents names, used to get next duplicate name
        /// </summary>
        private List<string> UsedNames
        { get; set; }

        /// <summary>
        /// Convert IfcConstructionProductResource to Spare
        /// </summary>
        /// <param name="source">IfcConstructionProductResource to convert</param>
        /// <param name="target">Empty Spare Object</param>
        /// <returns>Filled Spare Object</returns>
        protected override CobieSpare Mapping(IIfcConstructionProductResource source, CobieSpare target)
        {
            if (Helper == null) Helper = ((IfcToCoBieExpressExchanger)Exchanger).Helper;
            if (UsedNames == null) UsedNames = new List<string>();

            var name = (string.IsNullOrEmpty(source.Name)) ? "Resource" : source.Name.ToString();
            //check for duplicates, if found add a (#) => "Name(1)", if none return name unchanged
            name = Helper.GetNextName(name, UsedNames);
            target.Name = name;
            target.Created = Helper.GetCreatedInfo(source);

            var spareTypeCategory = Helper.GetCategories(source).FirstOrDefault();
            if (spareTypeCategory != null)
                target.SpareType = Helper.GetPickValue<CobieSpareType>(spareTypeCategory.Value);
            
            target.Suppliers.AddRange(GetSuppliers(source));
            target.ExternalObject = Helper.GetExternalObject(source);
            target.ExternalId = Helper.ExternalEntityIdentity(source);
            target.AltExternalId = source.GlobalId;
            target.ExternalSystem = Helper.GetExternalSystem(source);

            target.Description = GetDescription(source);
            target.SetNumber = GetSetNumber(source);
            target.PartNumber = GetPartNumber(source);
            return target;
        }

        /// <summary>
        /// Get the Supplier ContactKeys
        /// </summary>
        /// <param name="source">IfcConstructionProductResource object</param>
        /// <returns>List of suppliers ContactKeys</returns>
        private List<CobieContact> GetSuppliers(IIfcConstructionProductResource source)
        {
            var suppliers = new List<CobieContact>();
            var emailDelimited = string.Empty;

            //check IfcTypeObject
            var emails = Helper.GetCoBieProperty("SpareSuppliers", ParentObject);
            if (!string.IsNullOrEmpty(emails))
            {
                emailDelimited += emails;
            }

            //Check IfcConstructionProductResource Object
            var emailsResource = Helper.GetCoBieProperty("SpareSuppliers", source);
            if (!string.IsNullOrEmpty(emailsResource))
            {
                emailDelimited += emailsResource;
            }
            //Build list
            if (string.IsNullOrEmpty(emailDelimited)) return suppliers;
            var emailList = emailDelimited.Split(new[] { ':', ';', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var email in emailList)
            {
                var contact = Helper.GetOrCreateContact(email);
                if ((contact != null) && !suppliers.Contains(contact))
                {
                    suppliers.Add(contact);
                }
            }
            return suppliers;
        }

        /// <summary>
        /// Get the SetNumber string
        /// </summary>
        /// <param name="source">IfcConstructionProductResource object</param>
        /// <returns>SetNumber as a string</returns>
        private string GetSetNumber (IIfcConstructionProductResource source)
        {
            //check IfcTypeObject
            var setNo = Helper.GetCoBieProperty("SpareSetNumber", ParentObject);
            if (!string.IsNullOrEmpty(setNo))
            {
                return setNo;
            }

            //Check IfcConstructionProductResource Object
            setNo = Helper.GetCoBieProperty("SpareSetNumber", source);
            if (!string.IsNullOrEmpty(setNo))
            {
                return setNo;
            }
            return null;
        }

        /// <summary>
        /// Get the PartNumber string
        /// </summary>
        /// <param name="source">IfcConstructionProductResource object</param>
        /// <returns>PartNumber as a string</returns>
        private string GetPartNumber(IIfcConstructionProductResource source)
        {
            //check IfcTypeObject
            var partno = Helper.GetCoBieProperty("SparePartNumber", ParentObject);
            if (!string.IsNullOrEmpty(partno))
            {
                return partno;
            }

            //Check IfcConstructionProductResource Object
            partno = Helper.GetCoBieProperty("SparePartNumber", source);
            if (!string.IsNullOrEmpty(partno))
            {
                return partno;
            }
            return null;
        }

        /// <summary>
        /// Get the PartDescription string
        /// </summary>
        /// <param name="source">IfcConstructionProductResource object</param>
        /// <returns>Description</returns>
        private string GetDescription(IIfcConstructionProductResource source)
        {
            //Check IfcConstructionProductResource Object
            var desc = source.Description.ToString();
            if (!string.IsNullOrEmpty(desc))
            {
                return desc;
            }
            //"COBieDescription" -> support for COBie Toolkit for Autodesk Revit(had this in on old code, not sure if still relevant. this note date 8/10/2015)
            //Check IfcConstructionProductResource Object
            desc = Helper.GetCoBieProperty("CommonDescription", source);
            if (!string.IsNullOrEmpty(desc))
            {
                return desc;
            }
            
            //check IfcTypeObject
            desc = Helper.GetCoBieProperty("CommonDescription", ParentObject);
            if (!string.IsNullOrEmpty(desc))
            {
                return desc;
            }
            
            return null;

        }

        public override CobieSpare CreateTargetObject()
        {
            return Exchanger.TargetRepository.Instances.New<CobieSpare>();
        }
    }
}
