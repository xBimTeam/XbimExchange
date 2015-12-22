namespace Xbim.DPoW
{
    /// <summary>
    /// Arbitrary attributes which can be assigned to any object implementing abstract class DPoWAttributableObject
    /// </summary>
    public class Attribute
    {
        /// <summary>
        /// Name of attribute
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of attribute
        /// </summary>
        public string Definition { get; set; }
        /// <summary>
        /// Value of attribute. This is always string for the sake of simplicity
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Enumeration of basic value types. This is a hint for eventual conversion of string value to other types
        /// </summary>
        public ValueTypeEnum ValueType { get; set; }       
    }
}
