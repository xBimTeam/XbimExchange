using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation
{
    public class ProjectValidator
    {
        private Project _requirementsProject;
        public ProjectValidator(Project requirementsProject)
        {
            _requirementsProject = requirementsProject;
        }

        internal bool IsPass = true;

        public Project Validate(Project candidateProject)
        {
            var retP = new Project();
            if (candidateProject == null)
            {
                candidateProject = new Project() {Name = "Undefined", ExternalId = "Undefined"};
            }
            // check project name
            if (candidateProject.Name == _requirementsProject.Name)
            {
                retP.Name = candidateProject.Name;
            }
            else
            {
                retP.Name = string.Format("'{0}' (should be '{1}')", candidateProject.Name, _requirementsProject.Name);
                IsPass = false;
            }

            // check project ExternalId
            if (candidateProject.ExternalId == _requirementsProject.ExternalId)
            {
                retP.ExternalId = candidateProject.ExternalId;
            }
            else
            {
                retP.ExternalId = string.Format("{0} (should be '{1}')", candidateProject.ExternalId, _requirementsProject.ExternalId);
                IsPass = false;
            }
            return retP;
        }
    }
}
