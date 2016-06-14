using System;

namespace Xbim.CobieLiteUk.FilterHelper
{
    /// <summary>
    /// Merge Flags for roles in deciding if an object is allowed or discarded depending on the role of the model
    /// </summary>
    [Flags] //allows use to | and & values for multiple boolean tests
    public enum RoleFilter
    {
        Unknown = 0x1,
        Architectural = 0x2,
        Mechanical = 0x4,
        Electrical = 0x8,
        Plumbing = 0x10,
        FireProtection = 0x20
    }

    public static class RoleFilterExtensions
    {
        public static string ToResourceName(this RoleFilter filter)
        {
            return filter == RoleFilter.Unknown 
                ? "Xbim.COBieLiteUK.FilterHelper.COBieDefaultFilters.config" 
                : string.Format("Xbim.COBieLiteUK.FilterHelper.COBie{0}Filters.config", filter);
        }
    }
}