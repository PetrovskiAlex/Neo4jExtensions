using FluentAssertions;
using Neo4jExtensions.Tests.Model;
using NUnit.Framework;

namespace Neo4jExtensions.Tests
{
    public class CypherMatchTests
    {
        [Test]
        public void SimpleMatchTest()
        {
            var builder = new NodeCypherBuilder<Tariff>();
            var result = builder.Match("x").Build();

            result.Should().Be("(x:Tariff{ })");
        }

        [Test]
        public void MatchWithPatternTest()
        {
            var builder = new NodeCypherBuilder<Tariff>();
            var result = builder.Match("x")
                .Where(t => t.Number == "number1")
                .Where(t => t.Kind == Kind.Auto)
                .Where(t => t.Version == 1)
                .Build();

            result.Should().Be("(x:Tariff{ Number : \"number1\", Kind : \"Auto\", Version : 1})");
        }
    }
}