using System;
using System.Linq.Expressions;
using Neo4jExtensions.Tests.Model;
using NUnit.Framework;

namespace Neo4jExtensions.Tests
{
    public class CypherRelationTests
    {
        [Test]
        public void RelationTest()
        {
            //var mainNodeBuilder = new NodeCypherBuilder<Tariff>().Match("x");
            Expression<Func<INodeCypherBuilder<Tariff>, string>> depNodeBuilder = d => d.Match("dp").Build();
            Expression<Func<IRelationCypherBuilder<Route>, string>> relBuilder = r => r.To(depNodeBuilder, "r").Build();

            var s = relBuilder.Compile().Invoke(new RelationCypherBuilder<Route>());

            /*var result = mainNodeBuilder.Rel(relBuilder);

            result.Should().Be("(x:Tariff{ })-[r:ROUTE{ }]->(dp:Tariff{ })");*/
        }
    }
}