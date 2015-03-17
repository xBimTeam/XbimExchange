using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Xsd2Code.Library.Helpers
{
    /// <summary>
    /// Common utility methods 
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Created 2009-02-20 by Ruslan Urban
    ///     Moved some commonly used methods into this class and added new methods
    /// 
    /// </remarks>
    public static class Utility
    {
        /// <summary>
        /// Transform string to GenerationLanguage
        /// </summary>
        /// <param name="input">language string</param>
        /// <returns>GenerationLanguage object</returns>
        public static GenerationLanguage GetGenerationLanguage(string input)
        {
            if (string.IsNullOrEmpty(input)) return default(GenerationLanguage);

            switch (input.ToLower())
            {
                case "cs":
                case "c#":
                case "csharp":
                case "visualcsharp":
                    return GenerationLanguage.CSharp;
                case "vb":
                case "bas":
                case "visualbasic":
                    return GenerationLanguage.VisualBasic;
            }

            return ToEnum<GenerationLanguage>(input);
        }

        /// <summary>
        /// Convert user input to target platform enumeration
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns><see cref="TargetFramework"/></returns>
        public static TargetFramework GetTargetPlatform(string input)
        {
            if (string.IsNullOrEmpty(input)) return default(TargetFramework);

            switch (input.ToLower())
            {
                case "sl":
                case "silverlight":
                case "silverlight2":
                case "silverlight20":
                    return TargetFramework.Silverlight;
                case "2":
                case "20":
                case "2.0":
                case "net2":
                case "net20":
                case "net2.0":
                case "dotnet2":
                case "dotnet20":
                case "dotnet2.0":
                    return TargetFramework.Net35;
                case "3":
                case "30":
                case "3.0":
                case "net3":
                case "net30":
                case "net3.0":
                case "dotnet3":
                case "dotnet30":
                case "dotnet3.0":
                    return TargetFramework.Net30;
                case "35":
                case "3.5":
                case "net35":
                case "net3.5":
                case "dotnet35":
                case "dotnet3.5":
                    return TargetFramework.Net35;
            }

            return ToEnum<TargetFramework>(input);
        }


        /// <summary>
        /// Get generation language
        /// </summary>
        /// <param name="provider">Code DOM provider</param>
        /// <returns><see cref="GenerationLanguage"/></returns>
        static private GenerationLanguage GetGenerationLanguage(CodeDomProvider provider)
        {
            return GetGenerationLanguage(provider.FileExtension.Replace(".", ""));
        }

        /// <summary>
        /// Get output file path        
        /// </summary>
        /// <param name="xsdFilePath">Input file path</param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static string GetOutputFilePath(string xsdFilePath, CodeDomProvider provider)
        {
            /* DCM REMOVED: CodeDom Provider has FileExtension
            var language = GetGenerationLanguage(provider);
            return GetOutputFilePath(xsdFilePath, language);
             */ 
            return Path.ChangeExtension(xsdFilePath, ".designer." + provider.FileExtension);
        }

        /// <summary>
        /// Get output file path
        /// </summary>
        /// <param name="xsdFilePath">Input file path</param>
        /// <param name="language">Generation language</param>
        /// <returns>Generated file output path</returns>
        public static string GetOutputFilePath(string xsdFilePath, GenerationLanguage language)
        {
            return GetOutputFilePath(xsdFilePath, CodeDomProviderFactory.GetProvider(language));

            /* DCM REMOVED CodeDom Provider has FileExtension
            switch (language)
            {
                case GenerationLanguage.VisualBasic:
                    return Path.ChangeExtension(xsdFilePath, ".Designer.vb");
                case GenerationLanguage.VisualCpp:
                    return Path.ChangeExtension(xsdFilePath, ".Designer.cpp");
                default:
                    return Path.ChangeExtension(xsdFilePath, ".Designer.cs");
            }
             */ 
        }
        
        /// <summary>
        /// Gets the language extension based on the Passed language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static string GetLanguageExtension(GenerationLanguage language)
        {
            return CodeDomProviderFactory.GetProvider(language).FileExtension;
        }

        /// <summary>
        /// Convert string enumeration value name to actual enumeration value
        /// </summary>
        /// <typeparam name="T">Enumeration type</typeparam>
        /// <param name="name">Name of the enumeration value</param>
        /// <returns>Enumeration value of type <typeparamref name="T"/></returns>
        public static T ToEnum<T>(string name)
        {
            return ToEnum<T>(name, true);
        }

        /// <summary>
        /// Convert string enumeration value name to actual enumeration value
        /// </summary>
        /// <typeparam name="T">Enumeration type</typeparam>
        /// <param name="name">Name of the enumeration value</param>
        /// <param name="returnDefault">Flag that indicates that default enum value should be returned if <paramref name="name"/> value cannot be parsed</param>
        /// <returns>Enumeration value of type <typeparamref name="T"/></returns>
        public static T ToEnum<T>(string name, bool returnDefault)
        {
            if (!Enum.IsDefined(typeof (T), name))
            {
                if (returnDefault) return default(T);
                throw new ArgumentOutOfRangeException(string.Format("{0}...{1}", name, typeof (T)));
            }

            return (T) Enum.Parse(typeof (T), name, false);
        }

        /// <summary>
        /// Parse enum
        /// </summary>
        /// <typeparam name="T">Enumeration type</typeparam>
        /// <param name="name">Name of the enumeration value</param>
        /// <param name="defaultValue">Default return value if <paramref name="name"/> cannot be parsed</param>
        /// <returns>Enumeration value of type <typeparamref name="T"/></returns>
        public static T ToEnum<T>(string name, T defaultValue)
        {
            if (!Enum.IsDefined(typeof (T), name)) return defaultValue;
            try
            {
                return (T) Enum.Parse(typeof (T), name, false);
            }
            catch
            {
                return defaultValue;
            }
        }


        /// <summary>Get Enum description tag</summary>
        /// <param name="value">Enumeration value</param>
        /// <returns>Value of the [Description] attribute, or string value of the Enum field</returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }


        /// <summary>
        /// String to boolean static method 
        /// </summary>
        /// <param name="p">string to transform</param>
        /// <param name="defaultIfNull">if string is null use this default</param>
        /// <returns>booean result</returns>
        public static bool ToBoolean(string p, bool defaultIfNull)
        {
            if (string.IsNullOrEmpty(p)) return defaultIfNull;

            switch (p.Substring(0, 1).ToUpper())
            {
                case "T": // True in True|False (Most programming languages)
                case "1": // 1    in 1|0        (XML)
                case "Y": // Yes  in Yes|No     (Other cases)
                    return true;
            }
            return false;
        }        
        
        /// <summary>
        /// String to boolean static method 
        /// </summary>
        /// <param name="p">string to transform</param>
        /// <returns>booean result</returns>
        public static bool ToBoolean(string p)
        {
            return ToBoolean(p, false);
        }

    }
}