using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xbim.COBieLiteUK;
using Xbim.Ifc2x3.DateTimeResource;
using Xbim.Ifc2x3.ExternalReferenceResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.IO;
using Xbim.XbimExtensions.SelectTypes;

namespace XbimExchanger.IfcToCOBieLiteUK
{
    internal class MappingIfcDocumentSelectToDocument : XbimMappings<XbimModel, List<Facility>, string, IfcDocumentSelect, Document> 
    {
        private CoBieLiteUkHelper Helper
        { get; set; }
        
       
        public HashSet<IfcDocumentInformation> ChainInstMap
        { get; set; }

        protected override Document Mapping(IfcDocumentSelect source, Document target)
        {
            throw new NotImplementedException(); //see MappingMulti method
        }

        /// <summary>
        /// Return a document object list, in case IfcDocumentInformation.DocumentReferences more than one file
        /// </summary>
        /// <param name="ifcDocumentSelect"></param>
        /// <returns></returns>
        public List<Document> MappingMulti(IfcDocumentSelect ifcDocumentSelect)
        {
            List<Document> docList = new List<Document>();
            Helper = ((IfcToCOBieLiteUkExchanger)Exchanger).Helper;
            //process IfcDocumentReference first
            if (ifcDocumentSelect is IfcDocumentReference)
            {
                var ifcDocumentReference = ifcDocumentSelect as IfcDocumentReference;
                if (ifcDocumentReference.ReferenceToDocument != null)
                {
                    docList.Add(ConvertToDocument(ifcDocumentReference, ifcDocumentReference.ReferenceToDocument.FirstOrDefault()));//ReferenceToDocument is a SET [0:1]
                    return docList;
                }
            }
            //must be IfcDocumentInformation
            var ifcDocumentInformation = ifcDocumentSelect as IfcDocumentInformation;
            if (ifcDocumentInformation.DocumentReferences != null)
            {
                foreach (IfcDocumentReference ifcDocumentReference in ifcDocumentInformation.DocumentReferences)
                {
                    docList.Add(ConvertToDocument(ifcDocumentReference, ifcDocumentInformation));
                }
            }
            //Do the children files
            var childDocList = GetChildDocs(ifcDocumentInformation);
            //link child documents to first document in the list
            var linkDoc = docList.FirstOrDefault();
            if (linkDoc != null)
            {
                linkDoc.Documents = childDocList;
            }
            else //no docs to link too, then just add to the root document list
            {
                docList.Concat(childDocList);
            }

            return docList;
        }

        /// <summary>
        /// Get the child documents with drill down into children of child....
        /// </summary>
        /// <param name="ifcDocumentInformation">IfcDocumentInformation</param>
        /// <returns>List of Document</returns>
        private List<Document> GetChildDocs( IfcDocumentInformation ifcDocumentInformation)
        {
           

            List<Document> childDocList = new List<Document>();
            var ChildRels = ifcDocumentInformation.IsPointer.FirstOrDefault();//SET[0:1] gets the relationship when ifcDocumentInformation is the RelatingDocument (parent) document
            if (ChildRels != null && ChildRels.RelatedDocuments != null)
            {
                foreach (IfcDocumentInformation item in ChildRels.RelatedDocuments)
                {
                    if (ChainInstMap == null) //set ChainInstMap, used to avoid infinite loop
                        ChainInstMap = new HashSet<IfcDocumentInformation>();

                    if (!ChainInstMap.Contains(item)) //check we have not already evaluated this IfcDocumentInformation
                    {
                        childDocList.AddRange(MappingMulti(item)); //drill down
                    }
                }
            }
            return childDocList;
        }

        
        /// <summary>
        /// Convert a IfcDocumentReference to Document
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <param name="document">Document Object</param>
        /// <returns>Document</returns>
        private Document ConvertToDocument(IfcDocumentReference ifcDocumentReference, IfcDocumentInformation ifcDocumentInformation)
        {
           
            string name = GetName(ifcDocumentInformation) ?? GetName(ifcDocumentReference);
            //fail to get from IfcDocumentReference, so try assign a default
            if (string.IsNullOrEmpty(name))
            {
                name = "Document";
            }
            //check for duplicates, if found add a (#) => "DocName(1)", if none return name unchanged
            name = GetNextName(name); 

            var document = new Document();
            document.Name= name;
            document.CreatedBy = ifcDocumentInformation != null ?  GetCreatedBy(ifcDocumentInformation) : null;
            document.CreatedOn = ifcDocumentInformation != null ? GetCreatedOn(ifcDocumentInformation) : null;

            document.Categories = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Purpose)) ? new List<Category>(new[] { new Category { Code = ifcDocumentInformation.Purpose } }) : null;

