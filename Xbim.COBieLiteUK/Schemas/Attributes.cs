using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAttribute = System.Attribute;

namespace Xbim.COBieLiteUK
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyMappingAttribute: SAttribute
    {
        public string Type;
        public string Column;
        public string Header;
        public bool Required;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParentMappingAttribute : SAttribute
    {
        public string Type;
        public string Column;
        public string Header;
        public string Path;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SheetMappingAttribute : SAttribute
    {
        public string Type;
        public string Sheet;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PathAttribute : SAttribute
    {
        public string Path;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PicklistAttribute : SAttribute
    {
        public string PicklistItem;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ListAttribute : SAttribute
    {
    }
}
