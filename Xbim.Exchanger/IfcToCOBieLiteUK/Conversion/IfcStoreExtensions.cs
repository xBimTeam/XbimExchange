using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using Xbim.Common.Federation;
using Xbim.FilterHelper;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace XbimExchanger.IfcToCOBieLiteUK.Conversion
{
    public static class IfcStoreExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger("XbimExchanger.IfcToCOBieLiteUK.Conversion.IfcStoreExtensions");

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
                        Logger.ErrorFormat("File path does not exist: {0}", refModel.Name);    
                    }
                }
                catch (ArgumentException)
                {
                    Logger.ErrorFormat("File path is incorrect: {0}", refModel.Name);    
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