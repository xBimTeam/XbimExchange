using System.ComponentModel;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Code generation language enumeration
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Created 2009-02-20 by Ruslan Urban
    ///     Code refactoring: moved enum into a separate file
    ///     Added Description attribute for use in error messages
    /// 
    /// </remarks>
    [DefaultValue(CSharp)]
    public enum GenerationLanguage
    {
        /// <summary>
        /// CShap language
        /// </summary>
        [Description("C#")]
        CSharp,

        /// <summary>
        /// Visual Basic language
        /// </summary>
        [Description("Visual Basic")]
        VisualBasic
    }
}