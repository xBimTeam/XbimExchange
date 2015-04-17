using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation
{
    internal class ProjectValidator : IValidator
    {
        private readonly Project _requirementsProject;
        public ProjectValidator(Project requirementsProject)
        {
            HasFailures = false;
            _requirementsProject = requirementsProject;
        }

        public TerminationMode TerminationMode { get; set; }

        public bool HasFailures { get; private set; }

        public Project Validate(Project candidateProject)
        {
            var retP = new Project();
            if (candidateProject == null)
            {
                candidateProject = new Project {Name = "Undefined", ExternalId = "Undefined"};
            }
            // check project name
            if (candidateProject.Name == _requirementsProject.Name)
            {
                retP.Name = candidateProject.Name;
            }
            else
            {
                retP.Name = string.Format("'{0}' (should be '{1}')", candidateProject.Name, _requirementsProject.Name);
                HasFailures = true;
            }

            // check project ExternalId
            if (candidateProject.ExternalId == _requirementsProject.ExternalId)
            {
                retP.ExternalId = candidateProject.ExternalId;
            }
            else
            {
                retP.ExternalId = string.Format("{0} (should be '{1}')", candidateProject.ExternalId, _requirementsProject.ExternalId);
                HasFailures = true;
            }
            return retP;
        }
    }
}
