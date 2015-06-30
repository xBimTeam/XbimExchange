using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.COBie.Graph;

namespace Xbim.Cobie.Graph.Tests
{
    [TestClass]
    public class CobieGraphTests
    {
        [TestMethod]
        public void DatabaseConnection()
        {
            const string user = "neo4j";
            const string psw = "kolotoc";
            const string url = "http://localhost:7474/db/data";
            var client = ClientFactory.GetClient(user, psw, url);
        }

        [TestMethod]
        public void InsertionOfFacility()
        {
            const string user = "neo4j";
            const string psw = "kolotoc";
            const string url = "http://localhost:7474/db/data";
            var client = ClientFactory.GetClient(user, psw, url);

            var facility = new CobieSerializationTests().CreateSampleFacility();
            client.Cypher
                .Create("(f:Facility {data})")
                .WithParam("data", facility)
                .ExecuteWithoutResults();
        }
    }
}
