using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.DPoW
{
    public class ProjectStage
    {
        public string ProjectStageName { get; set; }
        public string ProjectStageCode { get; set; }
        public string ProjectStageDescription { get; set; }

        public Guid Id { get; set; }

        public List<SpaceTypes> SpaceTypes { get; set; }

        public List<AssetType> AssetTypes { get; set; }

        public List<AssemblyType> AssemblyTypes { get; set; }

        public List<Document> Documents { get; set; }
        public ProjectStage()
        {
            Documents = new List<Document>();
            SpaceTypes = new List<SpaceTypes>();
            AssetTypes = new List<AssetType>();
            AssemblyTypes = new List<AssemblyType>();
            Id = Guid.NewGuid();
        }
    }
}
