using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbim.CobieLiteUK.Validation.Extensions
{
    internal static class StringExtensions
    {
        internal static IEnumerable<string> CompoundStringToList(this string compoundString)
        {
            if (compoundString == null)
                yield break;

            var sb = new StringBuilder();
            var v = compoundString.ToCharArray();
            for (var i = 0; i < v.Length; i++)
            {
                switch (v[i])
                {
                    case ',':
                        yield return sb.ToString();
                        sb = new StringBuilder();
                        break;
                    case '\\':
                        try
                        {
                            var next = v[++i];
                            if (next != ',' && next != '\\')
                                throw new Exception("Invalid compound string.");
                            sb.Append(next);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid compound string.");
                        }
                        break;
                    default:
                        sb.Append(v[i]);
                        break;
                }
            }
            yield return sb.ToString();
        }


        internal static String ListToCompoundString(this IEnumerable<String> stringList)
        {
            if (stringList == null)
                return null;

            var asArray = stringList.ToArray();
            

            for (int i = 0; i < asArray.Length; i++)
            {
                asArray[i] = asArray[i].Replace(@"\", @"\\");
                asArray[i] = asArray[i].Replace(@",", @"\,");
            }

            return String.Join(",", asArray);
        }
    }
}
