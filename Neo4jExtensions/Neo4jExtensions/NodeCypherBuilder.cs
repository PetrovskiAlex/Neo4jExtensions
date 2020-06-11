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

        public IRelationCypherBuilder<TRel> Rel<TRel>(Expression<Func<IRelationCypherBuilder<TRel>, string>> relBuilder)
        {
            var relationCypherBuilder = new RelationCypherBuilder<TRel>();
            _relation = relBuilder.Compile().Invoke(relationCypherBuilder);
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