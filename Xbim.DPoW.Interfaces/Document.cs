namespace Xbim.DPoW.Interfaces
{
    public class Document
    {
        public ScopeOfDocument Scope { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentURI { get; set; }
        public Classification DocumentCategory { get; set; }
        public string DocumentName { get; set; }
    }
}
