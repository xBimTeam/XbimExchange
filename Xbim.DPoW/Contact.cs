using System;

namespace Xbim.DPoW
{
    /// <summary>
    /// Contact represents person in DPoW. E-mail is essential field to fill in
    /// to keep COBie happy.
    /// </summary>
    public class Contact:DPoWAttributableObject
    {
        /// <summary>
        /// Company name
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Department name
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Town name
        /// </summary>
        public string TownName { get; set; }
        /// <summary>
        /// Given name
        /// </summary>
        public string GivenName { get; set; }
        /// <summary>
        /// Family name
        /// </summary>
        public string FamilyName { get; set; }
        /// <summary>
        /// E-mail is essential field to fill in. It is used as a reference value in COBie
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// URL representing person or company
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Street
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// Post code
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// Postal box
        /// </summary>
        public string PostalBox { get; set; }

        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Constructor initializes ID to new unique value
        /// </summary>
        public Contact()
        {
            Id = Guid.NewGuid();
        }
    }
}
