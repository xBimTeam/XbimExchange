using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation
{
    internal class DocumentsValidator : IValidator
    {
        private readonly List<Document> _requiredDocuments;
        private readonly Facility _destinationFacility;

        public DocumentsValidator(List<Document> requiredDocuments, Facility destinationFacility)
        {
            HasFailures = false;
            _requiredDocuments = requiredDocuments;
            _destinationFacility = destinationFacility;
        }
        public TerminationMode TerminationMode { get; set; }
        
        public bool HasFailures { get; private set; }

        internal IEnumerable<Document> ValidatedDocs(List<Document> list)
        {
            var _dicDocs = new Dictionary<string, Document>();
            if (list != null)
            {
                try
                {
                    _dicDocs = list.ToDictionary(doc => doc.Name, doc => doc);
                }
                catch 
                {
                    // creates a dictionary with first values, but also presents a document error.
                    HasFailures = true;
                    foreach (var document in list.Where(document => !_dicDocs.ContainsKey(document.Name)))
                    {
                        _dicDocs.Add(document.Name, document);
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
                if (_dicDocs.ContainsKey(requiredDocument.Name))
                {
                    submitted = _dicDocs[requiredDocument.Name];
                }
                if (IsValid(submitted))
                {
                    var tmp = _destinationFacility.Clone(submitted);
                    if (tmp.Categories == null)
                        tmp.Categories = new List<Category>();
                    tmp.Categories.Add(FacilityValidator.PassedCat);
                    yield return tmp;
                }
                else
                {
                    var tmp = _destinationFacility.Clone(requiredDocument);
                    if (tmp.Categories == null)
                        tmp.Categories = new List<Category>();
                    tmp.Categories.Add(FacilityValidator.FailedCat);
                    HasFailures = true;
                    yield return tmp;
                }
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
