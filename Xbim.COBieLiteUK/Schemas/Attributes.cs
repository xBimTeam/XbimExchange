using System;
using SAttribute = System.Attribute;

namespace Xbim.COBieLiteUK
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
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SheetMappingAttribute : SAttribute
    {
        public string Type;
        public string Sheet;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParentAttribute : SAttribute
    {
        public Type DataType;
    }
}
