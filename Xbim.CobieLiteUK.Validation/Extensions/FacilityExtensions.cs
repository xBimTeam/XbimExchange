using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Extensions
{
    /// <summary>
    /// development support class
    /// </summary>
    internal static class FacilityExtensions
    {
        /// <summary>
        /// development support class; used to debug data provided.
        /// Not optimised; do not use for production purposes.
        /// </summary>
        internal static IEnumerable<string> GetClassifications(this Facility facility)
        {
            var l = new List<Tuple<string, string>>();
            foreach (var type in facility.AssetTypes)
            {
                if (type.Categories == null)
                    continue;

                foreach (var cat in type.Categories)
                {
                    var thisTuple = new Tuple<string, string>(cat.Classification, cat.Code);
                    if (!l.Contains(thisTuple))
                    {
                        l.Add(thisTuple);
                    }
                }
            }
            return l.Select(tuple => tuple.Item1 + "\t" + tuple.Item2);
        }
    }
}
