using System;
using FluentAssertions;
using Neo4jExtensions.Tests.Model;
using NUnit.Framework;

namespace Neo4jExtensions.Tests
{
    public class CypherRelationTests
    {
        [Test]
        public void RelationTest()
        {
            var mainNodeBuilder = new NodeCypherBuilder<Tariff>().Match("x");
            Action<INodeCypherBuilder<Tariff>> depNodeBuilder = d => d.Match("dp");
            Action<IRelationCypherBuilder<Route>> relBuilder = r => r.To(depNodeBuilder, "r");

            mainNodeBuilder.Rel(relBuilder);
            var result = mainNodeBuilder.Build();

            result.Should().Be("(x:Tariff{ })-[r:ROUTE{ }]->(dp:Tariff{ })");
        }
    }
}