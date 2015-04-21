using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLiteUK;
using Xbim.DPoW;
using Contact = Xbim.DPoW.Contact;
using FacilityType = Xbim.COBieLiteUK.Facility;

namespace XbimExchanger.DPoWToCOBieLiteUK
{
    internal abstract class DpoWtoCoBieLiteUkMapping<TSourceType, TTargetType> :
        XbimMappings<PlanOfWork, FacilityType, string, TSourceType, TTargetType> where TTargetType : CobieObject, new()
    {
        protected override TTargetType Mapping(TSourceType source, TTargetType target)
        {
            if (Exchanger.SourceRepository.Client == null)
            {
                var client = new Contact {Email = "client@client.cli"};
                Exchanger.SourceRepository.ClientContactId = client.Id;
                if (Exchanger.SourceRepository.Contacts == null)
                    Exchanger.SourceRepository.Contacts = new List<Contact>();
                Exchanger.SourceRepository.Contacts.Add(client);
            }

            //set CreatedBy to Client
            // ReSharper disable once PossibleNullReferenceException
            var cKey = Exchanger.SourceRepository.Client.Id.ToString();
            var cMapping = Exchanger.GetOrCreateMappings<MappingContactToContact>();
            var tContact = cMapping.GetOrCreateTargetObject(cKey);
            target.CreatedBy = new ContactKey {Email = tContact.Email};

            //set CreatedOn
            target.CreatedOn = Exchanger.SourceRepository.CreatedOn;

            return target;
        }
    }
}