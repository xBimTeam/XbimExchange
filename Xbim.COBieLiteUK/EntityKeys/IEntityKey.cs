using System;

// ReSharper disable once CheckNamespace
namespace Xbim.CobieLiteUk
{
    public interface IEntityKey
    {
        Type ForType { get; }
        string Name { get; set; }
        string GetSheet(string mapping);
    }
}
