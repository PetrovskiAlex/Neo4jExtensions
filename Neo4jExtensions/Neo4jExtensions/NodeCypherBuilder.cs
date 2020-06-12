using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Neo4jExtensions
{
    public class NodeCypherBuilder<T> : INodeCypherBuilder<T>
    {
        private string _match;
        private string _relation;
        private List<string> _patterns = new List<string>();

        public INodeCypherBuilder<T> Match(string target)
        {
            _match = $"{target}:" + CypherExtensions.GetNodeName<T>();
            return this;
        }

        public IRelationCypherBuilder<TRel> Rel<TRel>(Action<IRelationCypherBuilder<TRel>> relBuilder)
        {
            var relationCypherBuilder = new RelationCypherBuilder<TRel>();
            relBuilder(relationCypherBuilder);
            _relation = relationCypherBuilder.Build();

            return relationCypherBuilder;
        }

        public INodeCypherBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            var patterns = CypherExtensions.BuildFilterPatterns(new [] {predicate});
            _patterns.AddRange(patterns);

            return this;
        }

        public string Build()
        {
            return $"({_match}{{ {string.Join(", ", _patterns)}}}){_relation}";
        }
    }
}