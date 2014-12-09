using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc2x3.ActorResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.UtilityResource;
using Xbim.XbimExtensions;
//using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.COBie.Serialisers.XbimSerialiser;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Connection tab.
    /// </summary>
    public class COBieDataConnection : COBieData<COBieConnectionRow>
    {
        /// <summary>
        /// Data Connection constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataConnection(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Connection sheet
        /// </summary>
        /// <returns>COBieSheet<COBieConnectionRow></returns>
        public override COBieSheet<COBieConnectionRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Connections...");
            //Create new sheet
            COBieSheet<COBieConnectionRow> connections = new COBieSheet<COBieConnectionRow>(Constants.WORKSHEET_CONNECTION);

            // get all IfcRelConnectsElements objects from IFC file
            IEnumerable<IfcRelConnectsElements> ifcRelConnectsElements = Model.Instances.OfType<IfcRelConnectsElements>()
                                                                        .Where(rce => rce.RelatedElement != null &&
                                                                                      !Context.Exclude.ObjectType.Component.Contains(rce.RelatedElement.GetType()) &&
                                                                                      rce.RelatingElement != null &&
                                                                                      !Context.Exclude.ObjectType.Component.Contains(rce.RelatingElement.GetType())
                                                                                      );
            //get ifcRelConnectsPorts only if we have ifcRelConnectsElements
            IEnumerable<IfcRelConnectsPorts> ifcRelConnectsPorts = Enumerable.Empty<IfcRelConnectsPorts>();
            if (ifcRelConnectsElements.Count() > 0) ifcRelConnectsPorts = Model.Instances.OfType<IfcRelConnectsPorts>();

            ProgressIndicator.Initialise("Creating Connections", ifcRelConnectsElements.Count());

            int ids = 0;
            foreach (IfcRelConnectsElements ifcRelConnectsElement in ifcRelConnectsElements)
            {
                ProgressIndicator.IncrementAndUpdate();

                IfcElement relatingElement = ifcRelConnectsElement.RelatingElement;
                IfcElement relatedElement = ifcRelConnectsElement.RelatedElement;
                
                COBieConnectionRow conn = new COBieConnectionRow(connections);
                
                //try and get the IfcRelConnectsPorts first for relatingElement then for relatedElement
                IEnumerable<IfcRelConnectsPorts> ifcRelConnectsPortsElement = ifcRelConnectsPorts.Where(rcp => (rcp.RealizingElement != null) && ((rcp.RealizingElement == relatingElement) || (rcp.RealizingElement == relatedElement)));
                string connectionName = "";
                connectionName = (string.IsNullOrEmpty(ifcRelConnectsElement.Name)) ? "" : ifcRelConnectsElement.Name.ToString();
                

                conn.CreatedBy = GetTelecomEmailAddress(ifcRelConnectsElement.OwnerHistory);
                conn.CreatedOn = GetCreatedOnDateAsFmtString(ifcRelConnectsElement.OwnerHistory);
                conn.ConnectionType = GetComponentDescription(ifcRelConnectsElement);
                conn.SheetName = GetSheetByObjectType(relatingElement.GetType());

                conn.RowName1 = ((relatingElement != null) && (!string.IsNullOrEmpty(relatingElement.Name.ToString()))) ? relatingElement.Name.ToString() : DEFAULT_STRING;
                conn.RowName2 = ((relatedElement != null) && (!string.IsNullOrEmpty(relatedElement.Name.ToString()))) ? relatedElement.Name.ToString() : DEFAULT_STRING;
                //second attempt to get a name, if no IfcElement name then see if the associated type has a name
                //if (conn.RowName1 == DEFAULT_STRING) conn.RowName1 =  GetTypeName(relatingElement);
                //if (conn.RowName2 == DEFAULT_STRING) conn.RowName2 = GetTypeName(relatedElement);

                //try and get IfcRelConnectsPorts by using relatingElement then relatedElement is the RelizingElement, but this is optional property, but the IfcRelConnectsPorts object document states
                //"Each of the port is being attached to the IfcElement by using the IfcRelConnectsPortToElement relationship" and the IfcRelConnectsPortToElement is a inverse reference to HasPorts
                //on the IfcElement, so if no IfcRelConnectsPorts found for either Element, then check the HasPosts property of each element.
                List<string> realizingElement = new List<string>();
                List<string> relatedPort = new List<string>();
                List<string> relatingPort = new List<string>();
                foreach (IfcRelConnectsPorts port  in ifcRelConnectsPortsElement)
	            {
                    if ((string.IsNullOrEmpty(connectionName)) && (string.IsNullOrEmpty(port.Name))) connectionName = port.Name;
                    
                    if ((port.RealizingElement != null) && (!string.IsNullOrEmpty(port.RealizingElement.ToString())) ) //removed to allow export to xbim to keep sequence && (!realizingElement.Contains(port.RealizingElement.ToString()))
                        realizingElement.Add(port.RealizingElement.ToString());

                    if ((port.RelatedPort != null) && (!string.IsNullOrEmpty(port.RelatedPort.Name.ToString())) )//removed to allow export to xbim to keep sequence && (!relatedPort.Contains(port.RelatedPort.Name.ToString()))
                        relatedPort.Add(port.RelatedPort.Name.ToString());

                    if ((port.RelatingPort != null) && (!string.IsNullOrEmpty(port.RelatingPort.Name.ToString())) )//removed to allow export to xbim to keep sequence && (!relatingPort.Contains(port.RelatingPort.Name.ToString()))
                        relatingPort.Add(port.RelatingPort.Name.ToString());
	            }

                conn.RealizingElement = (realizingElement.Count > 0) ? COBieXBim.JoinStrings(':', realizingElement) : DEFAULT_STRING ;

                //no related port found so lets try and get from IfcElement.HasPorts
                if (relatedPort.Count == 0)
                {
                    IEnumerable<IfcRelConnectsPortToElement> relatedPorts = relatedElement.HasPorts.Where(rcpe => rcpe.RelatingPort != null);
                    foreach (IfcRelConnectsPortToElement port in relatedPorts)
                    {
                        if ((string.IsNullOrEmpty(connectionName)) && (string.IsNullOrEmpty(port.Name))) connectionName = port.Name;
                        if ((port.RelatingPort != null) && (!string.IsNullOrEmpty(port.RelatingPort.Name.ToString())) && (!relatedPort.Contains(port.RelatingPort.Name.ToString())))
                            relatedPort.Add(port.RelatingPort.Name.ToString());
                    }
                }
                //no relating port found so lets try and get from IfcElement.HasPorts
                if (relatingPort.Count == 0)
                {
                    IEnumerable<IfcRelConnectsPortToElement> relatingPorts = relatingElement.HasPorts.Where(rcpe => rcpe.RelatingPort != null);
                    foreach (IfcRelConnectsPortToElement port in relatingPorts)
                    {
                        if ((string.IsNullOrEmpty(connectionName)) && (string.IsNullOrEmpty(port.Name))) connectionName = port.Name;
                        if ((port.RelatingPort != null) && (!string.IsNullOrEmpty(port.RelatingPort.Name.ToString())) && (!relatedPort.Contains(port.RelatingPort.Name.ToString())))
                            relatingPort.Add(port.RelatingPort.Name.ToString());
                    }
                }

                conn.PortName1 = (relatingPort.Count > 0) ? COBieXBim.JoinStrings(':', relatingPort) : DEFAULT_STRING;
                conn.PortName2 = (relatedPort.Count > 0) ? COBieXBim.JoinStrings(':', relatedPort) : DEFAULT_STRING;
                
                conn.ExtSystem = GetExternalSystem(ifcRelConnectsElement);
                conn.ExtObject = ifcRelConnectsElement.GetType().Name;
                conn.ExtIdentifier = ifcRelConnectsElement.GlobalId;
                
                //if no ifcRelConnectsElement Name or Port names then revert to the index number 
                conn.Name = (string.IsNullOrEmpty(connectionName)) ? ids.ToString() : connectionName;
                conn.Description = (string.IsNullOrEmpty(ifcRelConnectsElement.Description)) ? DEFAULT_STRING : ifcRelConnectsElement.Description.ToString();

                connections.AddRow(conn);

                ids++;
            }

            connections.OrderBy(s => s.Name);
            
            ProgressIndicator.Finalise();

            return connections;
        }

        /// <summary>
        /// Get Description for passed in IfcElement
        /// </summary>
        /// <param name="ifcRelConnectsElement">Element holding description</param>
        /// <returns>string</returns>
        internal string GetComponentDescription(IfcRelConnectsElements ifcRelConnectsElement)
        {
            if (ifcRelConnectsElement != null)
            {
                if (!string.IsNullOrEmpty(ifcRelConnectsElement.Description)) return ifcRelConnectsElement.Description;
                else if (!string.IsNullOrEmpty(ifcRelConnectsElement.Name)) return ifcRelConnectsElement.Name;
            }
            return DEFAULT_STRING;
        }
        #endregion
    }
}
