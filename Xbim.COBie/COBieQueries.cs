using Xbim.XbimExtensions;
using Xbim.Ifc2x3.ProductExtension;
using System.Xml;
using Xbim.COBie.Rows;
using Xbim.COBie.Data;
using System;
using System.Collections.Generic;

namespace Xbim.COBie
{
    

    public class COBieQueries
    {
       
        private COBieContext _context;

        private COBieSheet<COBieAttributeRow> _attributes;
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        public COBieQueries(COBieContext context)
        {
            _context = context;

            _attributes = new COBieSheet<COBieAttributeRow>(Constants.WORKSHEET_ATTRIBUTE); //create the attribute sheet
        }

        public COBieContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Creates Contact COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieContactRow></returns>
        public COBieSheet<COBieContactRow> GetCOBieContactSheet()
        {
            COBieDataContact contacts = new COBieDataContact(Context);
            return contacts.Fill();
        }
        
        /// <summary>
        /// Creates Document COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieDocumentRow></returns>
        public COBieSheet<COBieDocumentRow> GetCOBieDocumentSheet()
        {
            COBieDataDocument documents = new COBieDataDocument(Context);
            return documents.Fill();
        }
        
        /// <summary>
        /// Creates Impact COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieImpactRow></returns>
        public COBieSheet<COBieImpactRow> GetCOBieImpactSheet()
        {
            COBieDataImpact impacts = new COBieDataImpact(Context);
            return impacts.Fill();
        }

       
        /// <summary>
        /// Creates Issue COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieIssueRow></returns>
        public COBieSheet<COBieIssueRow> GetCOBieIssueSheet()
        {
            COBieDataIssue issues = new COBieDataIssue(Context);
            return issues.Fill();
        }

        /// <summary>
        /// Creates Job COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieJobRow></returns>
        public COBieSheet<COBieJobRow> GetCOBieJobSheet()
        {
            COBieDataJob jobs = new COBieDataJob(Context);
            return jobs.Fill();
        }
        
 
        /// <summary>
        /// Creates Resource COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieResourceRow></returns>
        public COBieSheet<COBieResourceRow> GetCOBieResourceSheet()
        {
            COBieDataResource resources = new COBieDataResource(Context);
            return resources.Fill();
        }

        
        /// <summary>
        /// Creates Floor COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieFloorRow></returns>
        public COBieSheet<COBieFloorRow> GetCOBieFloorSheet()
        {
            COBieDataFloor floors = new COBieDataFloor(Context);
            (floors as IAttributeProvider).InitialiseAttributes(ref _attributes);
            return floors.Fill();
        }
        
        /// <summary>
        /// Creates Space COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieSpaceRow></returns>
        public COBieSheet<COBieSpaceRow> GetCOBieSpaceSheet()
        {
            COBieDataSpace spaces = new COBieDataSpace(Context);
            (spaces as IAttributeProvider).InitialiseAttributes(ref _attributes);
            return spaces.Fill();
        }

        /// <summary>
        /// Creates Facility COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieFacilityRow></returns>
        public COBieSheet<COBieFacilityRow> GetCOBieFacilitySheet()
        {
            COBieDataFacility facilities = new COBieDataFacility(Context);
            (facilities as IAttributeProvider).InitialiseAttributes(ref _attributes);
            return facilities.Fill();
        }
        
        /// <summary>
        /// Creates Spare COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieSpareRow></returns>
        public COBieSheet<COBieSpareRow> GetCOBieSpareSheet()
        {
            COBieDataSpare spares = new COBieDataSpare(Context);
            (spares as IAttributeProvider).InitialiseAttributes(ref _attributes);
            return spares.Fill();
        }

        
        /// <summary>
        /// Creates Zone COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieZoneRow></returns>
        public COBieSheet<COBieZoneRow> GetCOBieZoneSheet()
        {
            COBieDataZone zones = new COBieDataZone(Context);
            (zones as IAttributeProvider).InitialiseAttributes(ref _attributes);
            return zones.Fill();
        }

        
        /// <summary>
        /// Creates Type COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieTypeRow></returns>
        public COBieSheet<COBieTypeRow> GetCOBieTypeSheet()
        {
            COBieDataType types = new COBieDataType(Context);
            (types as IAttributeProvider).InitialiseAttributes(ref _attributes);
            return types.Fill();
        }
        
