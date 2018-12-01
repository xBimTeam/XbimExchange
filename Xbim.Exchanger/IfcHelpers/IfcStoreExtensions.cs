using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using Xbim.CobieLiteUk.FilterHelper;
using Xbim.Common;
using Xbim.Common.Federation;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcHelpers
{
    public static class IfcStoreExtensions
    {
        private static readonly ILogger Logger = XbimLogging.CreateLogger("XbimExchanger.IfcHelpers.IfcStoreExtensions");

        /// <summary>
        /// Get the file to roles information
        /// </summary>
        /// <returns>Dictionary or FileInfo to RoleFilter</returns>
        public static Dictionary<IReferencedModel, RoleFilter> GetFederatedFileRoles(this IfcStore model)
        {
            var modelRoles = new Dictionary<IReferencedModel, RoleFilter>();
            foreach (var refModel in model.ReferencedModels)
            {
                var docRole = MapActorRole(refModel.Role);
                try
                {
                    var file = new FileInfo(refModel.Name);
                    if (file.Exists)
                    {
                        // if was bare assignment before, I can't see how it might ever have worked.
                        if (modelRoles.ContainsKey(refModel))
                            modelRoles[refModel] = docRole;
                        else
                            modelRoles.Add(refModel, docRole);
                    }
                    else
                    {
                        Logger.LogError("File path does not exist: {path}", refModel.Name);    
                    }
                }
                catch (ArgumentException)
                {
                    Logger.LogError("File path is incorrect: {path}", refModel.Name);    
                }
            }
            return modelRoles;
        }

        private static RoleFilter MapActorRole(string roleName)
        {
            IfcRoleEnum role;
            if (!Enum.TryParse(roleName, true, out role)) 
                return RoleFilter.Unknown;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (role)
            {
                case IfcRoleEnum.ARCHITECT:
                    return RoleFilter.Architectural;
                case IfcRoleEnum.MECHANICALENGINEER:
                    return RoleFilter.Mechanical;
                case IfcRoleEnum.ELECTRICALENGINEER:
                    return RoleFilter.Electrical;
                default:
                    return RoleFilter.Unknown;
            }
        }
    }
}