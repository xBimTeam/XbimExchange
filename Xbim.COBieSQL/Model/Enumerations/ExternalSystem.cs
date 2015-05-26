namespace Xbim.COBieSQL.Model.Enumerations
{
    public class ExternalSystem : ICobieEnumeration
    {
        public uint ExternalSystemId { get; set; }
        public string Name { get; set; }

        public uint FacilityId { get; set; }
        public Facility Facility { get; set; }
    }
}
