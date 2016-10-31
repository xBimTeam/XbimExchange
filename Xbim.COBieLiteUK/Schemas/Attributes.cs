using System;
using SAttribute = System.Attribute;

// ReSharper disable once CheckNamespace
namespace Xbim.CobieLiteUk
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MappingAttribute: SAttribute
    {
        public string Type;
        public string Column;
        public string Header;
        public bool Required;
        public bool List;
        public string PickList;
        public string Path;
        public bool IsExtension = false;

        internal MappingAttribute Clone()
        {
            return (MappingAttribute)MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", Column, Header);
        }

        public string GetDefaultValue()
        {
            if (!string.IsNullOrWhiteSpace(PickList))
            {
                return "unknown";
            }
            if (IsExtension)
            {
                return "";
            }
            return "n/a";
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SheetMappingAttribute : SAttribute
    {
        public string Type;
        public string Sheet;
        public bool IsExtension = false;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParentAttribute : SAttribute
    {
        public Type DataType;
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AliasAttribute : SAttribute
    {
        public string Type;
        public string Value;
    }

}
