using Xbim.COBieLite;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLite
{
    class MappingJobToIssueType : DPoWToCOBieLiteMapping<Job, IssueType>
    {
        protected override IssueType Mapping(Job source, IssueType target)
        {
            target.externalID = source.Id.ToString();
            target.IssueName = source.Name;
            target.IssueDescription = source.Description;
            
            //responsible person can only be saved as a set of attributes
            if (source.Responsibility != null)
            {
                var person = source.Responsibility.GetResponsibleContact(Exchanger.SourceRepository);
                var role = source.Responsibility.GetResponsibleRole(Exchanger.SourceRepository);
                //use person if defined. Only role otherwise
                if (person != null)
                {
                    var cMap = Exchanger.GetOrCreateMappings<MappingContactToContact>();
                    ContactType contact;
                    if (cMap.GetTargetObject(person.Id.ToString(), out contact))
                        target.ContactAssignment = new ContactKeyType() { ContactEmailReference = contact.ContactEmail };    
                }
                else if (role != null)
                {
                    var rMap = Exchanger.GetOrCreateMappings<MappingRoleToContact>();
                    ContactType contact;
                    if (rMap.GetTargetObject(role.Id.ToString(), out contact))
                        target.ContactAssignment = new ContactKeyType() { ContactEmailReference = contact.ContactEmail };    
                }
            }
            return target;
        }
    }
}