            //ApprovalBy will be reset to a string field as ContactKey field is incorrect but for now fill ContactKey.email
            //document.ApprovalBy = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.IntendedUse)) ? new ContactKey() { Email = ifcDocumentInformation.IntendedUse } : null;
            document.ApprovalBy = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.IntendedUse)) ? ifcDocumentInformation.IntendedUse : null; //once fixed
            
           document.Stage = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Scope)) ? ifcDocumentInformation.Scope : null;

            document.Directory = GetFileDirectory(ifcDocumentReference);
            document.File = GetFileName(ifcDocumentReference);
            
            document.ExternalSystem = null;
            document.ExternalEntity = ifcDocumentReference.GetType().Name;
            document.ExternalId = null;

            document.Description = (ifcDocumentInformation != null) && (!string.IsNullOrEmpty(ifcDocumentInformation.Description)) ? ifcDocumentInformation.Description.ToString() : null;
            document.Reference = (ifcDocumentInformation != null) && 
                                    (ifcDocumentInformation.DocumentId != null) && 
                                    (ifcDocumentInformation.DocumentId.Value != null) && 
                                    (!string.IsNullOrEmpty(ifcDocumentInformation.DocumentId.Value.ToString())) ? ifcDocumentInformation.DocumentId.Value.ToString() : null;
            Helper.DocumentProgress.Add(document);
            return document;
        }

        /// <summary>
        /// Get createdOn 
        /// </summary>
        /// <param name="ifcDocumentInformation"></param>
        /// <returns></returns>
        private DateTime? GetCreatedOn(IfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation.CreationTime != null)
            {
                var created = ifcDocumentInformation.CreationTime ?? ifcDocumentInformation.LastRevisionTime;
                if (created != null)
                {
                    return new DateTime(created.DateComponent.YearComponent, created.DateComponent.MonthComponent, created.DateComponent.DayComponent);
                }
                
            } 
            
            if (Helper.DocumentOwnerLookup.ContainsKey(ifcDocumentInformation))
            {
                return Helper.GetCreatedOn(Helper.DocumentOwnerLookup[ifcDocumentInformation]);
            }

            return DateTime.Now;
        }

        /// <summary>
        /// Get created by
        /// </summary>
        /// <param name="ifcDocumentInformation"></param>
        /// <returns></returns>
        private ContactKey GetCreatedBy(IfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation.DocumentOwner != null)
            {
                return Helper.GetCreatedBy(ifcDocumentInformation.DocumentOwner);
            }

            //get owner from the IfcRelAssociatesDocument object
            if (Helper.DocumentOwnerLookup.ContainsKey(ifcDocumentInformation))
            {
                return Helper.GetCreatedBy(Helper.DocumentOwnerLookup[ifcDocumentInformation]);
            }
            return null;
        }

        /// <summary>
        /// Get next name for duplicates
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetNextName(string name)
        {
            //do we have any matching names
            if (Helper.DocumentProgress != null && Helper.DocumentProgress.Any())
            {
                var found = Helper.DocumentProgress.Where(d => d.Name.StartsWith(name,StringComparison.OrdinalIgnoreCase)).Select(n => n.Name);

                if (found.Any())
                {
                    if ((found.Count() == 1) && (found.First().Length == name.Length)) //we match the whole name
                    {
                        return name + "(1)"; //first duplicate
                    }
                    var srch = name + "(";

                    //we have duplicates so get names that are in correct format
                    var correctFormat = found.Where(s => s.StartsWith(srch, StringComparison.OrdinalIgnoreCase) && s.EndsWith(")"));
                    if (correctFormat.Any())
                    {
                        var number = correctFormat.Max(s => GetNextNo(srch, s));//.OrderBy(s => s).LastOrDefault();
                        if (number > 0)
                        {
                            return srch + number.ToString() + ")";
                        }
                    }
                } 
            }
            //string is not found or we failed to add next number return input argument string
            return name;
        }
        /// <summary>
        /// Get next number from string in a format Name(#), so "This Document(10)" should return 11
        /// </summary>
        /// <param name="prefix">string up to  and including'(', such as "Name(" </param>
        /// <param name="number">string formated "Name(#)", such as "Name(10)" </param>
        /// <returns>int</returns>
        private int GetNextNo(string prefix, string number)
        {
            var start = prefix.Length;
            var lgth = number.Length - start - 1;
            number = number.Substring(start, lgth); //get the string between brackets
            var strNo = Regex.Match(number, @"\d+").Value;
            if (!string.IsNullOrEmpty(strNo))
            {
                int no;
                if (int.TryParse(strNo, out no))
                {
                    return ++no;
                }
            }
            return 0;
        }
        

        /// <summary>
        /// Get Name from IfcDocumentReference
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string or null</returns>
        private string GetName(IfcDocumentReference ifcDocumentReference)
        {
            if (ifcDocumentReference == null)
                return null;
            
            if (!string.IsNullOrEmpty(ifcDocumentReference.Name))
            {
                return ifcDocumentReference.Name;
            }
            if (!string.IsNullOrEmpty(ifcDocumentReference.Location))
            {
                return Path.GetFileNameWithoutExtension(ifcDocumentReference.Location);
            }
            //we ignore  ItemReference, "which refers to a system interpretable position within the document" from http://www.buildingsmart-tech.org/ifc/IFC2x3/TC1/html/ifcexternalreferenceresource/lexical/ifcdocumentreference.htm

            
            return null;
        }

        /// <summary>
        /// Get Name from IfcDocumentInformation
        /// </summary>
        /// <param name="ifcDocumentInformation">Document Information Object</param>
        /// <returns>string or null</returns>
        private string GetName(IfcDocumentInformation ifcDocumentInformation)
        {
            if (ifcDocumentInformation == null)
                return null;
            
            if (!string.IsNullOrEmpty(ifcDocumentInformation.Name))
            {
                return ifcDocumentInformation.Name;
            }

            return null;
        }

        /// <summary>
        /// Get the file directory/location
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string</returns>
        private string GetFileDirectory(IfcDocumentReference ifcDocumentReference)
        {
            if (ifcDocumentReference == null)
                return null;

            if (!string.IsNullOrEmpty(ifcDocumentReference.Location))
            {
                return ifcDocumentReference.Location;
            }
            return null;
        }
        /// <summary>
        /// Get file name
        /// </summary>
        /// <param name="ifcDocumentReference">Document Reference Object</param>
        /// <returns>string</returns>
        private string GetFileName(IfcDocumentReference ifcDocumentReference)
        {
            if (ifcDocumentReference == null)
                return null;
            if (!string.IsNullOrEmpty(ifcDocumentReference.Name))
            {
                return ifcDocumentReference.Name;
            }

            if (!string.IsNullOrEmpty(ifcDocumentReference.Location))
            {
                try
                {
                    return Path.GetFileName(ifcDocumentReference.Location);
                }
                catch (Exception) //if exception just return the stored string
                {
                    return ifcDocumentReference.Location;
                }
            }
            return null;
        }

    }
}
