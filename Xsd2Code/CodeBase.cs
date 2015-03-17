using System.ComponentModel;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Code generation code base type enumeration
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Created 2009-02-20 by Ruslan Urban
    /// 
    /// </remarks>
    [DefaultValue(NetFX20)]
    public enum CodeBase
    {
        [Description(".Net Framework 2.0")]
        NetFX20,

        /* RU20090225: TODO: Implement WCF attribute generation
                [Description(".Net Framework 3.0")]
                Net30,
        */

        [Description("Silverlight 2.0")]
        Silverlight20
    }
}