namespace Xbim.DPoW
{
    /// <summary>
    /// This class represents required attribute from Digital Plan of Work
    /// </summary>
    public class RequiredAttribute 
    {
        /// <summary>
        /// Name of attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of attribute
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Property set name. This helps to convert data between DPoW, IFC and COBie
        /// </summary>
        public string PropertySetName { get; set; }
        
    }
}
