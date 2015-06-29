using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XbimExchanger.COBieLiteHelpers;

namespace Xbim.COBieLiteUK.FilterHelper
{
    public static class RoleIfcFilter
    {
        public static OutPutFilters AddRoleFilters (this OutPutFilters filters, MergeRoles roles)
        {
            OutPutFilters mergeFilter = new OutPutFilters();
            foreach (MergeRoles role in Enum.GetValues(typeof(MergeRoles)))
            {
                if (roles.HasFlag(role))
                {
                    switch (role)
                    {
                        case MergeRoles.Unknown:
                            break;
                        case MergeRoles.Architectural:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieArchitecturalFilters.config");
                            break;
                        case MergeRoles.Mechanical:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieMechanicalFilters.config");
                            break;
                        case MergeRoles.Electrical:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieElectricalFilters.config");
                            break;
                        case MergeRoles.Plumbing:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBiePlumbingFilters.config");
                            break;
                        case MergeRoles.FireProtection:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieFireProtectionFilters.config");
                            break;
                        default:
                            break;
                    }
                    filters.Merge(mergeFilter);
                }
                
            }

            return filters;
        }

        public static OutPutFilters AddRoleFilters(this OutPutFilters filters, MergeRoles roles)
        {
            OutPutFilters mergeFilter = new OutPutFilters();
            foreach (MergeRoles role in Enum.GetValues(typeof(MergeRoles)))
            {
                if (roles.HasFlag(role))
                {
                    switch (role)
                    {
                        case MergeRoles.Unknown:
                            break;
                        case MergeRoles.Architectural:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieArchitecturalFilters.config");
                            break;
                        case MergeRoles.Mechanical:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieMechanicalFilters.config");
                            break;
                        case MergeRoles.Electrical:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieElectricalFilters.config");
                            break;
                        case MergeRoles.Plumbing:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBiePlumbingFilters.config");
                            break;
                        case MergeRoles.FireProtection:
                            mergeFilter = new OutPutFilters("Xbim.COBieLiteUK.FilterHelper.COBieFireProtectionFilters.config");
                            break;
                        default:
                            break;
                    }
                    filters.Merge(mergeFilter);
                }

            }

            return filters;
        }

        public static OutPutFilters AddRoleFilters(this OutPutFilters filters, MergeRoles roles, Dictionary<MergeRoles, OutPutFilters> rolesFilters)
        {
            OutPutFilters mergeFilter = new OutPutFilters();
            foreach (MergeRoles role in Enum.GetValues(typeof(MergeRoles)))
            {
                if (roles.HasFlag(role))
                {
                    switch (role)
                    {
                        case MergeRoles.Unknown:
                            break;
                        case MergeRoles.Architectural:
                            mergeFilter = rolesFilters[role];
                            break;
                        case MergeRoles.Mechanical:
                            mergeFilter = rolesFilters[role];
                            break;
                        case MergeRoles.Electrical:
                            mergeFilter = rolesFilters[role];
                            break;
                        case MergeRoles.Plumbing:
                            mergeFilter = rolesFilters[role];
                            break;
                        case MergeRoles.FireProtection:
                            mergeFilter = rolesFilters[role];
                            break;
                        default:
                            break;
                    }
                    filters.Merge(mergeFilter);
                }

            }

            return filters;
        }

    }

    /// <summary>
    /// Merge Flags for roles in deciding if an object is allowed or discarded depending on the role of the model
    /// </summary>
    [Flags] //allows use to | and & values for multiple boolean tests
    public enum MergeRoles
    {
        Unknown = 0x1,
        Architectural = 0x2,
        Mechanical = 0x4,
        Electrical = 0x8,
        Plumbing = 0x10,
        FireProtection = 0x20

    }
}
