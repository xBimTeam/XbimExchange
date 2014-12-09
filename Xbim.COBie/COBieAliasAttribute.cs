using System;

namespace Xbim.COBie
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class COBieAliasAttribute : Attribute
    {
        public COBieAliasAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
