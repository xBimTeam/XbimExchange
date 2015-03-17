// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Net20Extension.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Implements code generation extension for .Net Framework 2.0
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xsd2Code.Library.Extensions
{
    /// <summary>
    /// Implements code generation extension for .Net Framework 2.0
    /// </summary>
    [CodeExtension(TargetFramework.Net20)]
    public class Net20Extension : CodeExtension
    {
        /// <summary>
        /// Create data contract attribute
        /// </summary>
        /// <param name="type">Code type declaration</param>
        /// <param name="schema">XML schema</param>
        protected override void CreateDataContractAttribute(System.CodeDom.CodeTypeDeclaration type, System.Xml.Schema.XmlSchema schema)
        {
            // No data contracts in the Net.20
        }
    }
}