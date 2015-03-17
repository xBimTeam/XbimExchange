using System.Collections.Generic;
using System.ComponentModel;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Collection type for generation
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Created 2009-02-20 by Ruslan Urban
    ///     Code refactoring: moved enum into a separate file
    /// 
    /// </remarks>
    [DefaultValue(List)]
    public enum CollectionType
    {
        /// <summary>
        /// Array collection
        /// </summary>
        Array,

        /// <summary>
        /// Generic List
        /// </summary>
        List,

        /// <summary>
        /// Generic IList
        /// </summary>
        IList,

        /// <summary>
        /// Provides a generic collection that supports data binding. Especially in WinForms
        /// </summary>
        BindingList,

        /// <summary>
        /// Generic list for notifications when items get added, removed, or when the whole list is refreshed
        /// </summary>
        ObservableCollection,

        /// <summary>
        /// Defined type for each specialized collection with designer and base files
        /// </summary>
        DefinedType
    }

    [DefaultValue(Default)]
    public enum PropertyNameSpecifiedType
    {
        /// <summary>
        /// Only for attributes and enumeration type
        /// </summary>
        Default,

        /// <summary>
        /// PropertyNameSpecified never generate
        /// </summary>
        None,

        /// <summary>
        /// PropertyNameSpecified whill be generate for all properties
        /// </summary>
        All
    }

    [DefaultValue(UTF8)]
    public enum DefaultEncoder
    {
        ASCII,
        Unicode,
        BigEndianUnicode,
        UTF8,
        UTF32,
        Default
    }
}