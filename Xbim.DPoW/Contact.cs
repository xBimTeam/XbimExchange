using System;

namespace Xbim.DPoW
{
    public class Contact
    {
        public string ContactCompanyName { get; set; }
        public string ContactDepartmentName { get; set; }
        public string ContactTownName { get; set; }
        public string ContactRole { get; set; }
        public string ContactGivenName { get; set; }
        public string ContactFamilyName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactURL { get; set; }
        public string ContactStreet { get; set; }
        public string ContactPostCode { get; set; }
        public string ContactCountry { get; set; }
        public string ContactRegion { get; set; }
        public string ContactPostalBox { get; set; }

        public Guid Id { get; set; }

        public Contact()
        {
            Id = Guid.NewGuid();
        }
    }
}
