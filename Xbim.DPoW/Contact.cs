using System;

namespace Xbim.DPoW
{
    public class Contact:DPoWAttributableObject
    {
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string TownName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string URL { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string PostalBox { get; set; }

        public Guid Id { get; set; }

        public Contact()
        {
            Id = Guid.NewGuid();
        }
    }
}
