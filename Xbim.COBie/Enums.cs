using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.COBie
{
    public enum COBieAttributeState
    {
                                            // Responsibility Matrix key
        Required_PrimaryKey = 0,            // RP
        Required_CompoundKeyPart = 1,       // RC
        Required_Information = 2,           // RI

        Required_Reference_PrimaryKey = 3,  // RS
        Required_Reference_ForeignKey = 4,  // RF
        Required_Reference_PickList = 5,    // RL

        Required_System = 6,                // RA

        Required_IfSpecified = 7,           // RS (again)

        Required_System_IfSpecified = 8, //External system field, but optional eg ExtIdentifier
        Required_PrimaryKey_IfSpecified = 9, //for optional primary keys
        Required_Multi_PrimaryKey = 10 //for fields which have multiple primary keys, comma separated
    }

    public enum COBieAllowedType
    {
        AlphaNumeric,
        Email,
        ISODate,
        Numeric,
        Text,
        AnyType,
        ISODateTime
    }

    public enum COBieKeyType
    {
        PrimaryKey,
        CompoundKey,
        ForeignKey,
        CompoundKey_ForeignKey,
        None
    }

    public enum COBieCardinality
    {
        /// <summary>
        /// A relationship that is 1:N 
        /// </summary>
        /// <remarks>e.g. Components can have only one single Type</remarks>
        OneToMany,
        /// <summary>
        /// A relationship that is N:M
        /// </summary>
        /// <remarks>e.g. a Component can be assigned to one or more Spaces</remarks>
        ManyToMany
    }

    /// <summary>
    /// Merge Flags for roles in deciding if an object is allowed or discarded depending on the role of the model
    /// </summary>
    [Flags] //allows use to | and & values for multiple boolean tests
    public enum COBieMergeRoles
    {
        Unknown = 0x1,
        Architectural = 0x2,
        Mechanical = 0x4,
        Electrical = 0x8,
        Plumbing = 0x10,
        FireProtection = 0x20
       
    }
}
