// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeExtensionAttribute.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Target framework attribute class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xsd2Code.Library.Extensions
{
    using System;

    /// <summary>
    /// Target framework attribute class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CodeExtensionAttribute : Attribute
    {
        /// <summary>
        /// Member field targetFrameworkField
        /// </summary>
        private readonly TargetFramework targetFrameworkField;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeExtensionAttribute"/> class.
        /// </summary>
        /// <param name="targetFramework">The target framework.</param>
        public CodeExtensionAttribute(TargetFramework targetFramework)
        {
            this.targetFrameworkField = targetFramework;
        }

        #region Property : TargetFramework

        /// <summary>
        /// Gets the target framework.
        /// </summary>
        /// <value>The target framework.</value>
        public TargetFramework TargetFramework
        {
            get { return this.targetFrameworkField; }
        }

        #endregion
    }
}