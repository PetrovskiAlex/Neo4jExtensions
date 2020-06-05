using System;
using Neo4jClient.Cypher;
using Neo4jExtensions.Tests.Model;
using NUnit.Framework;

namespace Neo4jExtensions.Tests
{
    public class CypherMatchTests
    {
        [Test]
        public void MatchTest()
        {
            IMatchCypherBuilder<Tariff> builder = null;

            var locationId = Guid.NewGuid();
            var tariffId = Guid.NewGuid();

            ICypherFluentQuery query = null;

            query = query.MatchBuilder<Tariff>()
                .Where(t => t.Number == "number").Where(t => t.Id == tariffId)
                .Where(t => t.Type == Kind.Auto).Where(t => t.Version == 1)
                .Rel<Route>()
                .Where(r => r.Order == 1).Where(r => r.Condition == RouteCondition.FOB)
                .To<Location>().Where(l => l.Id == locationId)
                .Build();
        }
    }
}