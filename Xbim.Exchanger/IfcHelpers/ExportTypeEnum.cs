using System;

namespace XbimExchanger.IfcHelpers
{
    public enum ExportFormatEnum
    {
        /// <summary>
        /// Binary excel file
        /// </summary>
        XLS,
        /// <summary>
        /// Xml excel file
        /// </summary>
        XLSX,
        /// <summary>
        /// Json format
        /// </summary>
        JSON,
        /// <summary>
        /// Xml format
        /// </summary>
        XML,
        /// <summary>
        /// Ifc format
        /// </summary>
        IFC,
        /// <summary>
        /// Step21 format
        /// </summary>
        STEP21,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EntityIdentifierMode
    {
        /// <summary>
        /// Use the Entity Label in the Ifc file (e.g. #23)
        /// </summary>
        IfcEntityLabels = 0,
        /// <summary>
        /// Use the GlobalId of the Entity (e.g. "10mjSDZJj9gPS2PrQaxa3z")
        /// </summary>
        GloballyUniqueIds = 1,
        /// <summary>
        /// Does not write any External Identifier for Entities
        /// </summary>
        None = 2
    }

    /// <summary>
    /// Control what we extract from IFC as systems
    /// </summary>
    [Flags]
    public enum SystemExtractionMode
    {
        System = 0x1, //default and should always be set
        PropertyMaps = 0x2, //include properties as set by GetPropMap("SystemMaps")
        Types = 0x4, //include types as system listing all defined objects in componentnsnames
    }
}