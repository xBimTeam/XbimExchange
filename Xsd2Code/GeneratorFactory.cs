using System;
using System.Linq;
using Xsd2Code.Library.Extensions;
using Xsd2Code.Library.Helpers;
using System.Reflection;
using Xsd2Code.Library.Properties;

namespace Xsd2Code.Library
{
    /// <summary>
    /// Class factory
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Created 2009-05-28 by Pascal Cabanel
    /// </remarks>
    static class GeneratorFactory
    {
        /// <summary>
        /// Get <see cref="ICodeExtension"/> code generator extension
        /// </summary>
        /// <param name="generatorParams">Generator parameters</param>
        /// <returns><see cref="ICodeExtension"/></returns>
        internal static Result<ICodeExtension> GetCodeExtension(GeneratorParams generatorParams)
        {

            var extention = new Result<ICodeExtension>(GeneratorFactory.GetExtention(generatorParams.TargetFramework), true);
            if (extention.Entity != null)
                return extention;

            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Select(type => new
                {
                    Type = type,
                    TargetFrameworkAttributes =
                type.GetCustomAttributes(typeof(CodeExtensionAttribute), true)
                })

                .Where(o =>
                       o.TargetFrameworkAttributes.Length > 0 &&
                       o.TargetFrameworkAttributes
                           .Where(attr =>
                               ((CodeExtensionAttribute)attr).TargetFramework == generatorParams.TargetFramework)
                           .Count() > 0)
                           .ToList();

            if (types.Count == 1)
            {
                try
                {
                    return new Result<ICodeExtension>(Activator.CreateInstance(types[0].Type) as ICodeExtension, true);
                }
                catch (Exception ex)
                {
                    return new Result<ICodeExtension>(null, false, ex.Message, MessageType.Error);
                }
            }

            return new Result<ICodeExtension>(null, false,
                                              string.Format(Resources.UnsupportedTargetFramework,
                                                            generatorParams.TargetFramework),
                                              MessageType.Error);
        }

        internal static  ICodeExtension GetExtention(TargetFramework target)
        {
            switch (target)
            {
                case TargetFramework.Net20:
                    return new Net20Extension();
                case TargetFramework.Net30:
                    return new Net30Extension();
                case TargetFramework.Net35:
                    return new Net35Extension();
                case TargetFramework.Net40:
                    return new Net35Extension();
                case TargetFramework.Silverlight:
                    return new SilverlightExtension();
                case TargetFramework.CobieLiteUk:
                    return new CoBieLiteUkExtension();
            }
            return null;
        }
    }
}
