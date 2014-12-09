using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ConstructionMgmtDomain;
using Xbim.XbimExtensions;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Resource tab.
    /// </summary>
    public class COBieDataResource : COBieData<COBieResourceRow>
    {

        /// <summary>
        /// Data Resource constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataResource(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Resource sheet
        /// </summary>
        /// <returns>COBieSheet<COBieResourceRow></returns>
        public override COBieSheet<COBieResourceRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Resources...");

            //create new sheet 
            COBieSheet<COBieResourceRow> resources = new COBieSheet<COBieResourceRow>(Constants.WORKSHEET_RESOURCE);

            // get all IfcConstructionEquipmentResource objects from IFC file
            IEnumerable<IfcConstructionEquipmentResource> ifcCer = Model.Instances.OfType<IfcConstructionEquipmentResource>();

            ProgressIndicator.Initialise("Creating Resources", ifcCer.Count());

            foreach (IfcConstructionEquipmentResource ifcConstructionEquipmentResource in ifcCer)
            {
                ProgressIndicator.IncrementAndUpdate();
                //if (ifcConstructionEquipmentResource == null) continue;

                COBieResourceRow resource = new COBieResourceRow(resources);
               
                resource.Name = (string.IsNullOrEmpty(ifcConstructionEquipmentResource.Name.ToString())) ? DEFAULT_STRING : ifcConstructionEquipmentResource.Name.ToString();
                resource.CreatedBy = GetTelecomEmailAddress(ifcConstructionEquipmentResource.OwnerHistory);
                resource.CreatedOn = GetCreatedOnDateAsFmtString(ifcConstructionEquipmentResource.OwnerHistory);
                resource.Category = (string.IsNullOrEmpty(ifcConstructionEquipmentResource.ObjectType.ToString())) ? DEFAULT_STRING : ifcConstructionEquipmentResource.ObjectType.ToString();
                resource.ExtSystem = GetExternalSystem(ifcConstructionEquipmentResource);
                resource.ExtObject = (ifcConstructionEquipmentResource == null) ? DEFAULT_STRING : ifcConstructionEquipmentResource.GetType().Name;
                resource.ExtIdentifier = ifcConstructionEquipmentResource.GlobalId;
                resource.Description = (string.IsNullOrEmpty(ifcConstructionEquipmentResource.Description)) ? DEFAULT_STRING : ifcConstructionEquipmentResource.Description.ToString();

                resources.AddRow(resource);
            }

            resources.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();
            return resources;
        }
        #endregion
    }
}
