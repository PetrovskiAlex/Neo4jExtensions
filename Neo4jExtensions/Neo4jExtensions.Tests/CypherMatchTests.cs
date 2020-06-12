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
            var result = new NodeCypherBuilder<Tariff>().Build();

            result.Should().Be("(n:Tariff{ })");
        }

        [Test]
        public void MatchParamNameTest()
        {
            var result = new NodeCypherBuilder<Tariff>("t").Build();

            result.Should().Be("(t:Tariff{ })");
        }
        
        [Test]
        public void MatchWithPatternTest()
        {
            var builder = new NodeCypherBuilder<Tariff>();
            var result = builder
                .Where(t => t.Number == "number1")
                .Where(t => t.Kind == Kind.Auto)
                .Where(t => t.Version == 1)
                .Build();

            result.Should().Be("(n:Tariff{ Number : \"number1\", Kind : \"Auto\", Version : 1})");
        }
    }
}