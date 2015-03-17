// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeDomProviderFactory.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Code DOM Provider factory design pattern implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xsd2Code.Library.Helpers
{
    using System;
    using System.CodeDom.Compiler;
    using Microsoft.CSharp;
    using Microsoft.VisualBasic;

    /// <summary>
    /// Code DOM Provider factory design pattern implementation
    /// </summary>
    public static class CodeDomProviderFactory
    {
        /// <summary>
        /// Get Code DOM provider
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>CodeDom provider according language.</returns>
        public static CodeDomProvider GetProvider(GenerationLanguage language)
        {
            switch (language)
            {
                case GenerationLanguage.CSharp:
                    return new CSharpCodeProvider();

                case GenerationLanguage.VisualBasic:
                    return new VBCodeProvider();

                default:
                    throw new NotImplementedException(
                        string.Format(Properties.Resources.UnsupportedLanguageCodeDomProvider, Utility.GetEnumDescription(language)));
            }
        }
    }
}