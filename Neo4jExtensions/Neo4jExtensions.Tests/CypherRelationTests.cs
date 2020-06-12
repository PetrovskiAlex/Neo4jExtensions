using FluentAssertions;
using Neo4jExtensions.Tests.Model;
using NUnit.Framework;

namespace Neo4jExtensions.Tests
{
    public class CypherRelationTests
    {
        [Test]
        public void SimpleToRelationTest()
        {
            var nodeBuilder = new NodeCypherBuilder<Tariff>();
            nodeBuilder.Rel<Route>(b => b.To<Tariff>("dp"));

            var result = nodeBuilder.Build();

            result.Should().Be("(n:Tariff{ })-[r:ROUTE{ }]->(dp:Tariff{ })");
        }

        [Test]
        public void SimpleFromRelationTest()
        {
            var nodeBuilder = new NodeCypherBuilder<Tariff>();
            nodeBuilder.Rel<Route>(b => b.From<Tariff>("dp"));

            var result = nodeBuilder.Build();

            result.Should().Be("(n:Tariff{ })<-[r:ROUTE{ }]-(dp:Tariff{ })");
        }

        [Test]
        public void RelationWithPatternsTest()
        {
            var nodeBuilder = new NodeCypherBuilder<Tariff>();
            nodeBuilder
                .Rel<Route>(b => b
                    .Where(r => r.Condition == RouteCondition.FOB)
                    .Where(r => r.Order == 1)
                    .To<Tariff>("dp"));

            var result = nodeBuilder.Build();

            result.Should().Be("(n:Tariff{ })-[r:ROUTE{ Condition : \"FOB\", Order : 1}]->(dp:Tariff{ })");
        }

        [Test]
        public void RelationWithDependentPatternsTest()
        {
            var nodeBuilder = new NodeCypherBuilder<Tariff>();
            nodeBuilder
                .Rel<Route>(b => b
                    .Where(r => r.Condition == RouteCondition.FOB)
                    .Where(r => r.Order == 1)
                    .To<Tariff>("dp", matchBuilder => matchBuilder.Where(t => t.Kind == Kind.Auto)));

            var result = nodeBuilder.Build();

            result.Should().Be("(n:Tariff{ })-[r:ROUTE{ Condition : \"FOB\", Order : 1}]->(dp:Tariff{ Kind : \"Auto\"})");
        }
    }
}