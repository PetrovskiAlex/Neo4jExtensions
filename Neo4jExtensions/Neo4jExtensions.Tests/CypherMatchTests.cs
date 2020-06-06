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

            Func<IRelationCypherBuilder<Route>, string> rel2RouteBuilder = x => x.Where(r => r.Order == 1).Build();

            Func<IMatchCypherBuilder<Location>, string> matchLocBuilder = m => m.Where(l => l.Id == locationId)
                .Rel(rel2RouteBuilder).Build();
            Func<IRelationCypherBuilder<Route>, string> relRouteBuilder = r => 
                r.Where(route => route.Order == 1).Where(route => route.Condition == RouteCondition.FOB)
                .To(matchLocBuilder).Build();

            var match = query.MatchBuilder<Tariff>()
                .Where(t => t.Number == "number").Where(t => t.Id == tariffId)
                .Where(t => t.Type == Kind.Auto).Where(t => t.Version == 1)
                .Rel(relRouteBuilder);
        }
    }
}