using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.CobieLiteUK.Validation.Extensions;

namespace Tests
{
    [TestClass]
    public class CobieLiteUKValidationExtraTests
    {
        [TestMethod]
        public void EnumerableStringToSringSuccedes()
        {
            const string value0 = @"Aa";
            const string value1 = @"Bb";

            var testStrings = new []
            {
                @"\",
                @",",
                @"\,",
                @"",
                @"Ciao, bella.",
                @",," ,
                @"\\",
                @"\,",
                @",\"
            }
            ;

            foreach (var str in testStrings)
            {
                var t = new List<String> {value0, str, value1};
                Test(t);
            }

            var l0 = new List<String>() { value0, value1 };
            Test(l0);

            
            Test(testStrings);
        }

        private static void Test(IEnumerable<string> startLista)
        {
            var startList = startLista.ToArray();
            var temporaryString = startList.ListToCompoundString();
            var rebuiltList = temporaryString.CompoundStringToList().ToArray();
            Assert.AreEqual(startList.Count(), rebuiltList.Count());

            var eStart = startList.GetEnumerator();
            var eRebuilt = rebuiltList.GetEnumerator();

            while (eStart.MoveNext() && eRebuilt.MoveNext())
            {
                Assert.AreEqual(eStart.Current, eRebuilt.Current);
            }
        }
    }
}
