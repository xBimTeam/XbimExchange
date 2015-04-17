using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