        /// <summary>
        /// Creates Component COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieComponentRow></returns>
        public COBieSheet<COBieComponentRow> GetCOBieComponentSheet()
        {
            COBieDataComponent components = new COBieDataComponent(Context);
            (components as IAttributeProvider).InitialiseAttributes(ref _attributes);
            return components.Fill();
        }

        
        /// <summary>
        /// Creates System COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieSystemRow></returns>
        public COBieSheet<COBieSystemRow> GetCOBieSystemSheet(Dictionary<string, HashSet<string>> compIndices)
        {
            COBieDataSystem systems = new COBieDataSystem(Context);
            return systems.Fill(compIndices);
        }

        
        /// <summary>
        /// Creates Assembly COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieAssemblyRow></returns>
        public COBieSheet<COBieAssemblyRow> GetCOBieAssemblySheet()
        {
            COBieDataAssembly assemblies = new COBieDataAssembly(Context);
            return assemblies.Fill();
        }

        
        /// <summary>
        /// Creates Connection COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieConnectionRow></returns>
        public COBieSheet<COBieConnectionRow> GetCOBieConnectionSheet()
        {
            COBieDataConnection connections = new COBieDataConnection(Context);
            return connections.Fill();
        }

        
        /// <summary>
        /// Creates Coordinate COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieCoordinateRow></returns>
        public COBieSheet<COBieCoordinateRow> GetCOBieCoordinateSheet()
        {
            COBieDataCoordinate coordinates = new COBieDataCoordinate(Context);
            return coordinates.Fill();
        }

        
        /// <summary>
        /// Creates Attribute COBieSheet Data
        /// </summary>
        /// <param name="model">IModel to read data from</param>
        /// <returns>COBieSheet<COBieAttributeRow></returns>
        public COBieSheet<COBieAttributeRow> GetCOBieAttributeSheet()
        {
            return _attributes;
        }


        #region PickLists

       

