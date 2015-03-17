using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Xsd2Code.Library.Helpers;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Code DOM Provider factory design pattern implementation
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Created 2009-02-18 by Ruslan Urban
    /// 
    /// </remarks>
    public static class CodeDomProviderFactory
    {
        /// <summary>
        /// Get Code DOM provider
        /// </summary>
        /// <param name="generatorParams"></param>
        /// <returns></returns>
        public static CodeDomProvider GetProvider(GeneratorParams generatorParams)
        {
            return GetProvider(generatorParams.Language);
        }

        /// <summary>
        /// Get Code DOM provider
        /// </summary>
        /// <returns></returns>
        public static CodeDomProvider GetProvider(GenerationLanguage language)
        {
            switch (language)
            {
                case GenerationLanguage.CSharp:
                    return new CSharpCodeProvider();

                case GenerationLanguage.VisualBasic:
                    return new VBCodeProvider();

                default:
                    throw new NotImplementedException(string.Format("Code provider for language {0} is not supported", Utility.GetEnumDescription(language)));
            }
        }
    }
}