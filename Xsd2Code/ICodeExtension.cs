//-----------------------------------------------------------------------
// <copyright file="ICodeExtension.cs" company="Xsd2Code">
//     copyright Pascal Cabanel.
// </copyright>
//-----------------------------------------------------------------------

namespace Xsd2Code
{
    using System;

    /// <summary>
    /// Extention interface
    /// </summary>
    public interface ICodeExtension
    {
        /// <summary>
        /// Process method
        /// </summary>
        /// <param name="code">CodeNamespace type</param>
        /// <param name="schema">XSD XmlSchema object</param>
        void Process(System.CodeDom.CodeNamespace code, System.Xml.Schema.XmlSchema schema);
    }
}
