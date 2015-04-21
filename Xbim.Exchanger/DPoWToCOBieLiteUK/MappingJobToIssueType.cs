using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.DPoW;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    class MappingJobToIssueType : DpoWtoCoBieLiteUkMapping<Xbim.DPoW.Job, Issue>
    {
        protected override Issue Mapping(Xbim.DPoW.Job source, Issue target)
        {
            base.Mapping(source, target);

            target.ExternalId = source.Id.ToString();
            target.ExternalSystem = "DPoW";
            target.Name = source.Name;
            target.Description = source.Description;
            
            //responsible person can only be saved as a set of attributes
            if (source.Responsibility != null)
            {
                var person = source.Responsibility.GetResponsibleContact(Exchanger.SourceRepository);
                var role = source.Responsibility.GetResponsibleRole(Exchanger.SourceRepository);
                //use person if defined. Only role otherwise
                if (person != null)
                {
                    var cMap = Exchanger.GetOrCreateMappings<MappingContactToContact>();
                    Xbim.COBieLiteUK.Contact contact;
                    if (cMap.GetTargetObject(person.Id.ToString(), out contact))
                        target.Owner = new ContactKey() { Email = contact.Email };    
                }
                else if (role != null)
                {
                    var rMap = Exchanger.GetOrCreateMappings<MappingRoleToContact>();
                    Xbim.COBieLiteUK.Contact contact;
                    if (rMap.GetTargetObject(role.Id.ToString(), out contact))
                        target.Owner = new ContactKey { Email = contact.Email };    
                }
            }
            return target;
        }
    }
}
