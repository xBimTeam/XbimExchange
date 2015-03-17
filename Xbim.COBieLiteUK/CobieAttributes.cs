using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.COBieLiteUK
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyMappingAttribute: Attribute
    {
        public string Type;
        public string Column;
        public string Header;
        public bool Required;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParentMappingAttribute : Attribute
    {
        public string Type;
        public string Column;
        public string Header;
        public string Path;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SheetMappingAttribute : Attribute
    {
        public string Type;
        public string Sheet;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PathAttribute : Attribute
    {
        public string Path;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PicklistAttribute : Attribute
    {
        public string PicklistItem;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ListAttribute : Attribute
    {
    }
}