        // Populate PickLists by column
        public COBieSheet<COBiePickListsRow> GetCOBiePickListsSheet(string pickListsXMLFilePath)
        {
            // read xml document for picklists
            if (string.IsNullOrEmpty(pickListsXMLFilePath)) pickListsXMLFilePath = "PickLists.xml";
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(pickListsXMLFilePath);
            XmlNodeList items = xdoc.SelectNodes("//PickLists//Item");

            COBieSheet<COBiePickListsRow> pickLists = new COBieSheet<COBiePickListsRow>(Constants.WORKSHEET_PICKLISTS);

            foreach (XmlNode node in items)
            {
                COBiePickListsRow pickList = new COBiePickListsRow(pickLists);
                XmlElement itemEle = (XmlElement)node;

                pickList.ApprovalBy = itemEle.GetElementsByTagName("ApprovalBy")[0].InnerText;
                pickList.AreaUnit = itemEle.GetElementsByTagName("AreaUnit")[0].InnerText;
                pickList.AssetType = itemEle.GetElementsByTagName("AssetType")[0].InnerText;
                pickList.CategoryFacility = itemEle.GetElementsByTagName("Category-Facility")[0].InnerText;
                pickList.CategorySpace = itemEle.GetElementsByTagName("Category-Space")[0].InnerText;
                pickList.CategoryElement = itemEle.GetElementsByTagName("Category-Element")[0].InnerText;
                pickList.CategoryProduct = itemEle.GetElementsByTagName("Category-Product")[0].InnerText;
                pickList.CategoryRole = itemEle.GetElementsByTagName("Category-Role")[0].InnerText;
                pickList.CoordinateSheet = itemEle.GetElementsByTagName("CoordinateSheet")[0].InnerText;
                pickList.ConnectionType = itemEle.GetElementsByTagName("ConnectionType")[0].InnerText;
                pickList.CoordinateType = itemEle.GetElementsByTagName("CoordinateType")[0].InnerText;
                pickList.DocumentType = itemEle.GetElementsByTagName("DocumentType")[0].InnerText;
                pickList.DurationUnit = itemEle.GetElementsByTagName("DurationUnit")[0].InnerText;
                pickList.FloorType = itemEle.GetElementsByTagName("FloorType")[0].InnerText;
                pickList.IssueCategory = itemEle.GetElementsByTagName("IssueCategory")[0].InnerText;
                pickList.IssueChance = itemEle.GetElementsByTagName("IssueChance")[0].InnerText;
                pickList.IssueImpact = itemEle.GetElementsByTagName("IssueImpact")[0].InnerText;
                pickList.IssueRisk = itemEle.GetElementsByTagName("IssueRisk")[0].InnerText;
                pickList.JobStatusType = itemEle.GetElementsByTagName("JobStatusType")[0].InnerText;
                pickList.JobType = itemEle.GetElementsByTagName("JobType")[0].InnerText;
                pickList.ObjAttribute = itemEle.GetElementsByTagName("objAttribute")[0].InnerText;
                pickList.ObjAttributeType = itemEle.GetElementsByTagName("objAttributeType")[0].InnerText;
                pickList.ObjComponent = itemEle.GetElementsByTagName("objComponent")[0].InnerText;
                pickList.ObjConnection = itemEle.GetElementsByTagName("objConnection")[0].InnerText;
                pickList.ObjContact = itemEle.GetElementsByTagName("objContact")[0].InnerText;
                pickList.ObjCoordinate = itemEle.GetElementsByTagName("objCoordinate")[0].InnerText;
                pickList.ObjDocument = itemEle.GetElementsByTagName("objDocument")[0].InnerText;
                pickList.ObjFacility = itemEle.GetElementsByTagName("objFacility")[0].InnerText;
                pickList.ObjFloor = itemEle.GetElementsByTagName("objFloor")[0].InnerText;
                pickList.ObjIssue = itemEle.GetElementsByTagName("objIssue")[0].InnerText;
                pickList.ObjJob = itemEle.GetElementsByTagName("objJob")[0].InnerText;
                pickList.ObjProject = itemEle.GetElementsByTagName("objProject")[0].InnerText;
                pickList.ObjResource = itemEle.GetElementsByTagName("objResource")[0].InnerText;
                pickList.ObjSite = itemEle.GetElementsByTagName("objSite")[0].InnerText;
                pickList.ObjSpace = itemEle.GetElementsByTagName("objSpace")[0].InnerText;
                pickList.ObjSpare = itemEle.GetElementsByTagName("objSpare")[0].InnerText;
                pickList.ObjSystem = itemEle.GetElementsByTagName("objSystem")[0].InnerText;
                pickList.ObjType = itemEle.GetElementsByTagName("objType")[0].InnerText;
                pickList.ObjWarranty = itemEle.GetElementsByTagName("objWarranty")[0].InnerText;
                pickList.ObjZone = itemEle.GetElementsByTagName("objZone")[0].InnerText;
                pickList.ResourceType = itemEle.GetElementsByTagName("ResourceType")[0].InnerText;
                pickList.SheetType = itemEle.GetElementsByTagName("SheetType")[0].InnerText;
                pickList.SpareType = itemEle.GetElementsByTagName("SpareType")[0].InnerText;
                pickList.StageType = itemEle.GetElementsByTagName("StageType")[0].InnerText;
                pickList.ZoneType = itemEle.GetElementsByTagName("ZoneType")[0].InnerText;
                pickList.LinearUnit = itemEle.GetElementsByTagName("LinearUnit")[0].InnerText;
                pickList.VolumeUnit = itemEle.GetElementsByTagName("VolumeUnit")[0].InnerText;
                pickList.CostUnit = itemEle.GetElementsByTagName("CostUnit")[0].InnerText;
                pickList.AssemblyType = itemEle.GetElementsByTagName("AssemblyType")[0].InnerText;
                pickList.ImpactType = itemEle.GetElementsByTagName("ImpactType")[0].InnerText;
                pickList.ImpactStage = itemEle.GetElementsByTagName("ImpactStage")[0].InnerText;
                pickList.ImpactUnit = itemEle.GetElementsByTagName("ImpactUnit")[0].InnerText;
                pickList.ObjAssembly = itemEle.GetElementsByTagName("objAssembly")[0].InnerText;
                pickList.ObjImpact = itemEle.GetElementsByTagName("objImpact")[0].InnerText;

                pickLists.Rows.Add(pickList);
            }

            return pickLists;
        }


        #endregion

    }
}
