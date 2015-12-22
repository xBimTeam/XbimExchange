using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.SharedBldgServiceElements;

namespace Xbim.COBie.Serialisers.XbimSerialiser
{
    public class COBieXBimConnection : COBieXBim
    {
        #region Properties
        private IEnumerable<IfcElement> IfcElements { get; set; }
        
        #endregion
        public COBieXBimConnection(COBieXBimContext xBimContext)
            : base(xBimContext)
        {

        }

        #region Methods
        /// <summary>
        /// Create and setup objects held in the Connection COBieSheet
        /// </summary>
        /// <param name="cOBieSheet">COBieSheet of COBieConnectionRow to read data from</param>
        public void SerialiseConnection(COBieSheet<COBieConnectionRow> cOBieSheet)
        {
            using (XbimReadWriteTransaction trans = Model.BeginTransaction("Add Connection"))
            {

                try
                {
                    int count = 1;
                    IfcElements = Model.Instances.OfType<IfcElement>();
                    
                    ProgressIndicator.ReportMessage("Starting Connections...");
                    ProgressIndicator.Initialise("Creating Connections", cOBieSheet.RowCount);
                    for (int i = 0; i < cOBieSheet.RowCount; i++)
                    {
                        BumpTransaction(trans, count);
                        count++;
                        ProgressIndicator.IncrementAndUpdate();
                        COBieConnectionRow row = cOBieSheet[i];
                        AddConnection(row);
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
        /// Add the data to the IfcRelConnectsElements object
        /// </summary>
        /// <param name="row">COBieConnectionRow holding the data</param>
        private void AddConnection(COBieConnectionRow row)
        {
            IfcElement relatingElement = null;
            IfcElement relatedElement = null;
            IfcRelConnectsElements ifcRelConnectsElements = null;
            if (ValidateString(row.RowName1))
                relatingElement = GetElement(row.RowName1);
           
            if (ValidateString(row.RowName2))
                relatedElement = GetElement(row.RowName2);

            //check on merge that we have not already created the IfcRelConnectsElements object
            ifcRelConnectsElements = CheckIfObjExistOnMerge<IfcRelConnectsElements>(row.Name).Where(rce => (rce.RelatingElement == relatingElement) && (rce.RelatedElement == relatedElement) ).FirstOrDefault();
            if (ifcRelConnectsElements != null)
            {
                return; //we have this object so return, make assumption that ports will also have exist!
            }

            if (ifcRelConnectsElements == null)
                ifcRelConnectsElements = Model.Instances.New<IfcRelConnectsElements>();
            
            //Add Created By, Created On and ExtSystem to Owner History. 
            SetUserHistory(ifcRelConnectsElements, row.ExtSystem, row.CreatedBy, row.CreatedOn);
                    
            //using statement will set the Model.OwnerHistoryAddObject to ifcRelConnectsElements.OwnerHistory as OwnerHistoryAddObject is used upon any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement
            using (COBieXBimEditScope context = new COBieXBimEditScope(Model, ifcRelConnectsElements.OwnerHistory))
            {
                if (ValidateString(row.Name)) ifcRelConnectsElements.Name = row.Name;
                if (ValidateString(row.ConnectionType)) ifcRelConnectsElements.Description = row.ConnectionType;

                if (relatingElement != null) 
                    ifcRelConnectsElements.RelatingElement = relatingElement;

                if (relatedElement != null)
                    ifcRelConnectsElements.RelatedElement = relatedElement;
                

                //Add Ports
                AddRelConnectsPorts(row.RealizingElement, row.PortName1, row.PortName2, relatingElement, relatedElement);
                
                //Add GlobalId
                AddGlobalId(row.ExtIdentifier, ifcRelConnectsElements);

                if (!ValidateString(ifcRelConnectsElements.Description)) ifcRelConnectsElements.Description = row.Description;
            }
            
        }

        /// <summary>
        /// Add the IfcRelConnectsPorts to the model
        /// </summary>
        /// <param name="realizingElement">List of IfcElement names</param>
        /// <param name="relatingPort">List of IfcPort Names</param>
        /// <param name="relatedPort">List of IfcPort Names</param>
        private void AddRelConnectsPorts(string realizingElement, string relatingPort, string relatedPort, IfcElement relatingElement, IfcElement relatedElement)
        {
            if (ValidateString(realizingElement) &&
                ValidateString(relatingPort) &&
                ValidateString(relatedPort)
                )
            {
                List<string> realizingElementList = SplitTheString(realizingElement);
                List<string> relatingPortList = SplitTheString(relatingPort);
                List<string> relatedPortList = SplitTheString(relatedPort);
                //check we have equal count for each list, if not we cannot create IfcRelConnectsPorts
                if (relatedPortList.Count() == relatingPortList.Count())
                {
                    for (int i = 0; i < relatedPortList.Count(); i++)
                    {
                        IfcDistributionPort ifcPortRelated = Model.Instances.New<IfcDistributionPort>(p => {p.Name = relatedPortList[i]; });
                        IfcDistributionPort ifcPortRelating = Model.Instances.New<IfcDistributionPort>(p => { p.Name = relatingPortList[i]; });
                        IfcRelConnectsPorts ifcRelConnectsPorts = Model.Instances.New<IfcRelConnectsPorts>(rcp => 
                                                                 { 
                                                                     rcp.RelatedPort = ifcPortRelated;
                                                                     rcp.RelatingPort = ifcPortRelating; 
                                                                 });
                        //see if we can add the RealizingElement, but only if list is in sync with port lists
                        if (realizingElementList.Count() == relatingPortList.Count())
                            ifcRelConnectsPorts.RealizingElement = GetElement(realizingElementList[i]);

                        //create the Relationship object
                        if (relatedElement != null)
                            Model.Instances.New<IfcRelConnectsPortToElement>(rcpe =>
                                {
                                    rcpe.RelatingPort = ifcPortRelated;
                                    rcpe.RelatedElement = relatedElement;
                                }
                                );
                        if (relatingElement != null)
                            Model.Instances.New<IfcRelConnectsPortToElement>(rcpe =>
                                {
                                    rcpe.RelatingPort = ifcPortRelating;
                                    rcpe.RelatedElement = relatingElement;
                                }
                                );
                    }
                }
            }

        }

        
        /// <summary>
        /// Get the IfcElement Object given the name
        /// </summary>
        /// <param name="name">Name of the IfcElement to extract</param>
        /// <returns>IfcElement object which has the passed name </returns>
        private IfcElement GetElement(string name)
        {
            name = name.Trim().ToLower();
            return IfcElements.Where(e => e.Name.ToString().ToLower().Trim() == name).FirstOrDefault();
        }

        #endregion
    }
}
