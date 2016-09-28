namespace Xbim.COBieLite
{
    public interface ICOBieObject
    {
        DocumentCollectionType Documents { get; set; }
        IssueCollectionType Issues { get; set; }
        AttributeCollectionType Attributes { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Category { get; set; }
        string Id { get; set; }
    }
}
