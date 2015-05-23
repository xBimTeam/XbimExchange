using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.CobieLiteUK.Validation.Extensions;
using Xbim.COBieLiteUK;
using Attribute = Xbim.COBieLiteUK.Attribute;

namespace Xbim.CobieLiteUK.Validation
{
    internal class DocumentsValidator : IValidator
    {
        private readonly List<Document> _requiredDocuments;
        private readonly Facility _destinationFacility;

        public DocumentsValidator(List<Document> requiredDocuments, Facility destinationFacility)
        {
            HasFailures = false;
            _requiredDocuments = requiredDocuments.Where(doc => doc.IsClassifiedAsRequirement()).ToList();
            _destinationFacility = destinationFacility;
        }
        public TerminationMode TerminationMode { get; set; }
        
        public bool HasFailures { get; private set; }

        internal IEnumerable<Document> ValidatedDocs(List<Document> list)
        {
            var dicDocs = new Dictionary<string, Document>();
            if (list != null)
            {
                try
                {
                    dicDocs = list.ToDictionary(doc => doc.Name, doc => doc);
                }
                catch 
                {
                    // creates a dictionary with first values, but also presents a document error.
                    HasFailures = true;
                    foreach (var document in list.Where(document => !dicDocs.ContainsKey(document.Name)))
                    {
                        dicDocs.Add(document.Name, document);
                    }
                }
                if (HasFailures) // it must be because of the try/catch earlier
                {
                    var d = new Document
                    {
                        Name = @"DPoW Validation Error",
                        CreatedBy = new ContactKey {Email = "Validation@DPoW"},
                        CreatedOn = DateTime.Now,
                        Categories = new List<Category>()
                        {
                            new Category() {Classification = "DPoW", Code = "Test Reports"},
                            FacilityValidator.FailedCat
                        },
                        Directory = "n/a",
                        File = "n/a",
                        Description = "Document names are not unique in submission.",
                    };
                    yield return d;
                }
            }
            foreach (var requiredDocument in _requiredDocuments)
            {
                if (TerminationMode == TerminationMode.StopOnFirstFail)
                    yield break;
                Document submitted = null;
                if (dicDocs.ContainsKey(requiredDocument.Name))
                {
                    submitted = dicDocs[requiredDocument.Name];
                }
                Document tmp;
                if (IsValid(submitted))
                {
                    tmp = _destinationFacility.Clone(submitted);
                    tmp.Categories = new List<Category> { FacilityValidator.PassedCat };
                    tmp.Attributes =
                        _destinationFacility.Clone(requiredDocument.Attributes as IEnumerable<Attribute>)
                            .ToList();
                }
                else
                {
                    tmp = _destinationFacility.Clone(requiredDocument);
                    tmp.Directory = submitted != null 
                        ? submitted.Directory 
                        : "";
                    tmp.File = submitted != null
                        ? submitted.File
                        : "";
                    tmp.Categories = new List<Category> { FacilityValidator.FailedCat };
                    HasFailures = true;
                }
                tmp.SetRequirementExternalSystem(requiredDocument.ExternalSystem);
                tmp.SetRequirementExternalId(requiredDocument.ExternalId);
                
                yield return tmp;
            }
        }

        private bool IsValid(Document submitted)
        {
            if (submitted == null)
                return false;
            if (IsInvalid(submitted.Directory) && IsInvalid(submitted.File))
                return false;
            return true;
        }

        private bool IsInvalid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
            return value == "n/a";
        }
    }
}
