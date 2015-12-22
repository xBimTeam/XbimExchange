using System;

// ReSharper disable once CheckNamespace
namespace Xbim.COBieLiteUK
{
    public interface IEntityKey
    {
        Type ForType { get; }
        string Name { get; set; }
        string GetSheet(string mapping);
    }
}
