//-----------------------------------------------------------------------
// <copyright file="NamespaceParam.cs" company="Xsd2Code">
//     copyright Pascal Cabanel.
// </copyright>
//-----------------------------------------------------------------------

namespace Xsd2Code.Library
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.ComponentModel;

    /// <summary>
    /// represent namespace parameter.
    /// </summary>
    public class NamespaceParam
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceParam"/> class.
        /// </summary>
        public NamespaceParam()
        {
        }

        /// <summary>
        /// Gets or sets the name space.
        /// </summary>
        /// <value>The name space.</value>
        [CategoryAttribute("Code"), DescriptionAttribute("namespace of generated file")]
        public string NameSpace
        {
            get;
            set;
        }
    }
}