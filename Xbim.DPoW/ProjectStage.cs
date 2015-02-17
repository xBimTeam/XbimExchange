using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.DPoW
{
    public class ProjectStage: DPoWAttributableObject
    {
        /// <summary>
        /// Name of this project stage
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Code of this projec stage
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Description of this project stage
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Unique ID of this project stage
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Space types defined at this stage
        /// </summary>
        public List<SpaceType> SpaceTypes { get; set; }
        /// <summary>
        /// Asset types defined at this project stage
        /// </summary>
        public List<AssetType> AssetTypes { get; set; }
        /// <summary>
        /// Assembly types defined at this stage
        /// </summary>
        public List<AssemblyType> AssemblyTypes { get; set; }
        /// <summary>
        /// Set of documentation defined at this stage
        /// </summary>
        public List<Documentation> DocumentationSet { get; set; }

        /// <summary>
        /// Initializes ID to new unique GUID
        /// </summary>
        public ProjectStage()
        {
            Id = Guid.NewGuid();
        }
    }
}
