using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ApprovalResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ControlExtension;
using Xbim.Ifc2x3.PropertyResource;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc2x3.ProductExtension;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Issue tab.
    /// </summary>
    public class COBieDataIssue : COBieData<COBieIssueRow>
    {
        /// <summary>
        /// Data Issue constructor
        /// </summary>
        /// <param name="context">The context of the model being generated</param>
        public COBieDataIssue(COBieContext context) : base(context)
        { }

        #region Methods
        /// <summary>
        /// Fill sheet rows for Issue sheet
        /// </summary>
        /// <returns>COBieSheet</returns>
        public override COBieSheet<COBieIssueRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Issues...");
            var ifcProject = Model.Instances.FirstOrDefault<IIfcProject>();
            Debug.Assert(ifcProject != null);

            //create new sheet
            var issues = new COBieSheet<COBieIssueRow>(Constants.WORKSHEET_ISSUE);

            //IEnumerable<IfcPropertySet> ifcProperties = Model.FederatedInstances.OfType<IfcPropertySet>().Where(ps => ps.Name.ToString() == "Pset_Risk");

            #region IFcApproval
            // get all IfcApproval objects from IFC file
            IEnumerable<IfcApproval> ifcApprovals = Model.FederatedInstances.OfType<IfcApproval>();
            ProgressIndicator.Initialise("Creating Issues (from IfcApprovals)", ifcApprovals.Count());

            List<IfcRelAssociatesApproval> ifcRelAssociatesApprovals = Model.FederatedInstances.OfType<IfcRelAssociatesApproval>().ToList();

            foreach (IfcApproval ifcApproval in ifcApprovals)
            {
                ProgressIndicator.IncrementAndUpdate();
                COBieIssueRow issue = new COBieIssueRow(issues);
                //get the associated property setIfcPropertySet
                var ifcPropertySet = ifcRelAssociatesApprovals
                                   .Where(ral => ral.RelatingApproval == ifcApproval)
                                   .SelectMany(ral => ral.RelatedObjects.OfType<IfcPropertySet>())
                                   .Where(ps => ps.Name == "Pset_Risk")
                                   .FirstOrDefault();

                List<IfcSimpleProperty> propertyList = new List<IfcSimpleProperty>();
                if (ifcPropertySet != null)
                    propertyList = ifcPropertySet.HasProperties.OfType<IfcSimpleProperty>().ToList();

                issue.Name = (string.IsNullOrEmpty(ifcApproval.Name)) ? DEFAULT_STRING : ifcApproval.Name.ToString();

                //lets default the creator to that user who created the project for now, no direct link to OwnerHistory on IfcApproval
                if (ifcPropertySet != null)
                {
                    //use "Pset_Risk" Property Set as source for this
                    issue.CreatedBy = GetTelecomEmailAddress(ifcPropertySet.OwnerHistory);
                    issue.CreatedOn = GetCreatedOnDateAsFmtString(ifcPropertySet.OwnerHistory);
                }
                else
                {
                    //if property set is null use project defaults
                    issue.CreatedBy = GetTelecomEmailAddress(ifcProject.OwnerHistory);
                    issue.CreatedOn = GetCreatedOnDateAsFmtString(ifcProject.OwnerHistory);
                }
                Interval propValues = GetPropertyEnumValue(propertyList, "RiskType");
                issue.Type = propValues.Value;

                propValues = GetPropertyEnumValue(propertyList, "RiskRating");
                issue.Risk = propValues.Value;

                propValues = GetPropertyEnumValue(propertyList, "AssessmentOfRisk");
                issue.Chance = propValues.Value;

                propValues = GetPropertyEnumValue(propertyList, "RiskConsequence");
                issue.Impact = propValues.Value;
                //GetIt(typeof(IfcApproval));
                //Risk assessment has to be on a task so we should have one
                List<IfcRoot> IfcRoots = GetIfcObjects(ifcApproval);
                issue.SheetName1 = (IfcRoots.Count > 0) ? GetSheetByObjectType(IfcRoots[0].GetType()) : DEFAULT_STRING;
                issue.RowName1 = (IfcRoots.Count > 0) ? IfcRoots[0].Name.ToString() : DEFAULT_STRING;

                //assuming that this row is a person associated with the ifcApproval, but might be a task
                string email = GetContact(ifcApproval);
                if (email == DEFAULT_STRING) //if no email, see if we have another ifcobject
                {
                    issue.SheetName2 = (IfcRoots.Count > 1) ? GetSheetByObjectType(IfcRoots[1].GetType()) : DEFAULT_STRING;
                    issue.RowName2 = (IfcRoots.Count > 1) ? IfcRoots[1].Name.ToString() : DEFAULT_STRING;
                }
                else
                {
                    issue.SheetName2 = (email != DEFAULT_STRING) ? Constants.WORKSHEET_CONTACT : DEFAULT_STRING;
                    issue.RowName2 = (email != DEFAULT_STRING) ? email : DEFAULT_STRING;
                }
                 
                issue.Description = (string.IsNullOrEmpty(ifcApproval.Description.ToString())) ? DEFAULT_STRING : ifcApproval.Description.ToString();

                propValues = GetPropertyEnumValue(propertyList, "RiskOwner");
                issue.Owner = propValues.Value;

                propValues = GetPropertyValue(propertyList, "PreventiveMeasures");
                issue.Mitigation = propValues.Value;

                issue.ExtSystem = (ifcPropertySet != null) ? GetExternalSystem(ifcPropertySet) : DEFAULT_STRING;
                issue.ExtObject = ifcApproval.GetType().Name;
                issue.ExtIdentifier = ifcApproval.Identifier.ToString();

                issues.AddRow(issue);
            }
            ProgressIndicator.Finalise();
            #endregion
            
            #region HS_Risk_UK
            // get all HS_Risk_UK Issues
            IEnumerable<IfcPropertySet> ifcProperties = Model.FederatedInstances.OfType<IfcPropertySet>().Where(ps => ps.Name.ToString() == "HS_Risk_UK");

            ProgressIndicator.Initialise("Creating Issues (from HS_Risk_UK psets)", ifcProperties.Count());

            foreach (IfcPropertySet propSet in ifcProperties)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieIssueRow issue = new COBieIssueRow(issues);
                List<IfcSimpleProperty> HSpropertyList = propSet.HasProperties.OfType<IfcSimpleProperty>().ToList();

                Interval propValues = GetPropertyValue(HSpropertyList, "RiskName");
                issue.Name = (propValues.Value == DEFAULT_STRING) ? propSet.Name.ToString() : propValues.Value.ToString();

                //
                //lets default the creator to that user who created the project for now, no direct link to OwnerHistory on IfcApproval
                if (propSet != null)
                {
                    //use "Pset_Risk" Property Set as source for this
                    issue.CreatedBy = GetTelecomEmailAddress(propSet.OwnerHistory);
                    issue.CreatedOn = GetCreatedOnDateAsFmtString(propSet.OwnerHistory);
                }
                else
                {
                    //if property set is null use project defaults
                    issue.CreatedBy = GetTelecomEmailAddress(ifcProject.OwnerHistory);
                    issue.CreatedOn = GetCreatedOnDateAsFmtString(ifcProject.OwnerHistory);
                }

                propValues = GetPropertyValue(HSpropertyList, "RiskCategory");
                issue.Type = propValues.Value;

                propValues = GetPropertyValue(HSpropertyList, "LevelOfRisk");
                issue.Risk = propValues.Value;

                propValues = GetPropertyValue(HSpropertyList, "RiskLikelihood");
                issue.Chance = propValues.Value;

                propValues = GetPropertyValue(HSpropertyList, "RiskConsequence");
                issue.Impact = propValues.Value;


                //TODO: We need to extend the functionality here as right now we make some assumptions:
                //1. The Issue SheetName1/RowName1 refers to a Component attached to the property set (it could be something else)
                //2. The component has an associated space, which makes up Sheetname2/Rowname2
                IfcRoot ifcRoot = GetAssociatedObject(propSet);
                issue.SheetName1 = GetSheetByObjectType(ifcRoot.GetType());
                issue.RowName1 = (!string.IsNullOrEmpty(ifcRoot.Name.ToString())) ? ifcRoot.Name.ToString() : DEFAULT_STRING;
             
                var SpaceBoundingBoxInfo = new List<SpaceInfo>();
                issue.RowName2 = COBieHelpers.GetComponentRelatedSpace(ifcRoot as IfcElement, Model, SpaceBoundingBoxInfo, Context);
                issue.SheetName2 = "Space";
                //End TODO

                propValues = GetPropertyValue(HSpropertyList, "RiskDescription");
                issue.Description = (propValues.Value == DEFAULT_STRING) ? propSet.Name.ToString() : propValues.Value.ToString();

                propValues = GetPropertyValue(HSpropertyList, "OwnerDiscipline");
                issue.Owner = propValues.Value;

                propValues = GetPropertyValue(HSpropertyList, "AgreedMitigation");
                issue.Mitigation = propValues.Value;

                issue.ExtSystem = (propSet != null) ? GetExternalSystem(propSet) : DEFAULT_STRING;
                issue.ExtObject = "HS_Risk_UK";
                issue.ExtIdentifier = propSet.GlobalId.ToString();//ifcApproval.Identifier.ToString();
                //

                issues.AddRow(issue);
            }

            issues.OrderBy(s => s.Name);

            ProgressIndicator.Finalise();
            #endregion

            #region PSet_Risk
            // get all PSet_Risk issues
            ifcProperties = Model.FederatedInstances.OfType<IfcPropertySet>().Where(ps => ps.Name.ToString() == "PSet_Risk");

            ProgressIndicator.Initialise("Creating Issues (from PSet_Risk)", ifcProperties.Count());

            foreach (IfcPropertySet propSet in ifcProperties)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieIssueRow issue = new COBieIssueRow(issues);
                List<IfcSimpleProperty> RiskpropertyList = propSet.HasProperties.OfType<IfcSimpleProperty>().ToList();

                Interval propValues = GetPropertyValue(RiskpropertyList, "RiskName");
                issue.Name = (propValues.Value == DEFAULT_STRING) ? propSet.Name.ToString() : propValues.Value.ToString();

                //TODO: Fill in the rest of these properties

                issues.AddRow(issue);
            }
            ProgressIndicator.Finalise();
            #endregion

            issues.OrderBy(s => s.Name);
            return issues;
        }

        //Fields for GetIfcObjects function
        List<IfcRelAssociatesApproval> ifcRelAssociatesApprovals = null;

        /// <summary>
        /// get all the IfcRoot objects attached to the ifcApproval
        /// </summary>
        /// <param name="ifcApproval">IfcApproval Object</param>
        /// <returns>List of IfcRoot Objects</returns>
        private List<IfcRoot> GetIfcObjects(IfcApproval ifcApproval)
        {
            List<IfcRoot> ifcRootObjs = new List<IfcRoot>();
            if (ifcRelAssociatesApprovals == null)
            {
                ifcRelAssociatesApprovals = Model.FederatedInstances.OfType<IfcRelAssociatesApproval>().ToList();
            }
            IEnumerable<IfcRoot> IfcRoots = ifcRelAssociatesApprovals.Where(ral => ral.RelatingApproval.EntityLabel == ifcApproval.EntityLabel)
                                            .SelectMany(ral => ral.RelatedObjects).OfType<IfcRoot>();
            foreach (IfcRoot item in IfcRoots)
            {
                if (item.GetType() != typeof(IfcPropertySet))
                {
                    ifcRootObjs.Add(item);
                }
            }
            
            return ifcRootObjs;
        }

        /// <summary>
        /// Get IfcPropertySet first associated object
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        private IfcRoot GetAssociatedObject(IfcPropertySet ps)
        {
            if ((ps.PropertyDefinitionOf.FirstOrDefault() != null) &&
                (ps.PropertyDefinitionOf.First().RelatedObjects.FirstOrDefault() != null)
                )
            {
                return ps.PropertyDefinitionOf.First().RelatedObjects.First();
            }
            if (ps.DefinesType.FirstOrDefault() != null)
            {
                return ps.DefinesType.FirstOrDefault();
            }

            return null;
        }

        //Fields for GetContact function
        List<IfcApprovalActorRelationship> ifcApprovalActorRelationships = null;

        private string GetContact(IfcApproval ifcApproval)
        {
            string eMail = DEFAULT_STRING;
            if (ifcApprovalActorRelationships == null)
            {
                ifcApprovalActorRelationships = Model.FederatedInstances.OfType<IfcApprovalActorRelationship>().ToList();
            }
            IfcPersonAndOrganization IfcPersonAndOrganization = ifcApprovalActorRelationships
                                                       .Where(aar => aar.Approval.EntityLabel == ifcApproval.EntityLabel)
                                                       .Select(aar => aar.Actor).OfType<IfcPersonAndOrganization>().FirstOrDefault();
        
            if(IfcPersonAndOrganization != null)
            {
                eMail = GetTelecomEmailAddress(IfcPersonAndOrganization);
            }

            return eMail;
        }

        #endregion
    }
}
