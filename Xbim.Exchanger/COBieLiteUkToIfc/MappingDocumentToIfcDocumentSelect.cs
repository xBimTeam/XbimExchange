using System;
using System.Linq;
using Xbim.CobieLiteUk;
using Xbim.Ifc2x3.DateTimeResource;
using Xbim.Ifc2x3.ExternalReferenceResource;

namespace XbimExchanger.COBieLiteUkToIfc
{
    public class MappingDocumentToIfcDocumentSelect : CoBieLiteUkIfcMappings<string, Document, IfcDocumentInformation>
    {
        protected override IfcDocumentInformation Mapping(Document document, IfcDocumentInformation ifcDocumentInformation)
        {
            ifcDocumentInformation.Name = document.Name;

            if (document.CreatedBy != null)
            {
                ifcDocumentInformation.DocumentOwner = Exchanger.SetEmailUser(document.CreatedBy.Email);
            }

            if (document.CreatedOn != null)
            {
                DateTime createOn = (DateTime) document.CreatedOn;
                var ifcCalanderDate = Exchanger.TargetRepository.Instances.New<IfcCalendarDate>(cd => { cd.DayComponent = createOn.Day; cd.MonthComponent = createOn.Month;  cd.YearComponent = createOn.Year; } );
                var ifcLocalTime = Exchanger.TargetRepository.Instances.New<IfcLocalTime>(lt => { lt.HourComponent = createOn.Hour; lt.MinuteComponent = createOn.Minute; lt.SecondComponent = createOn.Second; });
                ifcDocumentInformation.CreationTime = Exchanger.TargetRepository.Instances.New<IfcDateAndTime>(dt => { dt.DateComponent = ifcCalanderDate; dt.TimeComponent = ifcLocalTime; });
            }

            if (document.Categories != null && document.Categories.Any())
            {
                ifcDocumentInformation.Purpose = document.Categories.First().Code;
                
            }
            ifcDocumentInformation.IntendedUse = document.ApprovalBy;
            ifcDocumentInformation.Scope = document.Stage;

            var ifcDocumentReference = Exchanger.TargetRepository.Instances.New<IfcDocumentReference>(dr => { dr.Location = document.Directory; dr.Name = document.File; });
            ifcDocumentInformation.DocumentReferences.Add(ifcDocumentReference );
            ifcDocumentInformation.Description = document.Description;
            ifcDocumentInformation.DocumentId = document.Reference;

            //child docs so set up relationship
            if (document.Documents != null && document.Documents.Any())
            {
                var ifcDocumentInformationRelationship = Exchanger.TargetRepository.Instances.New<IfcDocumentInformationRelationship>();
                ifcDocumentInformationRelationship.RelatingDocument = ifcDocumentInformation;
                foreach (var doc in document.Documents)
                {
                    IfcDocumentInformation ifcDoc = Exchanger.TargetRepository.Instances.New<IfcDocumentInformation>();
                    ifcDoc = Mapping(doc, ifcDoc);
                    ifcDocumentInformationRelationship.RelatedDocuments.Add(ifcDoc);
                }
            }


            



            //using statement will set the Model.OwnerHistoryAddObject to IfcConstructionProductResource.OwnerHistory as OwnerHistoryAddObject is used to reset user history on any property changes, 
            //then swaps the original OwnerHistoryAddObject back in the dispose, so set any properties within the using statement to keep user history set in line above SetUserHistory
            //using (OwnerHistoryEditScope context = new OwnerHistoryEditScope(Exchanger.TargetRepository, ifcRelAssociatesDocument.OwnerHistory))
            //{

            //}


            return ifcDocumentInformation;
        }
    }
}
