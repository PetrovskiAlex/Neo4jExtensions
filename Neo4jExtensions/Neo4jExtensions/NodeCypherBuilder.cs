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

        public NodeCypherBuilder(string target)
        {
            _match = $"{target}:" + CypherExtensions.GetNodeName<T>();
        }
        
        public NodeCypherBuilder() : this("n")
        {
        }

        public void Rel<TRel>(string target, Action<IRelationCypherBuilder<TRel>> relBuilder = null)
        {
            var relationCypherBuilder = new RelationCypherBuilder<TRel>(target);

            relBuilder?.Invoke(relationCypherBuilder);

            _relation = relationCypherBuilder.Build();
        }

        public void Rel<TRel>(Action<IRelationCypherBuilder<TRel>> relBuilder = null)
        {
            Rel("r", relBuilder);
        }

        public INodeCypherBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            var pattern = CypherExtensions.BuildFilterPattern(predicate);
            _patterns.Add(pattern);

            return this;
        }

        public string Build()
        {
            return $"({_match}{{ {string.Join(", ", _patterns)}}}){_relation}";
        }
    }
}