using System.ComponentModel.DataAnnotations.Schema;

namespace Xbim.COBieSQL.Model.Enumerations
{
    public class ExternalSystem : ICobieEnumeration
    {
        public int ExternalSystemId { get; set; }
        public string Name { get; set; }

    }
}
